using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using WebApi.Interface;
using WebApi.Services;

namespace WebApi.Interfaces
{
    public interface IUnitOfWork<TContext> : IDisposable where TContext : DbContext
    {
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity;
        IDbContextTransaction BeginTransaction();
        void Commit();
        void Rollback();
    }

}
