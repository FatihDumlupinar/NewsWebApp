using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NewsWeb.Domain.Entities;
using NewsWeb.Domain.Entities.Identity;

namespace NewsWeb.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IGenericRepository<Issue> Issues { get; }
        public UserManager<AppUser> UserManager { get; }
        public SignInManager<AppUser> SignInManager { get; }
        public IHttpContextAccessor HttpContextAccessor { get; }
        Task<int> CommitAsync(CancellationToken cancellationToken = default);
    }
}
