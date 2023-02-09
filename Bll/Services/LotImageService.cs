using Dal.UnitOfWork;
using Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Bll.Services
{
    public class LotImageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string root;

        public LotImageService(IUnitOfWork unitOfWork, IWebHostEnvironment appEnv)
        {
            _unitOfWork = unitOfWork;
            root = appEnv.WebRootPath;
        }

        public async Task DeleteAll(int id)
        {
            var images = await _unitOfWork.LotImageRepository.FindByConditionAsync(x => x.LotId == id);
            foreach (var image in images)
            {
                string fullPath = root + image.Path;
                if (File.Exists(fullPath))
                {
                    try
                    {
                        File.Delete(fullPath);
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }
            }
        }

        public async Task Add(Lot res, IFormFileCollection url)
        {
            string filePath = Path.Combine(root, "images");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            foreach (var uploadedFile in url)
            {
                string path = "/images/" + uploadedFile.FileName;

                using (FileStream fileStream = new(root + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }

                LotImage img = new() { LotId = res.Id, Path = path };
                await _unitOfWork.LotImageRepository.Create(img);
            }
        }
    }
}