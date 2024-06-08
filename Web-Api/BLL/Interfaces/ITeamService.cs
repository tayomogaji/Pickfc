using Pickfc.Model.Entities;

namespace Pickfc.BLL.Interfaces
{
    public interface ITeamService
    {
        /// <summary>
        /// Gets a single team via its unique identifier
        /// </summary>
        /// <param name="id">identifier</param>
        /// <returns>Single team</returns>
        Team Team(int id);

        /// <summary>
        /// Gets teams via comp
        /// </summary>
        /// <param name="compId">Comp identifier</param>
        /// <returns>Teams from a comp</returns>
        IEnumerable<Team> Teams(int compId);

        /// <summary>
        /// Gets All teams from db
        /// </summary>
        /// <returns>All teams</returns>
        IEnumerable<Team> All();

        /// <summary>
        /// Creates new team db entery
        /// </summary>
        /// <param name="team">Team to be created</param>
        /// <returns>New team</returns>
        Team Add(Team team);

        /// <summary>
        /// Updates exsisting team entery
        /// </summary>
        /// <param name="team">Team to be updated</param>
        void Edit (Team team);

        /// <summary>
        /// Removes team entery from db
        /// </summary>
        /// <param name="team"></param>
        void Delete (Team team);

        /// <summary>
        /// Checks if team exist by matching name input with team names from dd 
        /// </summary>
        /// <param name="name">Team name</param>
        /// <returns>True if team exist otherwise false</returns>
        bool Exist(string name);
    }
}
