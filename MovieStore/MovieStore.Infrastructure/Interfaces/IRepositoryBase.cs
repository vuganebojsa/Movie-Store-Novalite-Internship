using MovieStore.Api.DTOs;
using MovieStore.Core.Model;
using System.Linq.Expressions;

namespace MovieStore.Infrastructure.Interfaces
{
    public interface IRepositoryBase<T>
    {
        Task<ICollection<T>> FindAll();
        Task<PagedList<T>> FindAll(PageDTO page);
        Task<T?> FindById(Guid id);
        Task<ICollection<T>> FindByCondition(Expression<Func<T, bool>> expression);
        Task Create(T entity);
        void Delete(T entity);
        Task SaveChanges();
    }
}
