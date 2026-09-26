using System.Linq.Expressions;

namespace JobBoard.Application.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(int id);

        #region With Specifications
        Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TEntity> specifications);
        Task<TEntity> GetByIdAsync(ISpecifications<TEntity> specifications);
        #endregion

        public Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
        Task<bool> ExistsAsync(ISpecifications<TEntity> specifications);

        Task<int> CountAsync(ISpecifications<TEntity> specifications = null);


    }
}
