using Pickfc.Model.Entities;

namespace Pickfc.BLL.Interfaces
{
    public interface ICompTeamService
    {
        /// <summary>
        /// Gets a single comp team via comp & team identifiers
        /// </summary>
        /// <param name="compId">comp identifier</param>
        /// <param name="teamId">team identifier</param>
        /// <returns>Single comp team</returns>
        CompTeam CompTeam(int compId, int teamId);

        /// <summary>
        /// Gets all teams from a comp
        /// </summary>
        /// <param name="compId">Comp identifier</param>
        /// <returns>All teams in a single comp</returns>
        IEnumerable<CompTeam> CompTeamsViaComp(int compId);

        /// <summary>
        /// Gets all comps a team is active in
        /// </summary>
        /// <param name="teamId">Team identifier</param>
        /// <returns>All comps a team is active in</returns>
        IEnumerable<CompTeam> CompTeamsViaTeam(int teamId);

        /// <summary>
        /// Creates new comp team db entery
        /// </summary>
        /// <param name="compTeam">Comp team to be created</param>
        /// <returns>New contest</returns>
        CompTeam Add(CompTeam compTeam);

        /// <summary>
        /// Removes comp team entery from db
        /// </summary>
        /// <param name="compTeam">Comp team to be removed</param>
        void Delete(CompTeam compTeam);

        /// <summary>
        /// checks if comp team input matches amy in db
        /// </summary>
        /// <param name="compTeam">Comp team to check</param>
        /// <returns>True of comp team exsit otherwise false</returns>
        bool Exist(CompTeam compTeam);
    }
}
