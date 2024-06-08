using Pickfc.DAL.Interfaces.IDB;
using Microsoft.EntityFrameworkCore;

namespace Pickfc.DAL.Infrastructure
{
    public abstract class WorkUnit<IRepositioryType> where IRepositioryType : DbContext
    {
        private readonly IDBFactory<IRepositioryType> dBFactory;
        private IRepositioryType dbContext;

        public WorkUnit(IDBFactory<IRepositioryType> dBFactory)
        {
            this.dBFactory = dBFactory;
        }

        public IRepositioryType DbContext
        {
            get { return dbContext ?? (dbContext = dBFactory.Init()); }
        }

        public int Commit() 
        { 
            return DbContext.SaveChanges();
        }
    }
}
