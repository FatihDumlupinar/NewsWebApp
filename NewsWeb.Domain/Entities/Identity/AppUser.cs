
using Microsoft.AspNetCore.Identity;

namespace NewsWeb.Domain.Entities.Identity
{
    public class AppUser : IdentityUser<Guid>
    {
        public string? FullName { get; set; }

    }
}
