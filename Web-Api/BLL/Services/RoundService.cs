using Pickfc.BLL.Interfaces;
using Pickfc.DAL.Infrastructure;
using Pickfc.Model.Context;
using Pickfc.Model.Entities;
using Pickfc.DAL.Interfaces;
using Pickfc.DAL.Repositories;

namespace Pickfc.BLL.Services
{
    public class RoundService : IRoundService
    {
        private readonly IRoundRepository roundRepository;
        private readonly IFixtureRepository fixtureRepository;
        private readonly WorkUnit<PickfcContext> workUnit;

        public RoundService(IRoundRepository roundRepository, IFixtureRepository fixtureRepository, IPickRepository pickRepository, WorkUnit<PickfcContext> workUnit) {
            this.roundRepository = roundRepository;
            this.fixtureRepository = fixtureRepository;
            this.workUnit = workUnit;
        }

        public Round Round(int id) {
            return roundRepository.SingleOrDefault(x => x.ID == id);
        }

        public IEnumerable<Round> Rounds(int compid) {
            return roundRepository.GetMany(x => x.CompID == compid);
        }

        public IEnumerable<Round> All() { 
            return roundRepository.GetAll().OrderByDescending(x => x.Timestamped);
        }

        public Round SetFirstRound(int compid) {
            Round round = new Round
            {
                CompID = compid,
                Name = "Round " + 1,
                Number = 1,
                Start = DateTime.Now.AddDays(1),
                Deadline = DateTime.Now.AddDays(7),
                Timestamped = DateTime.Now,
            };
            return round;
        }

        public Round Add(Round round) 
        {
            round.Timestamped = DateTime.Now;
            roundRepository.Add(round);
            workUnit.Commit();
            return round;
        }

        public void Edit(Round round) {
            roundRepository.Update(round);
            workUnit.Commit();
        }

        public void Delete(Round round)
        {
            if (round == null)
                return;

                IEnumerable<Fixture> fixtures = fixtureRepository.GetMany(x => x.RoundID == round.ID);
                if(fixtures != null)
                    foreach (var f in fixtures)
                        fixtureRepository.Delete(f);

                roundRepository.Delete(round);
                workUnit.Commit();
        }
    }
}
