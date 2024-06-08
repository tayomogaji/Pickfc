using Pickfc.DAL.Interfaces;
using Pickfc.DAL.Infrastructure;
using Pickfc.DAL.Interfaces.IDB;
using Pickfc.Model.Context;
using Pickfc.Model.Entities;

namespace Pickfc.DAL.Repositories
{
    public class ArtRepository : RepositoryBase<Art, PickfcContext>, IArtRepository
    {
        public ArtRepository(IDBFactory<PickfcContext> dbFactory) : base(dbFactory) { }
    }
}
