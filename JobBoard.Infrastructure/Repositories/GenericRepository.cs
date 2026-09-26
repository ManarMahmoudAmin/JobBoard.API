using JobBoard.Application.Interfaces;
using JobBoard.Infrastructure.Persistence;
using JobBoard.Application.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace JobBoard.Infrastructure.Infrastructure
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync()
            => await _context.Set<TEntity>().ToListAsync();


        public async Task<TEntity> GetByIdAsync(int id)
            => await _context.Set<TEntity>().FindAsync(id);


        #region With Specifications

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TEntity> specifications)
            => await SpecificationEvaluator.CreateQuery(_context.Set<TEntity>(), specifications).ToListAsync();

        public async Task<TEntity> GetByIdAsync(ISpecifications<TEntity> specifications)
            => await SpecificationEvaluator.CreateQuery(_context.Set<TEntity>(), specifications).SingleOrDefaultAsync();

        #endregion

        public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().Where(predicate).ToListAsync();
        }
        public async Task AddAsync(TEntity entity)
        {
            await _context.AddAsync(entity);
        }
        public void Update(TEntity entity)
        {
            _context.Update(entity);
        }
        public void Delete(TEntity entity)
        {
            _context.Remove(entity);
        }
        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().AnyAsync(predicate);
        }
        public async Task<bool> ExistsAsync(ISpecifications<TEntity> specifications)
        {
            return await SpecificationEvaluator.CreateQuery(_context.Set<TEntity>(), specifications).AnyAsync();
        }

        public async Task<int> CountAsync(ISpecifications<TEntity> specifications = null)
        {
            var query = SpecificationEvaluator.CreateQuery(_context.Set<TEntity>(), specifications, isForCount: true);
            return await query.CountAsync();
        }
    }
}
