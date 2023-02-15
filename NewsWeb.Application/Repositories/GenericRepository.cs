using Microsoft.EntityFrameworkCore;
using NewsWeb.Application.Interfaces;
using NewsWeb.Domain.Common;
using NewsWeb.Infrastructure.Services.Chache;
using NewsWeb.Persistence.Contexts;
using System.Linq.Expressions;

namespace NewsWeb.Application.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity, new()
    {
        #region Ctor&Fields

        protected readonly AppDbContext _appDbContext;
        private readonly ICacheService _cacheService;
        private readonly string cacheKey = $"{typeof(T)}";

        public GenericRepository(AppDbContext appDbContext, ICacheService cacheService)
        {
            _appDbContext = appDbContext;
            _cacheService = cacheService;
        }

        #endregion

        public virtual async ValueTask AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            _ = await _appDbContext.Set<T>().AddAsync(entity, cancellationToken);
        }

        public virtual async ValueTask<T> AddAsyncReturnEntity(T entity, CancellationToken cancellationToken = default)
        {
            var data = await _appDbContext.Set<T>().AddAsync(entity, cancellationToken);
            return data.Entity;
        }

        public virtual Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            return _appDbContext.Set<T>().AddRangeAsync(entities, cancellationToken);
        }

        public virtual Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
            {
                entity.IsActive = false;
                _ = _appDbContext.Set<T>().Update(entity);
            }, cancellationToken);
        }

        public virtual Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
            {
                foreach (var entity in entities)
                {
                    entity.IsActive = false;
                }

                _appDbContext.Set<T>().UpdateRange(entities);

            }, cancellationToken);
        }

        public virtual async Task<List<T>> FindAsync(Expression<Func<T, bool>>? expression = default, CancellationToken cancellationToken = default)
        {
            var cachedList = await _cacheService.GetAsync<List<T>>(cacheKey, cancellationToken);
            if (cachedList == null)
            {
                cachedList = expression == null ?
                    await _appDbContext.Set<T>().ToListAsync(cancellationToken) :
                    await _appDbContext.Set<T>().Where(expression).ToListAsync(cancellationToken);

                await _cacheService.SetAsync(cacheKey, cachedList, cancellationToken);
            }
            else
            {
                if (expression != null)
                {
                    var iQueryable = cachedList.AsQueryable();
                    iQueryable = iQueryable.Where(expression);
                    cachedList = iQueryable.ToList();
                }
            }

            return cachedList;
        }

        public virtual Task<T?> FindOneAsync(Expression<Func<T?, bool>> expression, CancellationToken cancellationToken = default)
        {
            return _appDbContext.Set<T>().FirstOrDefaultAsync(expression, cancellationToken);
        }

        public virtual Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _appDbContext.Set<T>().FirstOrDefaultAsync(i => i.IsActive && i.Id == id, cancellationToken);
        }

        public async Task RefreshCacheAsync(CancellationToken cancellationToken = default)
        {
            await _cacheService.RemoveAsync(cacheKey, cancellationToken);
            var cachedList = await _appDbContext.Set<T>().ToListAsync(cancellationToken);
            await _cacheService.SetAsync(cacheKey, cachedList, cancellationToken);
        }

        public virtual Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
            {
                _ = _appDbContext.Set<T>().Update(entity);
            }, cancellationToken);
        }

        public virtual Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
            {
                _appDbContext.Set<T>().UpdateRange(entities);
            }, cancellationToken);
        }
    }
}
