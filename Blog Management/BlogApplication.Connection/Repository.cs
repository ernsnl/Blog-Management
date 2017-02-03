using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BlogApplication.Entity;

namespace BlogApplication.Connection
{
    public class Repository<TEntity> : BlogApplication.Entity.IRepository<TEntity> where TEntity:class
    {
        protected DbContext context;
        private DbSet<TEntity> objectSet;

        public Repository(IUnitOfWork unitOfWork)
        {
            this.context = unitOfWork as DbContext;
            if (context != null)
                this.objectSet = context.Set<TEntity>();
        }
        public Repository() : this(new BlogApplicationEntities()) { }

        public TEntity Create()
        {
            return context.Set<TEntity>().Create();
        }

        public IQueryable<TEntity> GetQuery()
        {
            return objectSet;
        }

        public IQueryable<TElement> SqlQuery<TElement>(string sql, params object[] parameters) where TElement : class
        {
            return new ObjectQuery<TElement>(sql, ((IObjectContextAdapter)this).ObjectContext, MergeOption.NoTracking);
        }

        public IEnumerable<TEntity> GetAll(params string[] includes)
        {
            IQueryable<TEntity> objectSet = this.GetQuery();
            foreach (var included in includes)
                objectSet = objectSet.Include(included);
            return objectSet.AsEnumerable();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, params string[] includes)
        {
            IQueryable<TEntity> objectSet = this.GetQuery().Where<TEntity>(predicate);
            foreach (var included in includes)
                objectSet = objectSet.Include(included);
            return objectSet;
        }

        public TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            return objectSet.SingleOrDefault<TEntity>(predicate);
        }

        public TEntity First(Expression<Func<TEntity, bool>> predicate)
        {
            return objectSet.FirstOrDefault<TEntity>(predicate);
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return objectSet.Any(predicate);
        }

        public void Delete(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            objectSet.Remove(entity);
        }

        public void Add(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            objectSet.Add(entity);
        }

        public void Attach(TEntity entity)
        {
            objectSet.Attach(entity);
        }

        public void SaveChanges()
        {
            var changeSet = context.ChangeTracker.Entries();
            if (changeSet != null)
            {
                foreach (var entry in changeSet.Where(c => c.State != EntityState.Unchanged && c.State != EntityState.Deleted))
                {
                    var type = entry.Entity.GetType();
                    foreach (var item in entry.CurrentValues.PropertyNames)
                    {
                        var property = type.GetProperty(item);
                        if (property != null && entry.CurrentValues[item] == null && type.GetProperty(item).PropertyType.Name.IndexOf("string", StringComparison.InvariantCultureIgnoreCase) > -1)
                        {
                            entry.CurrentValues[item] = "";
                        }
                    }

                    if (entry.State == EntityState.Added && entry.CurrentValues.PropertyNames.Contains("CreatedDate"))
                        entry.CurrentValues["CreatedDate"] = DateTime.Now;

                    if (entry.CurrentValues.PropertyNames.Contains("UpdatedDate"))
                        entry.CurrentValues["UpdatedDate"] = DateTime.Now;
                }
            }

            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable SQLQuery(string sqlSelect)
        {
            return this.SQLQuery(sqlSelect, null);
        }

        public DataTable SQLQuery(string sqlSelect, params object[] parameters)
        {
            return this.SQLQuery(sqlSelect, 0, 0, parameters);
        }

        public DataTable SQLQuery(string sqlSelect, int page, int pageSize, params object[] parameters)
        {
            var param = new List<System.Data.SqlClient.SqlParameter>();
            for (int i = 0; i < parameters.Length; i++)
            {
                param.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@p" + i, Value = parameters[i] });
            }
            return this.SQLQuery(sqlSelect, page, pageSize, param.ToArray());
        }

        public DataTable SQLQuery(string sqlSelect, int page, int pageSize, System.Data.Common.DbParameter[] sqlParameters)
        {
            using (var cmd = this.context.Database.Connection.CreateCommand())
            {
                cmd.CommandText = sqlSelect;
                if (sqlParameters != null)
                {
                    foreach (var param in sqlParameters)
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = param.ParameterName, Value = param.Value, DbType = param.DbType });
                }

                var Adapter = System.Data.Common.DbProviderFactories.GetFactory(this.context.Database.Connection).CreateDataAdapter();
                Adapter.SelectCommand = cmd;
                DataSet DataSet = new System.Data.DataSet();

                this.CheckConnection();

                if (page > 0)
                    Adapter.Fill(DataSet, (page - 1) * pageSize, pageSize, "Table1");
                else
                    Adapter.Fill(DataSet, "Table1");
                return DataSet.Tables[0];
            }
        }

        public void Rollback()
        {
            foreach (var entry in this.context.ChangeTracker.Entries())
            {
                if (entry.State != EntityState.Unchanged)
                    entry.State = EntityState.Unchanged;
            }
        }

        private ObjectParameter CreateParameter(string name, string value)
        {
            return value != null ?
                new ObjectParameter(name, value) :
                new ObjectParameter(name, typeof(string));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (context != null)
                {
                    context.Dispose();
                    if (context.Database.Connection.State == ConnectionState.Open)
                        context.Database.Connection.Close();
                    context = null;
                }
            }
        }

        public IEnumerable<TEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }


        public object ExecuteNonQuery(string sql)
        {
            return this.ExecuteNonQuery(sql, null);
        }

        public object ExecuteNonQuery(string sql, params object[] parameters)
        {
            var param = new List<System.Data.SqlClient.SqlParameter>();
            for (int i = 0; i < parameters.Length; i++)
            {
                param.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@p" + i, Value = parameters[i] });
            }
            return ExecuteNonQuery(sql, param.ToArray());
        }

        public object ExecuteNonQuery(string sql, System.Data.Common.DbParameter[] sqlParameters)
        {
            using (var cmd = this.context.Database.Connection.CreateCommand())
            {
                cmd.CommandText = sql;
                if (sqlParameters != null)
                {
                    foreach (var param in sqlParameters)
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = param.ParameterName, Value = param.Value, DbType = param.DbType });
                }
                this.CheckConnection();
                return cmd.ExecuteNonQuery();
            }
        }

        public object ExecuteScalar(string sql)
        {
            return this.ExecuteScalar(sql, null);
        }

        public object ExecuteScalar(string sql, System.Data.Common.DbParameter[] sqlParameters)
        {
            using (var cmd = this.context.Database.Connection.CreateCommand())
            {
                cmd.CommandText = sql;
                if (sqlParameters != null)
                {
                    foreach (var param in sqlParameters)
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = param.ParameterName, Value = param.Value, DbType = param.DbType });
                }
                this.CheckConnection();
                return cmd.ExecuteScalar();
            }
        }


        public object ExecuteScalar(string sql, params object[] parameters)
        {
            var param = new List<System.Data.SqlClient.SqlParameter>();
            for (int i = 0; i < parameters.Length; i++)
            {
                param.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@p" + i, Value = parameters[i] });
            }
            return ExecuteScalar(sql, param.ToArray());
        }

        private void CheckConnection()
        {
            if (this.context.Database.Connection.State == ConnectionState.Closed)
                this.context.Database.Connection.Open();
        }
    }

}

