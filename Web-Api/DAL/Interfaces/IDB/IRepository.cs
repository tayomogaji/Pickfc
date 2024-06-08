using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Pickfc.DAL.Interfaces.IDB
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Adds new entery
        /// </summary>
        /// <param name="entity">entity to be added</param>
        void Add(T entity);

        /// <summary>
        /// Updates single entery
        /// </summary>
        /// <param name="entity">entity to be updated</param>
        void Update(T entity);

        /// <summary>
        /// Deletes single entity
        /// </summary>
        /// <param name="entity">entity to be deleted</param>
        void Delete(T entity);

        /// <summary>
        /// Gets single entity using delegate filtered by predigate
        /// </summary>
        /// <param name="predicate">func to test if the element satisfies the condition</param>
        /// <param name="include">func to include navigation of properties</param>
        /// <param name="disableTracking">true desables tracked changes, otherwiase false</param>
        /// <returns>single entity using delegate</returns>
        T SingleOrDefault(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);

        T Sord(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="include"></param>
        /// <param name="disableTracking"></param>
        /// <returns></returns>
        T FirstOrDefault(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool disableTracking = true);

        /// <summary>
        /// Gets all eneties filtered by predicate
        /// </summary>
        /// <param name="predicate">func to test if each element meets condition</param>
        /// <param name="include">func to inclde navigation properties</param>
        /// <param name="disableTracking">true desables tracked changes, otherwiase false</param>
        /// <returns></returns>
        IEnumerable<T> GetMany(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool disableTracking = true);

        /// <summary>
        /// Gets all enteries of type T
        /// </summary>
        /// <param name="include">func to inclde navigation properties</param>
        /// <param name="disableTracking">true desables tracked changes, otherwiase false</param>
        /// <returns></returns>
        IEnumerable<T> GetAll(Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool disableTracking = true);

        /// <summary>
        /// Checks if any enteries exist using delegate
        /// </summary>
        /// <param name="predicate">finc to test each element for condidtion</param>
        /// <returns>true if an entity meets the condtion, otherwise false</returns>
        bool Any(Expression<Func<T, bool>> predicate);

        //IQueryable<IGrouping<int, T>> GroupBy(Expression<Func<T, int>> keySelector);
    }
}
