using Dal.UnitOfWork;
using Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;

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
          => await _unitOfWork.LotImageRepository.FindByConditionAsync(conditon);

        public async Task<IReadOnlyCollection<LotImage>> List() => await _unitOfWork.LotImageRepository.GetAllAsync();

        public async Task CreateAsync(LotImage productImage)
        {
            if (productImage != null)
            {
                await _unitOfWork.LotImageRepository.Create(productImage);
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

        public async Task AddToServer(Lot res, IFormFileCollection url)
        {
            string filePath = Path.Combine(_host.WebRootPath, "Images");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            foreach (var uploadedFile in url)
            {
                // путь к папке Files
                string path = "/Images/" + uploadedFile.FileName;

                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_host.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }

                LotImage img = new() { LotId = res.Id, Path = path };

                await CreateAsync(img);
            }
        }
    }
}