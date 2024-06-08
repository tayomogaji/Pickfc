using Pickfc.DAL.Interfaces;
using Pickfc.DAL.Infrastructure;
using Pickfc.DAL.Interfaces.IDB;
using Pickfc.Model.Context;
using Pickfc.Model.Entities;

namespace Pickfc.DAL.Repositories
{
    public class BackupRepository : RepositoryBase<Backup, PickfcContext>, IBackupRepository
    {
        public BackupRepository(IDBFactory<PickfcContext> dbFactory) : base(dbFactory) { }
    }
}
