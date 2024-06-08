using Pickfc.DAL.Interfaces;
using Pickfc.BLL.Interfaces;
using Pickfc.DAL.Infrastructure;
using Pickfc.Model.Context;
using Pickfc.Model.Entities;
using Microsoft.Identity.Client;

namespace Pickfc.BLL.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository teamRepository;
        private readonly ICompTeamRepository compTeamRepository;
        private readonly WorkUnit<PickfcContext> workUnit;

        public TeamService(ITeamRepository teamRepository, ICompTeamRepository compTeamRepository, WorkUnit<PickfcContext> workUnit) {
            this.teamRepository = teamRepository;
            this.compTeamRepository = compTeamRepository;
            this.workUnit = workUnit;
        }

        public Team Team(int id) {
            return teamRepository.SingleOrDefault(x => x.ID == id);
        }

        public IEnumerable<Team> Teams(int compId) {
            List<Team> teams = new List<Team>();
            IEnumerable<CompTeam> compTeams = compTeamRepository.GetMany(x => x.CompID == compId);
            foreach (var ct in compTeams)
                teams.Add(teamRepository.SingleOrDefault(x => x.ID == ct.TeamID));

            return teams.OrderBy(x => x.Name);
        }
        
        public IEnumerable<Team> All() {
            return teamRepository.GetAll();
        }

        public Team Add(Team team) {
            team.Timestamped = DateTime.Now;
            teamRepository.Add(team);
            workUnit.Commit();
            return team;
        }

        public void Edit(Team team) {
            teamRepository.Update(team);
            workUnit.Commit();
        }

        public void Delete(Team team) {
            if (team != null) 
            {
                IEnumerable<CompTeam> compTeam = compTeamRepository.GetMany(x => x.TeamID == team.ID);
                foreach (var ct in compTeam) {
                    compTeamRepository.Delete(ct);
                }
                teamRepository.Delete(team);
                workUnit.Commit();
            }
        }

        public bool Exist(string name) {
            return teamRepository.Any(x => x.Name.ToLower() == name.ToLower());
        }
    }
}
