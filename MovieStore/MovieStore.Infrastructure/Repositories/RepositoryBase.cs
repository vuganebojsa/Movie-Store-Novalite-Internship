using Microsoft.EntityFrameworkCore;
using MovieStore.Api.DTOs;
using MovieStore.Core.Model;
using MovieStore.Infrastructure.Interfaces;
using System.Linq.Expressions;

namespace MovieStore.Infrastructure.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected MovieStoreContext _movieStoreContext { get; set; }
        public RepositoryBase(MovieStoreContext movieStoreContext) => _movieStoreContext = movieStoreContext;
        public async Task Create(T entity) => await _movieStoreContext.Set<T>().AddAsync(entity);
        public void Delete(T entity) => _movieStoreContext.Set<T>().Remove(entity);
        public async Task<ICollection<T>> FindAll() => await _movieStoreContext.Set<T>().ToListAsync();
        public async Task<PagedList<T>> FindAll(PageDTO page)
        {
            return PagedList<T>.ToPagedList(await _movieStoreContext.Set<T>().ToListAsync(),
            page.PageNumber,
            page.PageSize);
        }
        public async Task<ICollection<T>> FindByCondition(Expression<Func<T, bool>> expression) => await _movieStoreContext.Set<T>().Where(expression).ToListAsync();
        public async Task<T?> FindById(Guid id) => await _movieStoreContext.Set<T>().FindAsync(id);
        public async Task SaveChanges() => await _movieStoreContext.SaveChangesAsync();
    }
}
