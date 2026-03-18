using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using KULESH.UI.Data;

namespace KULESH.UI.Controllers
{
    [Route("[controller]/[action]")]
    public class ImageController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public ImageController(UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetAvatar()
        {
            byte[] content = null;
            string contentType = "image/png";

            if (User?.Identity?.IsAuthenticated == true)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user?.Avatar != null && user.Avatar.Length > 0)
                {
                    content = user.Avatar;
                    contentType = "image/png";
                }
            }

            if (content == null)
            {
                var defaultPathSvg = Path.Combine(_env.WebRootPath, "Images", "default-avatar.svg");
                var defaultPathPng = Path.Combine(_env.WebRootPath, "Images", "default-avatar.png");

                if (System.IO.File.Exists(defaultPathSvg))
                {
                    content = await System.IO.File.ReadAllBytesAsync(defaultPathSvg);
                    contentType = "image/svg+xml";
                }
                else if (System.IO.File.Exists(defaultPathPng))
                {
                    content = await System.IO.File.ReadAllBytesAsync(defaultPathPng);
                    contentType = "image/png";
                }
                else
                {
                    // fallback: 1x1 transparent png
                    content = new byte[] { 137,80,78,71,13,10,26,10,0,0,0,13,73,72,68,82,0,0,0,1,0,0,0,1,8,6,0,0,0,31,21,196,137,0,0,0,12,73,68,65,84,8,153,99,0,1,0,0,5,0,1,13,10,44,169,0,0,0,0,73,69,78,68,174,66,96,130 };
                    contentType = "image/png";
                }
            }

            return File(content, contentType);
        }
    }
}
