using Pickfc.Model.Entities;

namespace Pickfc.BLL.Interfaces
{
    public interface IBackupService
    {
        /// <summary>
        /// Gets single backup entery via unique identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Backup entry</returns>
        //Backup Backup(int id);

        /// <summary>
        /// Gets backup entry for a given player
        /// </summary>
        /// <param name="playerid">Player identifier</param>
        /// <returns>Player's backup entry</returns>
        Backup Backup(int playerid);

        /// <summary>
        /// Gets backup entries in a given game
        /// </summary>
        /// <param name="gameid">Game identifier</param>
        /// <returns>Game backup enteries</returns>
        IEnumerable<Backup> Backups(int gameid);

        /// <summary>
        /// Creates new backup db entry
        /// </summary>
        /// <param name="backup">backup to be created</param>
        /// <returns>New backup</returns>
        Backup Add(Backup backup);

        /// <summary>
        /// Updates existing backup entry
        /// </summary>
        /// <param name="backup">Backup entry to be updated</param>
        void Edit(Backup backup);

        /// <summary>
        /// Removes backup entry from db
        /// </summary>
        /// <param name="backup">Backup entry to be removed</param>
        void Delete(Backup backup);

        /// <summary>
        /// mappes player values to backup & vise verse
        /// </summary>
        /// <param name="player">Player to map/map to</param>
        /// <param name="toPlayer">If ture, backup mapps to player else player maps to backup</param>
        /// <returns>Backup with mapped values</returns>
        Backup Mapper(Player player, bool toPlayer);
    }
}
