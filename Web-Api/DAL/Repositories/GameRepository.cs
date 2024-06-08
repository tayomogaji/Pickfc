using Pickfc.DAL.Infrastructure;
using Pickfc.DAL.Interfaces;
using Pickfc.DAL.Interfaces.IDB;
using Pickfc.Model.Context;
using Pickfc.Model.Entities;

namespace Pickfc.DAL.Repositories
{
    public class GameRepository : RepositoryBase<Game, PickfcContext>, IGameRepository
    {
        public GameRepository(IDBFactory<PickfcContext> dbFactory) : base(dbFactory) { }
    }
}
