using Bll.Models;
using Dal.UnitOfWork;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bll.Services
{
  public class LotService
  {
    private readonly IUnitOfWork unitOfWork;

    public LotService(IUnitOfWork unitOfWork)
    {
      this.unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyCollection<Lot>> FilterLots(LotFilter filter)
    {
      List<Expression<Func<Lot, bool>>> conditions = new();

      if (filter.Name != null)
        conditions.Add(l => l.Name.ToLower().Contains(filter.Name.ToLower()));

      if (filter.Categories.Count > 0)
        conditions.Add(l => filter.Categories.Contains(l.Category.Name));

      return await unitOfWork.LotRepository.FindByConditions(conditions);
    }

    public async Task<IReadOnlyCollection<Category>> GetCategories(List<string> current)
    {
      //if (current == null) return await unitOfWork.CategoryRepository.GetAllAsync();
      return await unitOfWork.CategoryRepository.OrderDescending(c => current.Contains(c.Name));
    }
  }
}