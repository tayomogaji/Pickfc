using AutoMapper;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Mvc;
using Pickfc.BLL.Interfaces;
using Pickfc.Model.Entities;
using Pickfc.UI.ViewModels;

namespace Pickfc.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : Controller
    {
        private readonly ITeamService teamService;
        private readonly ICompTeamService compTeamService;
        private readonly IFixtureService fixtureService;
        private readonly IRoundService roundService;
        private readonly ICompService compService;
        private readonly IWebHostEnvironment iwhe;
        private readonly IMapper mapper;

        public TeamController(ITeamService teamService, ICompTeamService compTeamService, ICompService compService, IFixtureService fixtureService, IRoundService roundService, IWebHostEnvironment iwhe, IMapper mapper)
        {
            this.teamService = teamService;
            this.compTeamService = compTeamService;
            this.compService = compService;
            this.fixtureService = fixtureService;
            this.roundService = roundService;
            this.iwhe = iwhe;
            this.mapper = mapper;
        }

        [HttpGet("[action]")]
        public TeamVm Team(int id)
        {
            Team team = teamService.Team(id);
            TeamVm teamVm = mapper.Map<TeamVm>(team);
            teamVm.ID = id;
            TeamComps(teamVm);
            return teamVm;
        }

        [HttpGet("[action]")]
        public IEnumerable<TeamVm> Teams(int compId)
        {
            IEnumerable<TeamVm> teamsVm = mapper.Map<IEnumerable<TeamVm>>(teamService.Teams(compId));
            foreach (var t in teamsVm) {
                t.HasFixture = fixtureService.HasFixture(t.ID, compService.Comp(compId).RoundID);
                TeamComps(t);
            }
            return teamsVm.OrderBy(x => x.Name);
        }

        public void TeamComps(TeamVm teamVm)
        {
            IEnumerable<CompTeam> comps = compTeamService.CompTeamsViaTeam(teamVm.ID);
            teamVm.CompsCount = comps == null ? 0 : comps.Count();
            if (comps != null)
                foreach (CompTeam c in comps)
                {
                    teamVm.Comps.Add(mapper.Map<CompVm>(compService.Comp(c.CompID)));
                }
        }

        [HttpGet("[action]")]
        public IEnumerable<TeamVm> All()
        {
            IEnumerable<TeamVm> teamsVm = mapper.Map<IEnumerable<TeamVm>>(teamService.All());
            foreach (var tvm in teamsVm) {
                TeamComps(tvm);
            }
            return teamsVm.OrderBy(x => x.Name);
        }

        [HttpGet("[action]")]
        public IEnumerable<TeamVm> Fixtureless(int roundid)
        {
            List<TeamVm> fixtureless = new List<TeamVm>();
            foreach (var fixture in fixtureService.Fixtures(roundid))
                foreach (var team in mapper.Map<IEnumerable<TeamVm>>(teamService.Teams(roundService.Round(roundid).CompID)))
                {
                    fixtureless.Add(team);
                    if (team.ID == fixture.HomeID | team.ID == fixture.AwayID)
                        fixtureless.Remove(team);
                }
            return fixtureless.OrderBy(x => x.Name);
        }

        [HttpPost("[action]")]
        public IActionResult Add([FromBody] TeamVm teamVm)
        {
            if (teamService.Exist(teamVm.Name))
                return NoContent();

            Team team = teamService.Add(mapper.Map<Team>(teamVm));
            teamVm.ID = team.ID;
            teamVm.Timestamped = team.Timestamped;
            return Ok(teamVm);
        }

        [HttpPut("[action]")]
        public IActionResult Edit([FromBody] TeamVm teamVm)
        {
            Team team = teamService.Team(teamVm.ID);
            if (team == null)
                return NotFound();

            teamService.Edit(mapper.Map(teamVm, team));
            return Ok(teamVm);
        }

        [HttpDelete("[action]")]
        public IActionResult Delete(int id)
        {
            Team team = teamService.Team(id);
            IEnumerable<CompTeam> compTeams = compTeamService.CompTeamsViaTeam(id);
            foreach (CompTeam ct in compTeams)
                compTeamService.Delete(ct);

            if (team.Pic != string.Empty)
            {
                string file = Path.Combine(iwhe.WebRootPath, team.Pic);
                if (System.IO.File.Exists(file))
                    System.IO.File.Delete(file);
            }
            teamService.Delete(team);
            return NoContent();
        }

        [HttpPost("[action]")]
        public IActionResult CompAdd([FromBody] TeamVm teamVm)
        {
            CompTeam compTeam = new CompTeam
            {
                CompID = teamVm.CompID,
                TeamID = teamVm.ID
            };

            if (compTeamService.Exist(compTeam))
                return NoContent();

            compTeamService.Add(compTeam);
            return Ok();
        }

        //removes team from comp
        [HttpDelete("[action]")]
        public IActionResult CompDelete(int compid, int teamid)
        {
            CompTeam compTeam = compTeamService.CompTeam(compid, teamid);
            if (compTeam == null)
                return NotFound();

            compTeamService.Delete(compTeam);
            return Ok();
        }

        [HttpGet("[action]")]
        public bool Exist(string name)
        {
            return teamService.Exist(name);
        }
    }
}
