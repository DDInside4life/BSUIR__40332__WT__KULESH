using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using KULESH.Domain.Entities;
using KULESH.UI.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KULESH.UI.Areas.Admin.Pages.FootballTeams
{
    public class EditModel : PageModel
    {
        private readonly ITeamService _teamService;
        private readonly ICategoryService _categoryService;

        public EditModel(ITeamService teamService, ICategoryService categoryService)
        {
            _teamService = teamService;
            _categoryService = categoryService;
        }

        [BindProperty]
        public FootballTeam FootballTeam { get; set; } = default!;

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public SelectList Categories { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var response = await _teamService.GetTeamByIdAsync(id);
            if (!response.Success || response.Data == null)
            {
                return NotFound();
            }

            FootballTeam = response.Data;
            await LoadCategoriesAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (id != FootballTeam.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync();
                return Page();
            }

            // Handle image upload (new image replaces old one)
            if (ImageFile != null)
            {
                // Delete old image file if exists and not default
                if (!string.IsNullOrEmpty(FootballTeam.Image))
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", FootballTeam.Image.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                // Save new image
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                Directory.CreateDirectory(uploadsFolder);
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }
                FootballTeam.Image = "/images/" + uniqueFileName;
            }

            await _teamService.UpdateTeamAsync(id, FootballTeam, null); // Pass null if service expects IFormFile
            return RedirectToPage("./Index");
        }

        private async Task LoadCategoriesAsync()
        {
            var categoriesResponse = await _categoryService.GetCategoryListAsync();
            if (categoriesResponse.Success && categoriesResponse.Data != null)
            {
                Categories = new SelectList(categoriesResponse.Data, nameof(Category.Id), nameof(Category.Name), FootballTeam?.CategoryId);
            }
            else
            {
                Categories = new SelectList(Enumerable.Empty<SelectListItem>());
            }
        }
    }
}