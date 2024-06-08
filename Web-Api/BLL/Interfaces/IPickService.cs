using Pickfc.Model.Entities;

namespace Pickfc.BLL.Interfaces
{
    public interface IPickService
    {
        /// <summary>
        /// Gets a single pick by identifier
        /// </summary>
        /// <param name="id">Unique identifier</param>
        /// <returns>Single pick from db</returns>
        Pick Pick(int id);

        /// <summary>
        /// Gets all player picks from a given round
        /// </summary>
        /// <param name="roundid">Round identifier</param>
        /// <returns>All picks from a round</returns>
        IEnumerable<Pick> Picks(int roundid);

        /// <summary>
        /// Gets a player's pick from a given round in a game
        /// </summary>
        /// <param name="playerid">Player identifier</param>
        /// <param name="gameid">Game identifier</param>
        /// <param name="roundid">Round identitifer</param>
        /// <returns>Players round pick</returns>
        Pick PlayerPick(int playerid, int gameid, int roundid);

        /// <summary>
        /// Gets all picks made by a player from a game
        /// </summary>
        /// <param name="player">Player's picks to get</param>
        /// <returns>All picks made by player</returns>
        IEnumerable<Pick> PlayerPicks(int playerid, int gameid);

        /// <summary>
        /// Creates new pick db entry 
        /// </summary>
        /// <param name="pick">Pick to be added</param>
        /// <returns>New pick</returns>
        Pick Add(Pick pick);

        /// <summary>
        /// Updates exsisting pick entry
        /// </summary>
        /// <param name="pick">Pick to be updated</param>
        void Edit(Pick pick);

        /// <summary>
        /// Removes pick entry from db 
        /// </summary>
        /// <param name="pick">Pick to be removed</param>
        void Delete(Pick pick);

        /// <summary>
        /// Checks if team has been picked of set number of times
        /// </summary>
        /// <param name="playerid">Player identifier</param>
        /// <param name="teamid">Team identifier</param>
        /// <returns></returns>
        bool PickValid(int playerid, int teamid);

        /// <summary>
        /// Checks if a player has already made a pick in a given game round
        /// </summary>
        /// <param name="playerid">Player identifier</param>
        /// <param name="gameid">Game identifier</param>
        /// <param name="roundid">Round identifier</param>
        /// <returns>True if pick has been made otherwise flase</returns>
        bool PickExist(int playerid, int gameid, int roundid);
    }
}
