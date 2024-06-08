using Pickfc.DAL.Interfaces;
using Pickfc.BLL.Interfaces;
using Pickfc.DAL.Infrastructure;
using Pickfc.Model.Context;
using Pickfc.Model.Entities;

namespace Pickfc.BLL.Services
{
    public class BackupService : IBackupService
    {
        private readonly IBackupRepository backupRepo;
        private readonly WorkUnit<PickfcContext> workUnit;

        public BackupService(IBackupRepository backupRepo, WorkUnit<PickfcContext> workUnit) {
            this.backupRepo = backupRepo;
            this.workUnit = workUnit;
        }

        public Backup Backup(int playerid) {
            return backupRepo.SingleOrDefault(x => x.PlayerID == playerid);
        }

        public IEnumerable<Backup> Backups(int gameid)
        {
            return backupRepo.GetMany(x => x.GameID == gameid);
        }

        public Backup Add(Backup backup) {
            backup.BackedUp = false;
            backup.Timestamped = DateTime.Now;
            backupRepo.Add(backup);
            workUnit.Commit();
            return backup;
        }

        public void Edit(Backup backup) {
            backupRepo.Update(backup);
            workUnit.Commit();
        }

        public void Delete(Backup backup) {
            if (backup != null) {
                backupRepo.Delete(backup);
                workUnit.Commit();
            }
        }

        public Backup Mapper(Player player, bool toPlayer)
        {
            Backup backup = Backup(player.ID);

            if (toPlayer)
            {
                backup.PlayerID = player.ID;
                backup.Streak = player.Streak;
                backup.Life = player.Life;
                backup.Pts = player.Pts;
                backup.BoostTotal = player.BoostTotal;
                backup.HitsTotal = player.HitsTotal;
                //backup.HitByID = player.HitByID;
                backup.Eliminated = player.Eliminated;
                backup.PickTime = player.PickTime;
            }
            else {
                player.Streak = backup.Streak;
                player.Life = backup.Life;
                player.Pts = backup.Pts;
                //player.HitByID = backup.HitByID;
                player.BoostTotal = backup.BoostTotal;
                player.HitsTotal = backup.HitsTotal;
                player.Eliminated = backup.Eliminated;
                player.PickTime = backup.PickTime;
            }
            return backup;
        }
    }
}
