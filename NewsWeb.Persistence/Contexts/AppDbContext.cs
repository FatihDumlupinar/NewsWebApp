using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NewsWeb.Domain.Common;
using NewsWeb.Domain.Entities;
using NewsWeb.Domain.Entities.Identity;
using System.Security.Claims;

namespace NewsWeb.Persistence.Contexts
{
    public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AppDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            Guid userId = default;
            var userIdClaim = _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!string.IsNullOrEmpty(userIdClaim))
            {
                _ = Guid.TryParse(userIdClaim, out userId);
            }

            ChangeTracker.DetectChanges();
            var added = ChangeTracker.Entries()
            .Where(t => t.State == EntityState.Added)
            .Select(t => t.Entity)
            .ToArray();

            foreach (var entity in added)
            {
                if (entity is BaseEntity track)
                {
                    track.CreateDate = DateTime.Now;
                    track.IsActive = true;
                    track.CreateUserId = userId;
                }
            }

            var modified = ChangeTracker.Entries()
            .Where(t => t.State == EntityState.Modified)
            .Select(t => t.Entity)
            .ToArray();

            foreach (var entity in modified)
            {
                if (entity is BaseEntity track)
                {
                    track.UpdateDate = DateTime.Now;
                    track.UpdateUserId = userId;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        public DbSet<Issue> Issues { get; set; }
    }
}
