using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WebApi.Helpers;
using WebApi.Interface;
using WebApi.Interfaces;

namespace WebApi.Services
{
    public class GenericRepository<TEntity, TContext> : IGenericRepository<TEntity> where TEntity : class, IEntity where TContext : DbContext
    {
        private readonly DataContext _context;
        private IDbContextTransaction _transaction;
        private readonly IUnitOfWork<TContext> _unitOfWork;

        public GenericRepository(DataContext context, IUnitOfWork<TContext> unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        #region IGenericRepository<T> implementation     

        public IQueryable<TEntity> AsQueryable()
        {
            return _context.Set<TEntity>().AsQueryable();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().AsNoTracking().AsEnumerable();
        }

        public TEntity Find(params object[] keyValues)
        {
            return _context.Set<TEntity>().Find(keyValues);
        }
        public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression)
        {
            return _context.Set<TEntity>().Where(expression).AsNoTracking();
        }
        public IEnumerable<TEntity> FromSqlRaw(string sqlQuery, params object[] keyValues)
        {
            return _context.Set<TEntity>().FromSqlRaw(sqlQuery, keyValues).AsNoTracking();
        }
        public void Add(TEntity entity)
        {
            try
            {
                _context.Set<TEntity>().Add(entity);
            }
            //XL add to catch Database update Exception
            catch (DbUpdateException ex)
            {
                throw new AppException(ex.InnerException.Message);
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                throw new AppException(ex.Message);
            }
        }
        public void AddRange(IEnumerable<TEntity> entity)
        {
            try
            {
                _context.Set<TEntity>().AddRange(entity);
            }
            catch (DbUpdateException ex)
            {
                throw new AppException(ex.InnerException.Message);
            }
            catch (AppException ex)
            {
                throw new AppException(ex.Message);
            }
        }
        public void Update(TEntity entity)
        {
            try
            {
                _context.Set<TEntity>().Update(entity);
            }
            catch (DbUpdateException ex)
            {
                throw new AppException(ex.InnerException.Message);
            }
            catch (AppException ex)
            {
                throw new AppException(ex.Message);
            }
        }
        public void UpdateRange(IEnumerable<TEntity> entity)
        {
            try
            {
                _context.Set<TEntity>().UpdateRange(entity);
            }
            catch (DbUpdateException ex)
            {
                throw new AppException(ex.InnerException.Message);
            }
            catch (AppException ex)
            {
                throw new AppException(ex.Message);
            }
        }
        public void Delete(TEntity entity)
        {
            try
            {
                _context.Set<TEntity>().Remove(entity);
            }
            catch (DbUpdateException ex)
            {
                throw new AppException(ex.InnerException.Message);
            }
            catch (AppException ex)
            {
                throw new AppException(ex.Message);
            }
        }
        public void DeleteRange(IEnumerable<TEntity> entity)
        {
            try
            {
                _context.Set<TEntity>().RemoveRange(entity);
            }
            catch (DbUpdateException ex)
            {
                throw new AppException(ex.InnerException.Message);
            }
            catch (AppException ex)
            {
                throw new AppException(ex.Message);
            }
        }
        public void DbTracking(TEntity entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Added;
            }
            catch (DbUpdateException ex)
            {
                throw new AppException(ex.InnerException.Message);
            }
            catch (AppException ex)
            {
                throw new AppException(ex.Message);
            }
        }

        #endregion
    }
}
