using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> CompleteAsync();
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;
    }
}
