using Microsoft.Data.SqlClient.DataClassification;
using Pickfc.BLL.Services;
using Pickfc.Model.Entities;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Pickfc.BLL.Interfaces
{
    public interface IPlayerService
    {
        /// <summary>
        /// Gets player by unique identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Single player</returns>
        Player Player(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Single player</returns>

        /// <summary>
        /// Gets current user player from a single game
        /// </summary>
        /// <param name="userId">User's identifier</param>
        /// <param name="gameId">Game identifier</param>
        /// <returns>Users player from game</returns>
        Player UserPlayer(int userId, int gameId);

        /// <summary>
        /// Gets all players actively participating in a game
        /// </summary>
        /// <param name="gameId">Game identifier</param>
        /// <returns>List of active players from a game</returns>
        IEnumerable<Player> Players(int gameId);

        /// <summary>
        /// gets player entities assosicated with user
        /// </summary>
        /// <param name="userId">user identifier</param>
        /// <returns>user players list</returns>
        IEnumerable<Player> UserPlayers(int userId);

        /// <summary>
        /// Sets the default options for a new player
        /// </summary>
        /// <param name="game">game player will be added to</param>
        /// <param name="userId">User identifier</param>
        /// <param name="admin">checks if player is admin</param>
        /// <returns>Single player</returns>
        Player Set(Game game, int userId, bool admin);

        /// <summary>
        /// Creates a new player entry to db
        /// </summary>
        /// <param name="player">Player to be added</param>
        /// <returns>New player</returns>
        Player Add(Player player);

        /// <summary>
        /// Updates an exsisting player entery in db
        /// </summary>
        /// <param name="player">Player to be updated</param>
        void Edit(Player player);

        /// <summary>
        /// Removes player entery from db
        /// </summary>
        /// <param name="player">Player to be removed</param>
        void Delete(Player player);

        /// <summary>
        /// Resets player game stats & tokens 
        /// </summary>
        /// <param name="player">Player to reset</param>
        void Reset(Player player);

        /// <summary>
        /// Finds and awards a championship point to the winner of a game
        /// </summary>
        /// <param name="gameid">Game identifier</param>
        void Winner(int gameid);

        /// <summary>
        /// Checks if player(user) already exisit in a game
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="gameId">Game identifier</param>
        /// <returns>True if player(user) already exist in a game otherwise false</returns>
        bool Exist(int userId, int gameId);

        /// <summary>
        /// Checks if user player is active
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="gameId"></param>
        /// <returns>Ture if active otherwise false</returns>
        bool Active(int userId, int gameId);

        /// <summary>
        /// Get current rank prosition of player by ID
        /// </summary>
        /// <param name="id">Player identifier</param>
        /// <returns>Player's rank number</returns>
        int Pos(int id); 
    }
}
