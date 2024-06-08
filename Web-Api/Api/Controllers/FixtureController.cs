using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pickfc.BLL.Interfaces;
using Pickfc.Model.Entities;
using Pickfc.UI.ViewModels;

namespace Pickfc.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FixtureController : Controller
    {
        private readonly IFixtureService fixtureService;
        private readonly ITeamService teamService;
        private readonly IPickService pickService;
        private readonly IPlayerService playerService;
        private readonly IBackupService backupService;
        private readonly IRoundService roundService;
        private readonly IGameService gameService;
        private readonly IMapper mapper;
        private readonly int maxLife = 3;
        private readonly int hitValue = 2;

        public FixtureController(IFixtureService fixtureService, ITeamService teamService, IPickService pickService, IPlayerService playerService, IBackupService backupService, IRoundService roundService, IGameService gameService, IMapper mapper)
        {
            this.fixtureService = fixtureService;
            this.teamService = teamService;
            this.pickService = pickService;
            this.playerService = playerService;
            this.backupService = backupService;
            this.roundService = roundService;
            this.gameService = gameService;
            this.mapper = mapper;
        }

        [HttpGet("[action]")]
        public FixtureVm Fixture(int id)
        {
            Fixture fixture = fixtureService.Fixture(id);
            FixtureVm fixtureVm = mapper.Map<FixtureVm>(fixture);
            fixtureVm.Home = Team(fixtureVm.HomeID);
            fixtureVm.Away = Team(fixtureVm.AwayID);
            fixtureVm.ID = id;
            return fixtureVm;
        }

        [HttpGet("[action]")]
        public FixtureVm TeamFixture(int teamid, int roundid)
        {
            return Fixture(fixtureService.TeamFixture(teamid, roundid).ID);
        }

        [HttpGet("[action]")]
        public IEnumerable<FixtureVm> Fixtures(int roundid)
        {
            IEnumerable<FixtureVm> fixtureVms = mapper.Map<IEnumerable<FixtureVm>>(fixtureService.Fixtures(roundid));
            foreach (var fvm in fixtureVms)
            {
                fvm.Home = Team(fvm.HomeID);
                fvm.Away = Team(fvm.AwayID);
            }
            return fixtureVms;
        }

        public TeamVm Team(int id)
        {
            return mapper.Map<TeamVm>(teamService.Team(id));
        }

        [HttpPost("[action]")]
        public IActionResult Add([FromBody] FixtureVm fixtureVm)
        {
            Fixture fixture = fixtureService.Add(mapper.Map<Fixture>(fixtureVm));
            fixtureVm.ID = fixture.ID;
            return Ok(fixtureVm);
        }

        [HttpPut("[action]")]
        public IActionResult Edit([FromBody] FixtureVm fixtureVm)
        {
            Fixture fixture = fixtureService.Fixture(fixtureVm.ID);
            if (fixture == null)
                return NotFound();
            fixtureService.Edit(mapper.Map(fixtureVm, fixture));

            if (fixture.HomeResult != "") {
                PicksResult(fixtureVm);
            }
            return Ok(fixtureVm);
        }

        public void PicksResult(FixtureVm fixtureVm)
        {
            IEnumerable<Pick> picks = pickService.Picks(fixtureVm.RoundID);
            if (picks == null)
                return;

            foreach (var p in picks)
            {
                if (p.TeamID == fixtureVm.HomeID)
                {
                    p.Result = fixtureVm.HomeResult;
                    pickService.Edit(p);
                }
                else if (p.TeamID == fixtureVm.AwayID) {
                    p.Result = fixtureVm.AwayResult;
                    pickService.Edit(p);
                }
                PlayerResult(p);
            }
            Pickless(fixtureVm.RoundID);
        }

        public void PlayerResult(Pick pick)
        {
            Player player = playerService.Player(pick.PlayerID);
            if (player == null)
                return;
            
            Backup backup = backupService.Mapper(player, false);

            int pickless = playerService.Players(player.GameID).Where(x => x.PickID == 0).Count();

            int uniquePickBoost = pickService.Picks(pick.RoundID).Where(x => x.TeamID != pick.TeamID && x.GameID == player.GameID).Count() + pickless;
            int uniquePick = uniquePickBoost - playerService.Players(player.GameID).Where(x => x.Eliminated).Count();

            int pts = player.BoostPlayed > 0 ? uniquePickBoost * player.BoostPlayed : uniquePick;

            if (pick.Result == "W") 
                Win(player, pts);
            else 
                NoWin(player, pick.Result);

            TimeSpan timeDiff = pick.Time - roundService.Round(pick.RoundID).Start;
            player.RoundPickTime = timeDiff.TotalDays;
            player.PickTime = player.PickTime + timeDiff.TotalDays;

            playerService.Edit(player);
        }

        public void Win(Player player, int pts) 
        {
            player.RoundPts = pts;
            player.Pts = player.Pts + pts;
            player.Streak++;
            player.BoostTotal++;

            if (player.Streak % 4 == 0 && player.Life < maxLife) player.Life++;
            if (player.Streak % 2 == 0) player.HitsTotal++;
        }

        public void NoWin(Player player, string result)
        {
            player.Streak = 0;
            player.RoundPts = 0;
            if (result == "L") player.Life--;
            if (player.HitByID > 0) player.Life = player.Life - hitValue;
        }

        public void Pickless(int roundid)
        {
            foreach (Game g in gameService.Comps(roundService.Round(roundid).CompID))
                foreach (var p in playerService.Players(g.ID).Where(x => x.PickID == 0 && !x.Eliminated))
                {
                    backupService.Mapper(p, false);
                    NoWin(p, "L");
                    playerService.Edit(p);
                }
        }

        [HttpDelete("[action]")]
        public IActionResult Delete(int id)
        {
            fixtureService.Delete(fixtureService.Fixture(id));
            return NoContent();
        }
    }
}
