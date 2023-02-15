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

        public async Task<Lot?> Create(CreateLotInput lot, IFormFileCollection? files)
        {
            lot.CloseTime = lot.CloseTime.ToUniversalTime();

            int categoryId;

            if (lot.NewCategoryName != null && lot.NewCategoryName.Trim() != string.Empty)
            {
                var trimedNewName = lot.NewCategoryName.Trim();
                var category = await unitOfWork.CategoryRepository.FindOne(c => c.Name.ToLower() == trimedNewName.ToLower() && c.ParentId == lot.CategoryId);
                category ??= await unitOfWork.CategoryRepository.CreateReturn(new() { Name = trimedNewName, ParentId = lot.CategoryId });

                if (category == null) return null;
                categoryId = category.Id;
            }
            else
            {
                if (lot.CategoryId == null) return null;
                categoryId = lot.CategoryId.Value;
            }

            var res = await unitOfWork.LotRepository.CreateReturn(new Lot
            {
                Name = lot.Name,
                Description = lot.Description,
                Price = lot.Price,
                CategoryId = categoryId,
                MinimalBid = lot.MinimalBid,
                CloseTime = lot.CloseTime,
                IsClosed = false,
                UserId = lot.UserId,
            });
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
            return await unitOfWork.LotRepository.FindMany(conditon);
        }

        public async Task<Lot?> GetOne(int id) => await unitOfWork.LotRepository.GetByIdWithImages(id);

        public async Task<Lot?> GetDetails(int id)
        {
            return await unitOfWork.LotRepository.GetByIdWithImagesOrders(id);
        }

        public async Task<IReadOnlyCollection<Lot>> FilterLots(LotFilter filter)
        {
            List<Expression<Func<Lot, bool>>> conditions = new() { l => l.IsClosed == false };

            if (filter.Name != null)
                conditions.Add(l => l.Name.ToLower().Contains(filter.Name.ToLower()));

            if (filter.MinPrice != null)
                conditions.Add(l => l.Price >= filter.MinPrice);

            if (filter.MaxPrice != null)
                conditions.Add(l => l.Price <= filter.MaxPrice);

            // root = all lots
            if (filter.CategoryId == null) return await unitOfWork.LotRepository.FindManyWithOrdersImages(conditions);

            // if in category
            var currentCategory = await unitOfWork.CategoryRepository.FindOne(c => c.Id == filter.CategoryId);
            if (currentCategory == null) return await unitOfWork.LotRepository.FindManyWithOrdersImages(conditions);

            var subcategories = new List<int>() { currentCategory.Id };
            for (int i = 0; i < subcategories.Count; ++i)
            {
                var fullSubcategories = await unitOfWork.CategoryRepository.FindMany(c => c.ParentId == subcategories[i]);
                subcategories.AddRange(fullSubcategories.Select(c => c.Id));
            }

            conditions.Add(l => subcategories.Contains(l.CategoryId));

            return await unitOfWork.LotRepository.FindManyWithOrdersImages(conditions);
        }

        public async Task<IReadOnlyCollection<Lot>> UserSellLots(string uid)
        {
            return await unitOfWork.LotRepository.FindManyWithOrdersImages(new() { l => l.UserId == uid });
        }

        /// <summary>
        /// Fill all parents of given category
        /// </summary>
        /// <returns>Null if category isn't found, category if found</returns>
        public async Task<Category?> GetCategoryWithParents(string name)
        {
            var category = await unitOfWork.CategoryRepository.FindOne(c => c.Name.ToLower() == name.ToLower());
            if (category == null) return null;

            // get parents
            var current = category;
            while (current.ParentId != null)
            {
                var parent = await unitOfWork.CategoryRepository.FindOne(c => c.Id == current.ParentId);
                if (parent == null) break;
                current.Parent = parent;
                current = parent;
            }

            return category;
        }

        public async Task<IReadOnlyCollection<Category>> GetSubcategories(int? id)
        {
            return await unitOfWork.CategoryRepository.FindMany(c => c.ParentId == id);
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
                var userOrder = (await unitOfWork.OrderRepository.FindMany(x => (x.UserId == userId) && (x.LotId == lotId))).FirstOrDefault();
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