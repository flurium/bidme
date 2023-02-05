using System.Linq.Expressions;
using Microsoft.AspNetCore.Hosting;
using Dal.Models;
using Dal.Repository;
using Domain.Models;
using Dal.UnitOfWork;

namespace Bll.Services
{
    public class LotImageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _host;


        public LotImageService(IUnitOfWork unitOfWork, IWebHostEnvironment appEnv)
        {
            _unitOfWork = unitOfWork;
            _host = appEnv;
        }

        public async Task<IReadOnlyCollection<LotImage>> FindByConditionAsync(Expression<Func<LotImage, bool>> conditon)
          =>  await _unitOfWork.LotImageRepository.FindByConditionAsync(conditon);

        public async Task<IReadOnlyCollection<LotImage>> List() => await _unitOfWork.LotImageRepository.GetAllAsync();

        public async Task CreateAsync(LotImage productImage)
        {
            if (productImage != null)
            {
                await _unitOfWork.LotImageRepository.CreateAsync(productImage);
            }
        }
        /*
        public async Task Delete(int id)
        {
            await _unitOfWork.LotImageRepository.Delete(id);
        }
        */

        public async Task DeleteFromServer(int ProductId)
        {
            var images = await _unitOfWork.LotImageRepository.FindByConditionAsync(x => x.LotId == ProductId);
            foreach (var image in images)
            {
                string fullPath = _host.WebRootPath + image.Path;
                if (System.IO.File.Exists(fullPath))
                {
                    try
                    {
                        System.IO.File.Delete(fullPath);

                    }
                    catch (Exception e)
                    {

                    }
                }

            }
        }
    }
}