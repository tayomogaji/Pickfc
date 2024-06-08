using Pickfc.Model.Entities;

namespace Pickfc.BLL.Interfaces
{
    public interface IRoundService
    {
        /// <summary>
        /// Gets single round via id
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Single Round</returns>
        Round Round(int id);

        /// <summary>
        /// Gets all rounds for comp 
        /// </summary>
        /// <param name="compid">Comp identifier</param>
        /// <returns>Competition rounds</returns>
        IEnumerable<Round> Rounds(int compid);

        /// <summary>
        /// Gets all rounds from db
        /// </summary>
        /// <returns>All rounds</returns>
        IEnumerable<Round> All();

        /// <summary>
        /// Creates new round db entry
        /// </summary>
        /// <param name="round">Round to be crerated</param>
        /// <returns>New round</returns>
        Round Add(Round round);

        /// <summary>
        /// Updates exsisting round entry
        /// </summary>
        /// <param name="round">Round to be updated</param>
        void Edit(Round round);

        /// <summary>
        /// Removes round entry from db
        /// </summary>
        /// <param name="round">Round to be removed</param>
        void Delete(Round round);

        /// <summary>
        /// Sets the values of the very first round of a competiton
        /// </summary>
        /// <param name="compid">Competition identifier</param>
        /// <returns>New first round</returns>
        Round SetFirstRound(int compid); 
    }
}
