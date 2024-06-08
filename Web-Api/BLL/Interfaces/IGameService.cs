using Pickfc.Model.Entities;

namespace Pickfc.BLL.Interfaces
{
    public interface IGameService
    {
        /// <summary>
        /// Gets game from db using integer identifier
        /// </summary>
        /// <param name="id">identifier</param>
        /// <returns>Single game</returns>
        public Game Game(int id);

        /// <summary>
        /// Gets the public game of a contest
        /// </summary>
        /// <param name="compId">Comp identifier</param>
        /// <returns>The public game of Competiton</returns>
        public Game Public(int compId);

        /// <summary>
        /// Sets the default values for a public game
        /// </summary>
        /// <param name="comp">Competition the game is based on</param>
        /// <param name="creatorId">Identity of the user who created the game</param>
        /// <returns>Game with set  default values</returns>
        public Game PublicSet(Comp comp, int creatorId);

        /// <summary>
        /// Maps values of public game to values of parent Comp
        /// </summary>
        /// <param name="publicGame">Public game to be updated</param>
        /// <param name="comp">competition referece</param>
        public void PublicGameMapper(Game publicGame, Comp comp);

        /// <summary>
        /// Gets game from db using unqiue code
        /// </summary>
        /// <param name="code">code to enter</param>
        /// <returns>Single game</returns>
        public Game GameViaCode(string code);

        /// <summary>
        /// Gets all public games from db
        /// </summary>
        /// <returns>All master games</returns>
        public IEnumerable<Game> Publics();


        /// <summary>
        /// Gets all games from db 
        /// </summary>
        /// <returns>All games</returns>
        public IEnumerable<Game> Games();

        /// <summary>
        /// Gets games assosiated with competiton
        /// </summary>
        /// <param name="compId">Competiton idenitifier</param>
        /// <returns>Games assoisted with competiton</returns>
        public IEnumerable<Game> Comps(int compId);

        /// <summary>
        /// Gets games user is currently paricipating in
        /// </summary>
        /// <param name="players">Collection of users players</param>
        /// <returns>Users players</returns>
        public IEnumerable<Game> Users(IEnumerable<Player> players);

        /// <summary>
        /// Gets all legacy games from DB
        /// </summary>
        /// <returns>Legacy games</returns>
        public IEnumerable<Game> Legacies();

        /// <summary>
        /// Creates new game entry in db
        /// </summary>
        /// <param name="game">Game to be created</param>
        /// <returns>Newly created game</returns>
        Game Add(Game game);

        /// <summary>
        /// Updates existing game entry
        /// </summary>
        /// <param name="game">Game to be updated</param>
        void Edit(Game game);

        /// <summary>
        /// Removes game entry
        /// </summary>
        /// <param name="game">Game to be removed</param>
        void Delete(Game game);

        /// <summary>
        /// Generats a random code for a given game
        /// </summary>
        /// <param name="game">Game in need of a code</param>
        /// <returns>Assidnes a code to a game</returns>
        void Code(Game game);

        /// <summary>
        /// Checks if game code exsit within db 
        /// </summary>
        /// <param name="gameCode">Code input</param>
        /// <returns>true if code exist otherwise false</returns>
        public bool ValidCode(string gameCode);

        /// <summary>
        /// Counts the number of games a user has created
        /// </summary>
        /// <param name="uid">user identifier</param>
        /// <returns>number of games a user has created</returns>
        public int Creations(int uid);
    }
}
