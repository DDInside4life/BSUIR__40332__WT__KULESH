using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace KULESH.UI.Services
{
    public class NoOpEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Intentionally do nothing - simulate successful email sending for testing
            return Task.CompletedTask;
        }
    }
}
