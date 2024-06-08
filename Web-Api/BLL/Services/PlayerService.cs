using Pickfc.DAL.Interfaces;
using Pickfc.BLL.Interfaces;
using Pickfc.DAL.Infrastructure;
using Pickfc.Model.Context;
using Pickfc.Model.Entities;

namespace Pickfc.BLL.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository playerRepository;
        private readonly IPickRepository pickRepository;
        private readonly IBackupRepository backupRepository;
        private readonly int life = 2;
        private readonly WorkUnit<PickfcContext> workUnit;

        public PlayerService(IPlayerRepository playerRepository, IPickRepository pickRepository, IBackupRepository backupRepository, WorkUnit<PickfcContext> workUnit) {
            this.playerRepository = playerRepository;
            this.pickRepository = pickRepository;
            this.backupRepository = backupRepository;
            this.workUnit = workUnit;
        }

        public Player Player(int id) {
            return playerRepository.SingleOrDefault(x => x.ID == id);
        }

        public Player UserPlayer(int userId, int gameId) {
            return playerRepository.SingleOrDefault(x => x.UserID == userId && x.GameID == gameId);
        }

        public IEnumerable<Player> Players(int gameId) {
            return playerRepository.GetMany(x => x.GameID == gameId).OrderByDescending(x => x.Pts).ThenByDescending(x => x.PickTime > 0).ThenBy(x => x.PickTime).ThenBy(x => x.TimeStamped);
        }

        public IEnumerable<Player> UserPlayers(int userId) { 
            return playerRepository.GetMany(x => x.UserID == userId);
        }

        public IEnumerable<Player> CompPlayers(int compId) {
            return playerRepository.GetMany(x => x.CompID == compId);
        }

        public Player Set(Game game, int userid, bool admin) {
            Player player = new Player()
            {
                UserID = userid,
                GameID = game.ID,
                CompID = game.CompID,
                Admin = admin,
                Active = true,
                TimeStamped = DateTime.Now
            };
            return player;
        }

        public Player Add(Player player) {

            IEnumerable<Player> players = playerRepository.GetMany(x => x.GameID == player.GameID && x.UserID == player.UserID);

            if (players != null)
                foreach (var p in players)
                    playerRepository.Delete(p);

            player.Life = life;
            playerRepository.Add(player);
            workUnit.Commit();
            Backup backup = new Backup
            {
                GameID = player.GameID,
                PlayerID = player.ID,
                Life = player.Life,
                Timestamped = DateTime.Now
            };
            backupRepository.Add(backup);
            workUnit.Commit();

            return player;
        }

        public void Edit(Player player) {
            if (player == null)
                return;

            if (!player.Active)
                HitListReset(player.ID);

            playerRepository.Update(player);
            workUnit.Commit();
        }

        public void Delete(Player player) {
            if (player == null)
                return;

            IEnumerable<Pick> picks = pickRepository.GetMany(x => x.PlayerID == player.ID);
            if (picks != null)
                foreach (var pick in picks)
                    pickRepository.Delete(pick);

            if (Backup(player.ID) != null)
                backupRepository.Delete(Backup(player.ID));

            HitListReset(player.ID);
            playerRepository.Delete(player);
            workUnit.Commit();
        }

        public void Reset(Player player)
        {
            if (player == null)
                return;

            player.PickID = 0;
            player.Pts = 0;
            player.HitByID = 0;
            player.HitsTotal = 0;
            player.HitsPlayed = 0;
            player.BoostTotal = 0;
            player.BoostPlayed = 0;
            player.Streak = 0;
            player.Life = life;
            player.PickTime = 0;
            player.Eliminated = false;
        }

        public void HitListReset(int id) {
            if (playerRepository.Any(x => x.HitByID == id)) {
                IEnumerable<Player> players = playerRepository.GetMany(x => x.HitByID == id);
                foreach (var p in players) {
                    p.HitByID = 0;
                    Edit(p);
                    Backup(p.ID).HitByID = 0;
                    backupRepository.Update(Backup(p.ID));
                    workUnit.Commit();
                }
            }
        }

        public void Winner(int gameid) {
            var winner = Players(gameid).ToList().ElementAt(0);
            winner.Champs++;
            Edit(winner);
        }

        public Backup Backup(int id) {
            return backupRepository.SingleOrDefault(x => x.PlayerID == id);
        }

        public bool Exist(int userId, int gameId)
        {
            return playerRepository.Any(x => x.UserID == userId && x.GameID == gameId);
        }

        public bool Active(int userId, int gameId) {
            return playerRepository.Any(x => x.UserID == userId && x.GameID == gameId && x.Active);
        }

        public int Pos(int id) {
            return Players(Player(id).GameID).ToList().FindIndex(x => x.ID == id) + 1;
        }
    }
}
