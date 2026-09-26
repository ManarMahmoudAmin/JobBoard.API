using JobBoard.Application.Interfaces;
using JobBoard.Infrastructure.Persistence;
using JobBoard.Infrastructure.Infrastructure;
using System.Collections;

namespace JobBoard.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly Hashtable _Infrastructure;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            _Infrastructure = new Hashtable();
        }
        public async Task<int> CompleteAsync()
            => await _context.SaveChangesAsync();


        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            var repoType = typeof(TEntity).Name;

            //Check if the repository already exists in the hashtable
            if (!_Infrastructure.ContainsKey(repoType))
            {
                var repo = new GenericRepository<TEntity>(_context);
                _Infrastructure.Add(repoType, repo);
            }
            return _Infrastructure[repoType] as IGenericRepository<TEntity>;
        }
    }
}