using Dal.UnitOfWork;
using Domain.Models;

namespace Bll.Services
{
    public class CategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyCollection<Category>> List() => await _unitOfWork.CategoryRepository.GetAllAsync();

        public async Task<bool> Create(Category category)
        {
            try
            {
                category.Name = category.Name.Trim();
                await _unitOfWork.CategoryRepository.Create(category);
                return true;
            }
            catch (Exception) { return false; }
        }

        /// <returns>Is operation success</returns>
        public async Task<bool> Edit(int id, string name)
        {
            try
            {
                name = name.Trim();
                var sameName = await _unitOfWork.CategoryRepository.FindByConditionAsync(c => c.Name.ToLower() == name.ToLower());
                if (sameName != null && sameName.Count != 0) return false;
                await _unitOfWork.CategoryRepository.Update(id, name);
                return true;
            }
            catch (Exception) { return false; }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                await _unitOfWork.CategoryRepository.DeleteOne(id);
                return true;
            }
            catch (Exception) { return false; }
        }

        /*
        public async Task<Category?> PropertiesFor(int id)
        {
            var category = (await _unitOfWork.CategoryRepository.FindByConditionWithPropertiesAsync(c => c.Id == id)).FirstOrDefault();
            if (category == null) return null;
            return category;
        }
        */
    }
}