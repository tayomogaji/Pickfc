using Pickfc.DAL.Interfaces;
using Pickfc.DAL.Infrastructure;
using Pickfc.DAL.Interfaces.IDB;
using Pickfc.Model.Context;
using Pickfc.Model.Entities;

namespace Pickfc.DAL.Repositories
{
    public class CompTeamRepository : RepositoryBase<CompTeam, PickfcContext>, ICompTeamRepository
    {
        public CompTeamRepository(IDBFactory<PickfcContext> dbFactory) : base(dbFactory) { }
    }
}
