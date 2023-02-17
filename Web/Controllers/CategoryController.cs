using Bll.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize(Roles = Role.Admin)]
    public class CategoryController : Controller
    {
        public static string Name => "category";

        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index() => View(await _categoryService.List());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string Name)
        {
            var category = new Category() { Name = Name };
            await _categoryService.Create(category);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string Name)
        {
            var res = await _categoryService.Edit(id, Name);
            return res ? RedirectToAction(nameof(Index)) : View("Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _categoryService.Delete(id);
            return res ? RedirectToAction(nameof(Index)) : View("Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSubCategory(int ParentId, string Name)
        {
            var category = new Category() { Name = Name , ParentId =ParentId };
            await _categoryService.Create(category);
            return RedirectToAction(nameof(Index));
        }
    }
}
