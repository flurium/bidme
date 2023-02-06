using Bll.Models;
using Dal.UnitOfWork;
using Domain.Models;
using System.Linq.Expressions;

namespace Bll.Services
{
    public class LotService
    {
        private readonly IUnitOfWork unitOfWork;

        public LotService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        public async Task<Lot> CreateAsync(Lot product)
        {
            if (product != null)
            {
                return await unitOfWork.LotRepository.CreateAsync(product);
            }
            return null;
        }

        public async Task Delete(int id)
        {
            await unitOfWork.LotRepository.Delete(id);
        }

        public async Task<IReadOnlyCollection<Lot>> FindByConditionAsync(Expression<Func<Lot, bool>> conditon)
        {
            return await unitOfWork.LotRepository.FindByConditionAsync(conditon);
        }
      
        public async Task<Lot> FirstOrDefault(Expression<Func<Lot, bool>> conditon)
        {
            return await unitOfWork.LotRepository.FirstOrDefault(conditon);
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
            //if (current == null) return await unitOfWork.CategoryRepository.GetAllAsync();
            return await unitOfWork.CategoryRepository.OrderDescending(c => current.Contains(c.Name));
        }

        public async Task<bool> MakeBid(double bid, int lotId, string userId)
        {
           
            var order =await unitOfWork.OrderRepository.MaxPrice(lotId);
           
            
            if((order == null)||(order.OrderPrice < bid))
            {
                var user = await unitOfWork.OrderRepository.FindByConditionAsync(x=>x.UserId==userId);
                if (user != null)
                {
                    var userOrder=user.First();
                    userOrder.OrderPrice = bid;
                    await unitOfWork.OrderRepository.Edit(userOrder);
                    return true;
                }
                await unitOfWork.OrderRepository.CreateAsync(new Order { OrderPrice = bid, LotId = lotId, UserId = userId });
                return true;

            }

            return false;
            
           
        }

        public async Task Edit(string description, int lotId)
        {
            var lot = await unitOfWork.LotRepository.GetByIdAsync(lotId);

            if (lot != null)
            {
                lot.Description = description;
            }
            else return;
            await unitOfWork.LotRepository.Edit(lot);
        }
    }
}