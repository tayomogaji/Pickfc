using Pickfc.BLL.Interfaces;
using Pickfc.DAL.Infrastructure;
using Pickfc.Model.Context;
using Pickfc.Model.Entities;
using Pickfc.DAL.Interfaces;

namespace Pickfc.BLL.Services
{
    public class PickService : IPickService
    {
        private readonly IPickRepository pickRepository;
        private readonly WorkUnit<PickfcContext> workUnit;

        public PickService(IPickRepository pickRepository, WorkUnit<PickfcContext> workUnit) {
            this.pickRepository = pickRepository;
            this.workUnit = workUnit;
        }

        public Pick Pick(int id) {
            return pickRepository.SingleOrDefault(x => x.ID == id);
        }

        public IEnumerable<Pick> Picks(int roundid)
        {
            return pickRepository.GetMany(x => x.RoundID == roundid);
        }

        public Pick PlayerPick(int playerid, int gameid, int roundid) 
        {
            return pickRepository.SingleOrDefault(x => x.PlayerID == playerid && x.GameID == gameid && x.RoundID == roundid);
        }

        public IEnumerable<Pick> PlayerPicks(int playerid, int gameid) 
        {
            return pickRepository.GetMany(x => x.PlayerID == playerid && x.GameID == gameid).OrderByDescending(x => x.Timestamped);
        }

        public Pick Add(Pick pick) 
        { 
            pick.Timestamped = DateTime.Now;
            pick.Time = DateTime.Now;
            pickRepository.Add(pick);
            workUnit.Commit();
            return pick;
        }

        public void Edit(Pick pick) 
        { 
            pickRepository.Update(pick);
            workUnit.Commit();
        }

        public void Delete(Pick pick)
        {
            if (pick != null) 
            { 
                pickRepository.Delete(pick);
                workUnit.Commit();
            }
        }

        public bool PickValid(int playerid, int teamid) 
        {
            return pickRepository.Any(x => x.PlayerID == playerid && x.TeamID != teamid);
        }

        public bool PickExist(int playerid, int gameid, int roundid) {
            return pickRepository.Any(x => x.PlayerID == playerid && x.GameID == gameid && x.RoundID == roundid);
        }
    }
}
