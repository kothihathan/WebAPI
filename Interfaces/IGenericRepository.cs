using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System;
using WebApi.Interface;
using Microsoft.EntityFrameworkCore.Storage;

namespace WebApi.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class, IEntity
    {        
        IQueryable<TEntity> AsQueryable();
        IEnumerable<TEntity> GetAll();
        TEntity Find(params object[] keyValues);
        IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression);
        IEnumerable<TEntity> FromSqlRaw(string sqlQuery, params object[] keyValues);
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entity);
        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entity);
        void Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entity);
    }
}
