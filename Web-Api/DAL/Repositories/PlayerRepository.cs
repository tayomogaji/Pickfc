using Pickfc.DAL.Infrastructure;
using Pickfc.DAL.Interfaces;
using Pickfc.DAL.Interfaces.IDB;
using Pickfc.Model.Context;
using Pickfc.Model.Entities;

namespace Pickfc.DAL.Repositories
{
    public class PlayerRepository : RepositoryBase<Player, PickfcContext>, IPlayerRepository
    {
        public PlayerRepository(IDBFactory<PickfcContext> dbFactory) : base(dbFactory) { }
    }
}
