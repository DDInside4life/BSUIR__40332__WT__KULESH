using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KULESH.Domain.Entities;
using KULESH.UI.Services;
using System.Threading.Tasks;

namespace KULESH.UI.Areas.Admin.Pages.FootballTeams
{
    public class DetailsModel : PageModel
    {
        private readonly ITeamService _teamService;

        public DetailsModel(ITeamService teamService)
        {
            _teamService = teamService;
        }

        public FootballTeam FootballTeam { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var response = await _teamService.GetTeamByIdAsync(id);
            if (!response.Success || response.Data == null)
            {
                return NotFound();
            }

            FootballTeam = response.Data;
            return Page();
        }
    }
}