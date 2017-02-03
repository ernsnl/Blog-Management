using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BlogApplication.Entity;

namespace BlogApplication.Connection
{
    public class RepositoryFactory : IRepositoryFactory
    {
        [ThreadStatic]
        private static RepositoryFactory _Current;

        private Hashtable UnitOfWorks = new Hashtable();

        public IRepository<TEntity> GetRepositoryFromEntity<TEntity>() where TEntity : class
        {
            return this.GetInstance<IRepository<TEntity>>();
        }
        public TRepository GetRepository<TRepository>()
        {
            return this.GetInstance<TRepository>();
        }
        public TInstance GetInstance<TInstance>()
        {
            return (TInstance)this.GetInstance(typeof(TInstance));
        }
        public object GetInstance(Type type)
        {
            type = GetRealType(type);
            if (type != null)
            {
                if (type.GetInterfaces().Contains(typeof(IUnitOfWork)))
                {
                    if (UnitOfWorks[type.GetType().Name] == null)
                    {
                        UnitOfWorks[type.GetType().Name] = Activator.CreateInstance(type);
                    }
                    return UnitOfWorks[type.GetType().Name];
                }

                var constructor = type.GetConstructors().Single();
                var parameters = constructor.GetParameters();

                if (!parameters.Any()) return Activator.CreateInstance(type);
                var args = new List<object>();
                foreach (var parameter in parameters)
                {
                    var arg = GetInstance(parameter.ParameterType);
                    args.Add(arg);
                }
                var result = Activator.CreateInstance(type, args.ToArray());
                return result;
            }
            return null;

        }
        protected ConstructorInfo GetDefaultConstructor<TInstance>()
        {
            ConstructorInfo defaultConstructor = typeof(TInstance).GetConstructor(Type.EmptyTypes);
            return defaultConstructor;
        }
        protected Type GetRealType(Type type)
        {

            var Item = typeof(RepositoryFactory).Assembly.GetExportedTypes()
                .Where(type.IsAssignableFrom)
                .Where(t => !t.IsAbstract && !t.IsInterface);

            return Item.FirstOrDefault();
        }

        public static IRepositoryFactory Current
        {
            get
            {
                _Current = _Current ?? new RepositoryFactory();
                return _Current;
            }
        }

        public void Clear()
        {
            this.UnitOfWorks.Clear();
        }

        public static void Dispose()
        {
            if (_Current != null)
            {
                _Current.Clear();
            }
            _Current = null;
        }
    }
}