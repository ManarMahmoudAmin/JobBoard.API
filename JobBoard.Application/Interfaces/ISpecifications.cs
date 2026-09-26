using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace JobBoard.Application.Interfaces
{
    public interface ISpecifications<TEntity> where TEntity : class
    {
        public Expression<Func<TEntity, bool>>? Criteria { get; }
        public List<Expression<Func<TEntity, object>>> Includes { get; }
        public Expression<Func<TEntity, object>> Order { get; }
        public Expression<Func<TEntity, object>> OrderDesc { get; }
        public List<Expression<Func<TEntity, object>>> ThenBy { get; }
        public List<Expression<Func<TEntity, object>>> ThenByDesc { get; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginationEnabled { get; set; }
    }
}
