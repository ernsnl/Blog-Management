using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace BlogApplication.Entity
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        IQueryable<TEntity> GetQuery();
        IEnumerable<TEntity> GetAll(params string[] includes);
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, params string[] includes);
        TEntity Single(Expression<Func<TEntity, bool>> predicate);
        TEntity First(Expression<Func<TEntity, bool>> predicate);
        bool Any(Expression<Func<TEntity, bool>> predicate);
        TEntity Create();
        void Add(TEntity entity);
        void Delete(TEntity entity);
        void Attach(TEntity entity);
        void SaveChanges();
        void Rollback();
        IQueryable<TElement> SqlQuery<TElement>(string sql, params object[] parameters) where TElement : class;
        DataTable SQLQuery(string sqlSelect);
        DataTable SQLQuery(string sqlSelect, params object[] parameters);
        DataTable SQLQuery(string sqlSelect, int page, int pageSize, params object[] parameters);
        DataTable SQLQuery(string sqlSelect, int page, int pageSize, System.Data.Common.DbParameter[] parameters);
        object ExecuteNonQuery(string sql);
        object ExecuteNonQuery(string sql, System.Data.Common.DbParameter[] sqlParameters);
        object ExecuteNonQuery(string sql, params object[] parameters);
        object ExecuteScalar(string sql);
        object ExecuteScalar(string sql, System.Data.Common.DbParameter[] sqlParameters);
        object ExecuteScalar(string sql, params object[] parameters);
    }
}