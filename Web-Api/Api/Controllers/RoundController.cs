using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pickfc.BLL.Interfaces;
using Pickfc.Model.Entities;
using Pickfc.UI.ViewModels;
using System.Numerics;

namespace Pickfc.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoundController : Controller
    {
        private readonly IRoundService roundService;
        private readonly IFixtureService fixtureService;
        private readonly IPickService pickService;
        private readonly ITeamService teamService;
        private readonly IPlayerService playerService;
        private readonly IUserService userService;
        private readonly ICompService compService;
        private readonly IGameService gameService;
        private readonly IBackupService backupService;
        private readonly IMapper mapper;

        public RoundController(IRoundService roundService, IFixtureService fixtureService, IPickService pickService, ITeamService teamService, IPlayerService playerService, IUserService userService, ICompService compService, IGameService gameService, IBackupService backupService, IMapper mapper)
        {
            this.roundService = roundService;
            this.fixtureService = fixtureService;
            this.pickService = pickService;
            this.teamService = teamService;
            this.playerService = playerService;
            this.userService = userService;
            this.compService = compService;
            this.gameService = gameService;
            this.backupService = backupService;
            this.mapper = mapper;
        }

        [HttpGet("[action]")]
        public RoundVm Round(int id)
        {
            RoundVm roundVm = mapper.Map<RoundVm>(roundService.Round(id));

            RoundName(roundVm);
            roundVm.Current = compService.Comp(roundVm.CompID).RoundID == id;
            roundVm.ID = id;

            foreach (FixtureVm fvm in Fixtures(id))
                roundVm.Fixtures.Add(fvm);

            SetPicks(roundVm);
            Deadline(roundVm);

            return roundVm;
        }

        [HttpGet("[action]")]
        public IEnumerable<RoundVm> Rounds(int compid)
        {
            IEnumerable<RoundVm> roundsVm = mapper.Map<IEnumerable<RoundVm>>(roundService.Rounds(compid));
            foreach (var rvm in roundsVm)
            {
                RoundName(rvm);
                rvm.Current = compService.Comp(rvm.CompID).RoundID == rvm.ID;

                foreach (var fvm in Fixtures(rvm.ID))
                    rvm.Fixtures.Add(fvm);

                SetPicks(rvm);
                Deadline(rvm);
            }
            return roundsVm.OrderByDescending(x => x.Current).ThenByDescending(x => x.Show).ThenBy(x => x.Number);
        }

        [HttpPost("[action]")]
        public IActionResult Add([FromBody] RoundVm roundVm)
        {
            Round round = roundService.Add(mapper.Map<Round>(roundVm));
            roundVm.ID = round.ID;
            return Ok(roundVm);
        }

        [HttpPut("[action]")]
        public IActionResult Edit([FromBody] RoundVm roundVm)
        {
            Round round = roundService.Round(roundVm.ID);
            if (round == null)
                return NotFound();

            roundService.Edit(mapper.Map(roundVm, round));
            return Ok(roundVm);
        }

        [HttpPut("[action]")]
        public IActionResult SetCurrentRound([FromBody] RoundVm roundVm)
        {
            Round round = roundService.Round(roundVm.ID);
            Comp comp = compService.Comp(round.CompID);

            if (round == null) return NotFound();

            comp.RoundID = round.ID;
            compService.Edit(comp);

            foreach (var g in gameService.Comps(comp.ID))
                foreach (var p in playerService.Players(g.ID))
                {
                    p.PickID = 0;
                    p.HitByID = 0;
                    p.BoostPlayed = 0;
                    p.HitsPlayed = 0;
                    if (p.Life <= 0) {
                        p.Eliminated = true;
                        p.BoostTotal = 0;
                        p.HitsTotal = 0;
                    }

                    playerService.Edit(p);
                    backupService.Mapper(p, true);
                    backupService.Edit(backupService.Backup(p.ID));
                }
            return Ok(roundVm);
        }

        [HttpDelete("[action]")]
        public IActionResult Delete(int id)
        {
            Round round = roundService.Round(id);
            roundService.Delete(round);
            return NoContent();
        }

        public string RoundName(RoundVm roundVm)
        {
            return roundVm.Name = "Round " + roundVm.Number.ToString();
        }

        public TeamVm Team(int id)
        {
            return mapper.Map<TeamVm>(teamService.Team(id));
        }

        public IEnumerable<FixtureVm> Fixtures(int id)
        {
            IEnumerable<FixtureVm> fixturesVm = mapper.Map<IEnumerable<FixtureVm>>(fixtureService.Fixtures(id));

            foreach (var fvm in fixturesVm)
            {
                fvm.Home = Team(fvm.HomeID);
                fvm.Away = Team(fvm.AwayID);
            }
            return fixturesVm;
        }

        public void SetPicks(RoundVm roundVm) {
            if (pickService.Picks(roundVm.ID) != null)
                foreach (Pick p in pickService.Picks(roundVm.ID))
                {
                    PickVm pvm = mapper.Map<PickVm>(p);
                    User user = userService.User(playerService.Player(pvm.PlayerID).UserID);

                    pvm.Team = Team(pvm.TeamID);
                    pvm.PlayerName = user.FullName;
                    //pvm.PlayerPic = user.Pic;
                    roundVm.Picks.Add(pvm);
                }
        }

        public void Deadline(RoundVm roundVm) {
            TimeSpan timeDiff = roundVm.Deadline - DateTime.Now;
            roundVm.DeadlineMsecs = (int)Math.Floor(timeDiff.TotalMilliseconds);
            roundVm.DeadlineDays = (int)Math.Floor(timeDiff.TotalDays);
        }
    }
}
