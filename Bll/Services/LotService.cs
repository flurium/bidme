using Bll.Models;
using Dal.UnitOfWork;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Linq.Expressions;

namespace Bll.Services
{
  public class LotService
  {
    private readonly IUnitOfWork unitOfWork;
    private readonly LotCloserService lotCloser;
    private readonly LotImageService lotImageService;

    public LotService(IUnitOfWork unitOfWork, IEnumerable<IHostedService> hostedService, LotImageService lotImageService)
    {
      this.unitOfWork = unitOfWork;
      lotCloser = (LotCloserService)hostedService.First(s => s.GetType() == typeof(LotCloserService));
      this.lotImageService = lotImageService;
    }

    public async Task<Lot?> Create(Lot lot, IFormFileCollection? files)
    {
      lot.CloseTime = lot.CloseTime.ToUniversalTime();
      var res = await unitOfWork.LotRepository.CreateReturn(lot);
      if (res == null) return null;

      lotCloser.AddToWaitingOrder(new WaitingLot(res.Id, res.CloseTime));
      if (files != null) await lotImageService.Add(res, files);

      return res;
    }

    public async Task Delete(int id)
    {
      await unitOfWork.LotRepository.DeleteOne(id);
      await lotImageService.DeleteAll(id);
    }

    public async Task<IReadOnlyCollection<Lot>> FindByConditionAsync(Expression<Func<Lot, bool>> conditon)
    {
      return await unitOfWork.LotRepository.FindByConditionAsync(conditon);
    }

    public async Task<Lot?> GetOne(int id) => await unitOfWork.LotRepository.GetByIdWithImages(id);

    public async Task<Lot?> GetDetails(int id)
    {
      return await unitOfWork.LotRepository.GetByIdWithImagesOrders(id);
    }

    public async Task<IReadOnlyCollection<Lot>> FilterLots(LotFilter filter)
    {
      List<Expression<Func<Lot, bool>>> conditions = new();

      if (filter.Name != null)
        conditions.Add(l => l.Name.ToLower().Contains(filter.Name.ToLower()));

      if (filter.Categories.Count > 0)
        conditions.Add(l => filter.Categories.Contains(l.Category.Name));

      if (filter.MinPrice != null)
        conditions.Add(l => l.Price >= filter.MinPrice);

      if (filter.MaxPrice != null)
        conditions.Add(l => l.Price <= filter.MaxPrice);

      return await unitOfWork.LotRepository.FindByConditions(conditions);
    }

    public async Task<IReadOnlyCollection<Category>> GetCategories(List<string> current)
    {
      return await unitOfWork.CategoryRepository.OrderDescending(c => current.Contains(c.Name));
    }

    public async Task<bool> MakeBid(double amount, int lotId, string userId)
    {
      var order = await unitOfWork.OrderRepository.MaxPrice(lotId);

      if (order == null || order.OrderPrice < amount)
      {
        var userOrder = (await unitOfWork.OrderRepository.FindByConditionAsync(x => x.UserId == userId)).FirstOrDefault();
        if (userOrder != null)
        {
          userOrder.OrderPrice = amount;
          await unitOfWork.OrderRepository.Edit(userOrder);
          return true;
        }
        await unitOfWork.OrderRepository.Create(new Order { OrderPrice = amount, LotId = lotId, UserId = userId });
        return true;
      }

      return false;
    }

    public async Task Edit(string description, int lotId)
    {
      var lot = await unitOfWork.LotRepository.GetById(lotId);

      if (lot != null)
      {
        lot.Description = description;
      }
      else return;
      await unitOfWork.LotRepository.Edit(lot);
    }

    public async Task Expired(int lotId)
    {
      var lot = await unitOfWork.LotRepository.GetById(lotId);
      if (lot == null) return;
      await unitOfWork.LotRepository.UpdateStatus(lot.Id, true);
    }
  }
}