using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pickfc.BLL.Interfaces;
using Pickfc.Model.Entities;
using Pickfc.UI.ViewModels;

namespace Pickfc.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompController : Controller
    {
        private readonly IGameService gameService;
        private readonly ICompService compService;
        private readonly ICompTeamService compTeamService;
        private readonly ITeamService teamService;
        private readonly IPlayerService playerService;
        private readonly IBackupService backupService;
        private readonly IUserService userService;
        private readonly IRoundService roundService;
        private readonly IPickService pickService;
        private readonly IWebHostEnvironment iwhe;
        private readonly IMapper mapper;

        public CompController(IGameService gameService, ICompService compService, ICompTeamService compTeamService, ITeamService teamService, IPlayerService playerService, IBackupService backupService, IUserService userService, IRoundService roundService, IPickService pickService, IWebHostEnvironment iwhe, IMapper mapper)
        {
            this.gameService = gameService;
            this.compService = compService;
            this.compTeamService = compTeamService;
            this.teamService = teamService;
            this.playerService = playerService;
            this.backupService = backupService;
            this.userService = userService;
            this.roundService = roundService;
            this.pickService = pickService;
            this.iwhe = iwhe;
            this.mapper = mapper;
        }

        [HttpGet("[action]")]
        public CompVm Comp(int id)
        {
            CompVm compVm = mapper.Map<CompVm>(compService.Comp(id));
            compVm.ID = id;
            compVm.Admin = mapper.Map<UserVm>(userService.User(compVm.AdminID));
            CompTeam(compVm);
            return compVm;
        }

        [HttpGet("[action]")]
        public CompVm Default()
        {
            return Comp(compService.Default().ID);
        }

        [HttpGet("[action]")]
        public IEnumerable<CompVm> Comps()
        {
            IEnumerable<CompVm> compsVm = mapper.Map<IEnumerable<CompVm>>(compService.Comps()).OrderBy(x => x.Name);
            foreach (var cvm in compsVm)
            {
                cvm.Admin = mapper.Map<UserVm>(userService.User(cvm.AdminID));
                CompTeam(cvm);
            }
            return compsVm;
        }

        public void CompTeam(CompVm compVm)
        {
            IEnumerable<CompTeam> teams = compTeamService.CompTeamsViaComp(compVm.ID);

            compVm.TeamsCount = teams == null ? 0 : teams.Count();
            compVm.TeamsRemaining = compVm.TeamsTotal - compVm.TeamsCount;
            if (teams != null)
                foreach (CompTeam t in teams)
                    compVm.Teams.Add(mapper.Map<TeamVm>(teamService.Team(t.TeamID)));
        }

        [HttpPost("[action]")]
        public IActionResult Add([FromBody] CompVm compVm)
        {
            int uid = userService.CurrentUser().ID;
            User user = userService.CurrentUser();
            if (user.Admin)
            {
                compVm.AdminID = uid;
                compVm.Legacy = user.FullAdmin;
            }

            if (compService.Comps().Count() == 0)
                compVm.Default = true;

            Comp comp = compService.Add(mapper.Map<Comp>(compVm));
            compVm.ID = comp.ID;
            Game game = gameService.Add(gameService.PublicSet(comp, uid));

            if (comp.Legacy) {
                IEnumerable<User> users = userService.Users();
                foreach (var u in users)
                {
                    bool admin = true ? comp.AdminID == u.ID : false;
                    playerService.Add(playerService.Set(game, u.ID, admin));
                }
            }

            Round round = roundService.Add(roundService.SetFirstRound(comp.ID));
            comp.RoundID = round.ID;
            compService.Edit(comp);

            return Ok(compVm);
        }

        [HttpPut("[action]")]
        public IActionResult Edit([FromBody] CompVm compVm)
        {
            Comp comp = compService.Comp(compVm.ID);
            if (comp == null)
                return NotFound();

            comp.Active = true ? compVm.TeamsCount >= compVm.TeamsTotal : false;
            compService.Edit(mapper.Map(compVm, comp));

            Game publicGame = gameService.Public(comp.ID);
            IEnumerable<Game> games = gameService.Comps(comp.ID);

            if (publicGame != null)
            {
                gameService.PublicGameMapper(publicGame, comp);
                gameService.Edit(publicGame);
            }

            foreach (var game in games)
                if (!game.Public)
                    gameService.Edit(game);

            return Ok(compVm);
        }

        [HttpDelete("[action]")]
        public IActionResult Delete(int id)
        {
            Comp comp = compService.Comp(id);
            RoundsDelete(id);

            foreach (Game game in gameService.Comps(id)) {
                if (playerService.Players(game.ID) != null)
                    foreach (Player player in playerService.Players(game.ID))
                        playerService.Delete(player);

                if (game.Pic != comp.Pic) PicDelete(game.Pic);
                gameService.Delete(game);
            }

            PicDelete(comp.Pic);
            compService.Delete(comp);
            return NoContent();
        }

        [HttpPut("[action]")]
        public IActionResult DefaultSwitch(int id)
        {
            Comp from = compService.Default();
            if (from != null)
            {
                from.Default = false;
                compService.Edit(from);
            }

            Comp to = compService.Comp(id);
            to.Default = true;
            compService.Edit(to);
            return Ok();
        }

        [HttpGet("[action]")]
        public bool HasTeam(int id, int teamid)
        {
            return compTeamService.CompTeam(id, teamid) != null ? true : false;
        }

        [HttpGet("[action]")]
        public bool DefaultExist()
        {
            return compService.DefaultExist();
        }

        [HttpPost("[action]")]
        public IActionResult Reset([FromBody] CompVm compVm) 
        {
            var games = gameService.Comps(compVm.ID);
            
            foreach (var g in games) 
            {
                var players = playerService.Players(g.ID);
                var winner = players.ToList().ElementAt(0);
                winner.Champs++;

                foreach (var p in players)
                {
                    playerService.Reset(p);
                    playerService.Edit(p);
                    backupService.Edit(backupService.Mapper(p, true));
                    if (!p.Active)
                        playerService.Delete(p);
                }
            }
            RoundsDelete(compVm.ID);
            Round round = roundService.Add(roundService.SetFirstRound(compVm.ID));
            Comp comp = compService.Comp(compVm.ID);
            comp.RoundID = round.ID;
            comp.OpenNotified = false;
            compService.Edit(comp);

            return Ok(compVm);
        }

        void RoundsDelete(int compid) {
            var rounds = roundService.Rounds(compid);
            if (rounds != null)
                foreach (var r in rounds)
                {
                    IEnumerable<Pick> picks = pickService.Picks(r.ID);
                    if (picks != null)
                        foreach (var p in picks)
                            pickService.Delete(p);

                    roundService.Delete(r);
                }
        }

        void PicDelete(string pic) {
            if (pic != string.Empty)
            {
                string file = Path.Combine(iwhe.WebRootPath, pic);
                if (System.IO.File.Exists(file))
                    System.IO.File.Delete(file);
            }
        }
    }
}
