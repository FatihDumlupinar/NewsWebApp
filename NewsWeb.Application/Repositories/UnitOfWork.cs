using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NewsWeb.Application.Interfaces;
using NewsWeb.Domain.Entities;
using NewsWeb.Domain.Entities.Identity;
using NewsWeb.Persistence.Contexts;

namespace NewsWeb.Application.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Ctor&Fields

        private readonly AppDbContext _appDbContext;
        private readonly IGenericRepository<Issue> _issues;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UnitOfWork(IGenericRepository<Issue> issues, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor, AppDbContext appDbContext, SignInManager<AppUser> signInManager)
        {
            _issues = issues;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _appDbContext = appDbContext;
            _signInManager = signInManager;
        }

        #endregion

        #region Properties
        
        public IGenericRepository<Issue> Issues => _issues;

        public UserManager<AppUser> UserManager => _userManager;

        public SignInManager<AppUser> SignInManager => _signInManager;

        public IHttpContextAccessor HttpContextAccessor => _httpContextAccessor; 

        #endregion

        /// <summary>
        /// Veritabanı için olan değişiklikleri veritabanına gönderir.
        /// </summary>
        /// <returns>Kaç verinin etkilendiğini geriye döndürür</returns>
        public Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            return _appDbContext.SaveChangesAsync(cancellationToken);
        }

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _appDbContext.Dispose();
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// Veritabanı için oluşturulmuş DbContext'leri Dispose eder. (Dispose = kullanılan kaynağı serbest bırakır.)
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
