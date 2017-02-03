using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApplication.Entity
{
    public interface IRepositoryFactory
    {
        IRepository<TEntity> GetRepositoryFromEntity<TEntity>() where TEntity :class;

        TRepository GetRepository<TRepository>();
    }
}
