using Pickfc.Model.Entities;

namespace Pickfc.BLL.Interfaces
{
    public interface IFixtureService
    {
        /// <summary>
        /// Gets single fixture via id
        /// </summary>
        /// <param name="id">Fixture identifier</param>
        /// <returns>Single Fixture</returns>
        Fixture Fixture(int id);

        /// <summary>
        /// Gets a team's fixtrue within a given round
        /// </summary>
        /// <param name="teamid">Team identifier</param>
        /// <param name="roundid">Round identifier</param>
        /// <returns>Team's current fixture</returns>
        Fixture TeamFixture(int teamid, int roundid);

        /// <summary>
        /// Gets all fixtures for a round 
        /// </summary>
        /// <param name="roundid">Round identifier</param>
        /// <returns>All fixtures for round</returns>
        IEnumerable<Fixture> Fixtures(int roundid);

        /// <summary>
        /// Creates new fixture db entry
        /// </summary>
        /// <param name="fixture">Fixture to be crerated</param>
        /// <returns>New round</returns>
        Fixture Add(Fixture fixture);

        /// <summary>
        /// Updates exsisting fixture entry
        /// </summary>
        /// <param name="fixture">Fixture to be updated</param>
        void Edit(Fixture fixture);

        /// <summary>
        /// Removes fixture entry from db
        /// </summary>
        /// <param name="fixture">Fixture to be removed</param>
        void Delete(Fixture fixture);

        /// <summary>
        /// Checks if team has been assigned a fixture for a round
        /// </summary>
        /// <param name="teamid">Team identifier</param>
        /// <param name="roundid">Round identifier</param>
        /// <returns>True if team has fixture otherwise false</returns>
        bool HasFixture(int teamid, int roundid);
    }
}
