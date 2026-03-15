using Microsoft.AspNetCore.Identity;

namespace KULESH.UI.Data
{
    public class ApplicationUser : IdentityUser
    {
        public byte[]? Avatar { get; set; }
    }
}
