using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KULESH.Domain.Entities;
using KULESH.UI.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KULESH.UI.Areas.Admin.Pages.FootballTeams
{
    public class IndexModel : PageModel
    {
        private readonly ITeamService _teamService;
        private readonly ICategoryService _categoryService;

        public IndexModel(ITeamService teamService, ICategoryService categoryService)
        {
            _teamService = teamService;
            _categoryService = categoryService;
        }

        public IList<FootballTeam> FootballTeams { get; set; } = new List<FootballTeam>();

        [FromQuery]
        public string? Category { get; set; }

        public async Task OnGetAsync()
        {
            // Load all categories for the dropdown
            var categoriesResponse = await _categoryService.GetCategoryListAsync();
            if (categoriesResponse.Success && categoriesResponse.Data != null)
            {
                ViewData["Categories"] = categoriesResponse.Data;
            }

            // Determine current category name for display
            if (!string.IsNullOrEmpty(Category))
            {
                var currentCat = categoriesResponse.Data?.FirstOrDefault(c => c.NormalizedName == Category);
                ViewData["CurrentCategory"] = currentCat?.Name ?? Category;
            }
            else
            {
                ViewData["CurrentCategory"] = "Все";
            }

            // Get filtered teams
            var teamResponse = await _teamService.GetTeamListAsync(Category);
            if (teamResponse.Success && teamResponse.Data != null)
            {
                FootballTeams = teamResponse.Data;
            }
        }
    }
}