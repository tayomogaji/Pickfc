using Pickfc.DAL.Interfaces;
using Pickfc.BLL.Interfaces;
using Pickfc.DAL.Infrastructure;
using Pickfc.Model.Context;
using Pickfc.Model.Entities;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Pickfc.BLL.Services
{
    public class CompTeamService : ICompTeamService
    {
        private readonly ICompTeamRepository compTeamRepository;
        private readonly WorkUnit<PickfcContext> workUnit;

        public CompTeamService(ICompTeamRepository compTeamRepository, WorkUnit<PickfcContext> workUnit) {
            this.compTeamRepository = compTeamRepository;
            this.workUnit = workUnit;
        }

        public CompTeam CompTeam(int compId, int teamId)
        {
            return compTeamRepository.SingleOrDefault(x => x.CompID == compId && x.TeamID == teamId);
        }

        public IEnumerable<CompTeam> CompTeamsViaComp(int compId)
        {
            return compTeamRepository.GetMany(x => x.CompID == compId);
        }

        public IEnumerable<CompTeam> CompTeamsViaTeam(int teamId)
        {
            return compTeamRepository.GetMany(x => x.TeamID == teamId);
        }

        public CompTeam Add(CompTeam compTeam) {
            compTeam.Timestamped = DateTime.Now;
            compTeamRepository.Add(compTeam);
            workUnit.Commit();
            return compTeam;
        }

        public void Delete(CompTeam compTeam) { 
            if(compTeam != null) {
                compTeamRepository.Delete(compTeam);
                workUnit.Commit();
            }
        }

        public bool Exist(CompTeam compTeam)
        {
            return compTeamRepository.Any(x => x.CompID == compTeam.ID && x.TeamID == compTeam.TeamID);
        }
    }
}
