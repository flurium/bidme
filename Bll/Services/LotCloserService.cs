using Bll.Models;
using Dal.Context;
using Dal.Repository.Interfaces;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.Services
{
    /// <summary>
    /// Close lots when end time is now
    /// </summary>
    public class LotCloserService : BackgroundService
    {
        private readonly IServiceProvider services;

        public LotCloserService(IServiceProvider services) => this.services = services;

        public LinkedList<WaitingLot> Order { get; private set; }

        public void AddToOrder(WaitingLot lot)
        {
            var place = FindPlaceForNew(lot.CloseTime);
            if (place != null) Order.AddAfter(place, lot);
            else Order.AddFirst(lot);
        }

        private LinkedListNode<WaitingLot>? FindPlaceForNew(DateTime time)
        {
            var last = Order.Last;
            while (last != null)
            {
                if (last.Value.CloseTime <= time) return last;
                last = last.Previous;
            }
            return null;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = services.CreateScope();
            var lotRepository = scope.ServiceProvider.GetRequiredService<ILotRepository>();

            await Load(lotRepository);
            await Look(lotRepository, stoppingToken);
        }

        /// <summary>
        /// Load not closed lots from db
        /// </summary>
        private async Task Load(ILotRepository lotRepository)
        {
            // todo order by time
            var lots = await lotRepository.FindMany(l => !l.IsClosed, l => l.CloseTime, false);
            var waitingLots = lots.Select(l => new WaitingLot(l.Id, l.CloseTime));
            Order = new(waitingLots);
        }

        private async Task Look(ILotRepository lotRepository, CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var current = Order.First;
                while (current != null && current.Value.CloseTime <= DateTime.Now)
                {
                    await lotRepository.UpdateStatus(current.Value.Id, true);
                    Order.Remove(current);
                    current = current.Next;
                }
                await Task.Delay(60000, stoppingToken);
            }
        }
    }
}