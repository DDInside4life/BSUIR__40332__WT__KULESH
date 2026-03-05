using Microsoft.AspNetCore.Mvc;
using KULESH.Domain.Entities;
using KULESH.Domain.Models;
using KULESH.UI.Services;

namespace KULESH.UI.Controllers
{
    public class FootballTeamController(
        ICategoryService categoryService, 
        ITeamService teamService) : Controller
    {
        public async Task<IActionResult> Index(string? category)
        {
            var teamResponse = await teamService.GetTeamListAsync(category);
            if (!teamResponse.Success)
            {
                return NotFound(teamResponse.ErrorMessage);
            }

            // Get all categories for the filter dropdown
            var categoriesResponse = await categoryService.GetCategoryListAsync();
            ViewBag.Categories = categoriesResponse.Data;   // List<Category>

            // Determine the current category name to display
            if (string.IsNullOrEmpty(category))
                ViewData["currentCategory"] = "Все";
            else
            {
                var currentCat = categoriesResponse.Data?.FirstOrDefault(c => c.NormalizedName == category);
                ViewData["currentCategory"] = currentCat?.Name ?? category;
            }

            return View(teamResponse.Data);
        }
    }
}
