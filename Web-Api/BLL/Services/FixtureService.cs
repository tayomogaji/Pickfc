using Pickfc.BLL.Interfaces;
using Pickfc.DAL.Infrastructure;
using Pickfc.Model.Context;
using Pickfc.Model.Entities;
using Pickfc.DAL.Interfaces;

namespace Pickfc.BLL.Services
{
    public class FixtureService : IFixtureService
    {
        private readonly IFixtureRepository fixtureRepository;
        private readonly WorkUnit<PickfcContext> workUnit;

        public FixtureService(IFixtureRepository fixtureRepository, WorkUnit<PickfcContext> workUnit) {
            this.fixtureRepository = fixtureRepository;
            this.workUnit = workUnit;
        }

        public Fixture Fixture(int id) 
        {
            return fixtureRepository.SingleOrDefault(x => x.ID == id);
        }

        public Fixture TeamFixture(int teamid, int roundid)
        {
            return fixtureRepository.SingleOrDefault(x => x.RoundID == roundid && x.HomeID == teamid | x.AwayID == teamid);
        }

        public IEnumerable<Fixture> Fixtures(int roundid) 
        {
            return fixtureRepository.GetMany(x => x.RoundID == roundid);
        }

        public Fixture Add(Fixture fixture) 
        {
            fixture.TimeStamped = DateTime.Now;
            fixtureRepository.Add(fixture);
            workUnit.Commit();
            return fixture;
        }

        public void Edit(Fixture fixture) 
        {
            fixtureRepository.Update(fixture);
            workUnit.Commit();
        }

        public void Delete(Fixture fixture)
        {
            if (fixture != null)
            {
                fixtureRepository.Delete(fixture);
                workUnit.Commit();
            }
        }

        public bool HasFixture(int teamid, int roundid)
        {
            return fixtureRepository.Any(x => (x.HomeID == teamid || x.AwayID == teamid) && x.RoundID == roundid);
        }
    }
}
