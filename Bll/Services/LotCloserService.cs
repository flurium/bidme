using Bll.Models;
using Dal.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bll.Services
{
    /// <summary>
    /// Close lots when end time is now
    /// </summary>
    public class LotCloserService : BackgroundService
    {
        private ILotRepository lotRepository;
        private LinkedList<WaitingLot> order;
        private IServiceProvider services;

        public LotCloserService(IServiceProvider services)
        {
            this.services = services;
        }

        public void AddToWaitingOrder(WaitingLot lot)
        {
            var place = FindPlaceForNew(lot.CloseTime);
            if (place != null) order.AddAfter(place, lot);
            else order.AddFirst(lot);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = services.CreateScope();
            lotRepository = scope.ServiceProvider.GetRequiredService<ILotRepository>();
            await Load();
            await Look(stoppingToken);
        }

        private LinkedListNode<WaitingLot>? FindPlaceForNew(DateTime time)
        {
            var last = order.Last;
            while (last != null)
            {
                if (last.Value.CloseTime <= time) return last;
                last = last.Previous;
            }
            return null;
        }

        private async Task Load()
        {
            var lots = await lotRepository.FindMany(l => !l.IsClosed, l => l.CloseTime, false);
            var waitingLots = lots.Select(l => new WaitingLot(l.Id, l.CloseTime));
            order = new(waitingLots);
        }

        private async Task Look(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var current = order.First;
                while (current != null && current.Value.CloseTime <= DateTime.Now)
                {
                    await lotRepository.UpdateStatus(current.Value.Id, true);
                    order.Remove(current);
                    current = current.Next;
                }
                await Task.Delay(60000, stoppingToken);
            }
        }
    }
}