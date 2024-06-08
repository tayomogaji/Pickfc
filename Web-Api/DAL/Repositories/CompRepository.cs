using Pickfc.DAL.Interfaces;
using Pickfc.DAL.Infrastructure;
using Pickfc.DAL.Interfaces.IDB;
using Pickfc.Model.Context;
using Pickfc.Model.Entities;

namespace Pickfc.DAL.Repositories
{
    public class CompRepository : RepositoryBase<Comp, PickfcContext>, ICompRepository
    {
        public CompRepository(IDBFactory<PickfcContext> dbFactory) : base(dbFactory) { }
    }
}
