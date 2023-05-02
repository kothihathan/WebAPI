using Autofac.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Win32.SafeHandles;
using System;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using WebApi.Helpers;
using WebApi.Interface;
using WebApi.Interfaces;

namespace WebApi.Services
{
    
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
    {
        private readonly DataContext _dbContext;
        private IDbContextTransaction _transaction;
        // Flag: Has Dispose already been called?
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        public UnitOfWork(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity
        {
            return new GenericRepository<TEntity, TContext>(_dbContext, this);
        }
        public IDbContextTransaction BeginTransaction()
        {
            _transaction = _dbContext.Database.BeginTransaction();
            return _transaction;
        }
        public void Commit()
        {
            _dbContext.SaveChanges();
        }
        public void Rollback()
        {
            _dbContext.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
        }             

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                // Free any other managed objects here.
                //
                _dbContext.Dispose();
            }

            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            GC.SuppressFinalize(this);
        }
    }
}
