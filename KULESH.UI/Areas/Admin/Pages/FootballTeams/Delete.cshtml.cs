using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KULESH.Domain.Entities;
using KULESH.UI.Services;
using System.Threading.Tasks;

namespace KULESH.UI.Areas.Admin.Pages.FootballTeams
{
    public class DeleteModel : PageModel
    {
        private readonly ITeamService _teamService;

        public DeleteModel(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int id)
        {
            // Optional: delete image file
            var response = await _teamService.GetTeamByIdAsync(id);
            if (response.Success && response.Data != null && !string.IsNullOrEmpty(response.Data.Image))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", response.Data.Image.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            await _teamService.DeleteTeamAsync(id);
            return RedirectToPage("./Index");
        }
    }
}