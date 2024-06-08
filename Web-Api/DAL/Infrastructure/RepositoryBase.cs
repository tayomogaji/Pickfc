using Pickfc.DAL.Interfaces.IDB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Pickfc.DAL.Infrastructure
{
    public class RepositoryBase<TEntityType, TRepositioyType> where TEntityType: class where TRepositioyType : DbContext
    {
        #region properties
        private TRepositioyType _dataContext;
        private readonly DbSet<TEntityType> _dbSet;

        private IDBFactory<TRepositioyType> DbFactory 
        {
            get;
            set;
        }
        #endregion

        protected TRepositioyType DbContext => _dataContext ?? (_dataContext = DbFactory.Init());

        protected RepositoryBase(IDBFactory<TRepositioyType> dbFactory) 
        {
            DbFactory = dbFactory;
            _dbSet = DbContext.Set<TEntityType>();
        }

        #region implementation
        public virtual void Add(TEntityType entity)
        {
            _dbSet.Add(entity);
        }


        public virtual void Update(TEntityType entity) 
        {
            _dbSet.Update(entity);
            _dataContext.Entry(entity).State= EntityState.Modified;
        }

        public virtual void Delete(TEntityType entity)
        {
            _dbSet.Remove(entity);
        }

        public TEntityType SingleOrDefault(Expression<Func<TEntityType, bool>> predicate, Func<IQueryable<TEntityType>,
            IIncludableQueryable<TEntityType, object>> include = null)
        {
            IQueryable<TEntityType> query = _dbSet;
            if (include != null)
                query = include(query);

            return query.SingleOrDefault(predicate);
        }

        public TEntityType Sord(Expression<Func<TEntityType, bool>> predicate, Func<IQueryable<TEntityType>,
        IIncludableQueryable<TEntityType, object>> include = null)
        {
            IQueryable<TEntityType> query = _dbSet;
            if (include != null)
                query = include(query);

            return query.SingleOrDefault(predicate);
        }

        public TEntityType FirstOrDefault(Expression<Func<TEntityType, bool>> predicate, Func<IQueryable<TEntityType>,
        IIncludableQueryable<TEntityType, object>> include = null, bool disableTracking = true)
        {
            IQueryable<TEntityType> query = _dbSet;
            if (disableTracking)
                query = query.AsNoTracking();
            if (include != null)
                query = include(query);
            return query.FirstOrDefault(predicate);
        }

        public TEntityType SorD(IEnumerable<TEntityType> entity, Func<TEntityType, bool> predicate)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return entity.SingleOrDefault(predicate);
        }

        public virtual IEnumerable<TEntityType> GetMany(Expression<Func<TEntityType, bool>> predicate, Func<IQueryable<TEntityType>, IIncludableQueryable<TEntityType, object>> include = null, bool disableTracking = true) 
        {
            IQueryable<TEntityType> query = _dbSet;
            if (disableTracking)
                query = query.AsNoTracking();
            if (include != null)
                query = include(query);
            return query.Where(predicate).ToList();
        }

        public virtual IEnumerable<TEntityType> GetAll(Func<IQueryable<TEntityType>, 
            IIncludableQueryable<TEntityType, object>> include = null, bool disableTracking = true)
        {
            IQueryable<TEntityType> query = _dbSet;
            if (disableTracking)
                query = query.AsNoTracking();
            if (include != null)
                query = include(query);
            return query.ToList();
        }

        public bool Any(Expression<Func<TEntityType, bool>> predicate)
        {
            return _dbSet.Any(predicate);
        }
        #endregion
    }
}
