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
    public class CreateModel : PageModel
    {
        private readonly ITeamService _teamService;
        private readonly ICategoryService _categoryService;

        public CreateModel(ITeamService teamService, ICategoryService categoryService)
        {
            _teamService = teamService;
            _categoryService = categoryService;
        }

        [BindProperty]
        public FootballTeam FootballTeam { get; set; } = default!;

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public SelectList Categories { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadCategoriesAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync();
                return Page();
            }

            // Handle image upload
            if (ImageFile != null)
            {
                // Generate unique filename and save to wwwroot/images
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                Directory.CreateDirectory(uploadsFolder); // Ensure folder exists
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }
                // Set the image path in the entity (relative to wwwroot)
                FootballTeam.Image = "/images/" + uniqueFileName;
            }

            var result = await _teamService.CreateTeamAsync(FootballTeam, null); // We handle file separately, but service may expect IFormFile; adjust if needed
            // If your service's CreateTeamAsync expects IFormFile, pass ImageFile instead of null.
            // For simplicity, we set FootballTeam.Image above and pass null to the service.

            if (result.Success)
            {
                return RedirectToPage("./Index");
            }

            ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Error creating team.");
            await LoadCategoriesAsync();
            return Page();
        }

        private async Task LoadCategoriesAsync()
        {
            var categoriesResponse = await _categoryService.GetCategoryListAsync();
            if (categoriesResponse.Success && categoriesResponse.Data != null)
            {
                Categories = new SelectList(categoriesResponse.Data, nameof(Category.Id), nameof(Category.Name));
            }
            else
            {
                Categories = new SelectList(Enumerable.Empty<SelectListItem>());
            }
        }
    }
}