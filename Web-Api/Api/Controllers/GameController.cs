using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using Pickfc.BLL.Interfaces;
using Pickfc.Model.Entities;
using Pickfc.UI.ViewModels;

namespace Pickfc.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : Controller
    {
        private readonly IGameService gameService;
        private readonly ICompService compService;
        private readonly IUserService userService;
        private readonly IArtService artService;
        private readonly IPlayerService playerService;
        private readonly IBackupService backupService;
        private readonly IPickService pickService;
        private readonly ITeamService teamService;
        private readonly IRoundService roundService;
        private readonly IWebHostEnvironment iwhe;
        private readonly IMapper mapper;

        public GameController(IGameService gameService, ICompService compService, IUserService userService, IArtService artService, IPlayerService playerService, IBackupService backupService, IPickService pickService, ITeamService teamService, IRoundService roundService, IWebHostEnvironment iwhe, IMapper mapper)
        {
            this.gameService = gameService;
            this.compService = compService;
            this.userService = userService;
            this.artService = artService;
            this.playerService = playerService;
            this.backupService = backupService;
            this.pickService = pickService;
            this.teamService = teamService;
            this.roundService = roundService;
            this.iwhe = iwhe;
            this.mapper = mapper;
        }

        [HttpGet("[action]")]
        public GameVm Game(int id)
        {
            Game game = gameService.Game(id);
            GameVm gameVm = mapper.Map<GameVm>(game);
            UserVm userVm = mapper.Map<UserVm>(userService.CurrentUser());
            IEnumerable<Player> players = playerService.Players(id);

            gameVm.ID = id;
            gameVm.Comp = mapper.Map<CompVm>(compService.Comp(game.CompID));
            gameVm.Creator = mapper.Map<UserVm>(userService.User(game.CreatorID));
            gameVm.CurrentPlayer = mapper.Map<PlayerVm>(playerService.UserPlayer(userVm.ID, id));
            gameVm.CurrentPlayer.User = userVm;
            gameVm.CurrentPlayer.Pos = playerService.Pos(gameVm.CurrentPlayer.ID);

            HitBy(gameVm.CurrentPlayer);
            Pick(gameVm.CurrentPlayer);
            Art(gameVm.CurrentPlayer.User);

            foreach (Player p in players)
            {
                PlayerVm pvm = mapper.Map<PlayerVm>(p);
                User user = userService.User(p.UserID);
                pvm.User = mapper.Map<UserVm>(user);
                Art(pvm.User);
                pvm.Name = pvm.User.FullName;
                pvm.Pos = playerService.Pos(pvm.ID);
                if (pvm.HitByID > 0) {
                    HitBy(pvm);
                }
                if (roundService.Round(compService.Comp(game.CompID).RoundID).Deadline <= DateTime.Now)
                    Pick(pvm);

                gameVm.Players.Add(pvm);
                if (pvm.Admin)
                    gameVm.Admins.Add(pvm);
            }
            return gameVm;
        }

        [HttpGet("[action]")]
        public GameVm Public(int compId)
        {
            return Game(gameService.Public(compId).ID);
        }

        [HttpGet("[action]")]
        public IEnumerable<GameVm> Users()
        {
            User user = userService.CurrentUser();
            IEnumerable<GameVm> gamesVm = mapper.Map<IEnumerable<GameVm>>(gameService.Users(playerService.UserPlayers(user.ID)));
            foreach (var gvm in gamesVm)
            {
                gvm.Creator = mapper.Map<UserVm>(userService.User(gvm.CreatorID));
                gvm.Comp = mapper.Map<CompVm>(compService.Comp(gvm.CompID));
                gvm.RoundDeadlined = roundService.Round(gvm.Comp.RoundID).Deadline <= DateTime.Now;

                gvm.CurrentPlayer = mapper.Map<PlayerVm>(playerService.UserPlayer(user.ID, gvm.ID));
                gvm.CurrentPlayer.User = mapper.Map<UserVm>(user);
                gvm.CurrentPlayer.Pos = playerService.Pos(gvm.CurrentPlayer.ID);
                if (gvm.CurrentPlayer.HitByID > 0)
                    HitBy(gvm.CurrentPlayer);

                Pick(gvm.CurrentPlayer);
                Art(gvm.CurrentPlayer.User);
            }
            return gamesVm.Where(x => x.Comp.Active);
        }

        [HttpPost("[action]")]
        public IActionResult Add([FromBody] GameVm gameVm)
        {
            Comp comp = compService.Comp(gameVm.CompID);
            User user = userService.CurrentUser();

            //if (!CanAdd(user))
            //    return BadRequest("Game creation limit reached");

            if (gameVm.Public)
                gameVm.Name = comp.Name;

            gameVm.Pic = comp.Pic;
            gameVm.CreatorID = user.ID;
            gameVm.Active = comp.Active;

            if (!gameVm.Deadline && gameVm.DeadlineDate != null) gameVm.DeadlineDate = null;

            Game game = gameService.Add(mapper.Map<Game>(gameVm));
            gameVm.ID = game.ID;

            playerService.Add(playerService.Set(game, user.ID, true));

            return Ok(gameVm);
        }

        [HttpPut("[action]")]
        public IActionResult Edit([FromBody] GameVm gameVm)
        {
            Game game = gameService.Game(gameVm.ID);
            if (game == null)
                return NotFound();

            if (!gameVm.Deadline && gameVm.DeadlineDate != null) gameVm.DeadlineDate = null;

            gameService.Edit(mapper.Map(gameVm, game));

            return Ok(gameVm);
        }

        [HttpGet("[action]")]
        public GameVm Join(string code)
        {
            Game game = gameService.GameViaCode(code);
            GameVm gameVm = mapper.Map<GameVm>(game);
            gameVm.ID = game.ID;

            int uid = userService.CurrentUser().ID;
            if (!playerService.Exist(uid, game.ID))
                playerService.Add(playerService.Set(game, uid, false));

            return gameVm;
        }

        [HttpGet("[action]")]
        public bool CanAdd(User user)
        {
            var userVm = mapper.Map<UserVm>(user);
            if (gameService.Creations(userVm.ID) >= 3 && !userVm.FullAdmin) return false; else return true;
        }

        [HttpGet("[action]")]
        public bool ValidCode(string code)
        {
            return gameService.ValidCode(code);
        }

        [HttpDelete("[action]")]
        public IActionResult Delete(int id)
        {
            Game game = gameService.Game(id);

            foreach (Player p in playerService.Players(id))
                playerService.Delete(p);

            foreach (var b in backupService.Backups(id))
                backupService.Delete(b);

            if (game.Pic != string.Empty && game.Pic != compService.Comp(game.CompID).Pic)
            {
                string file = Path.Combine(iwhe.WebRootPath, game.Pic);
                if (System.IO.File.Exists(file))
                    System.IO.File.Delete(file);
            }

            gameService.Delete(game);
            return NoContent();
        }

        public void DeleteExisting(string pic)
        {
            if (pic != string.Empty)
            {
                string file = Path.Combine(iwhe.WebRootPath, pic);
                if (System.IO.File.Exists(file))
                    System.IO.File.Delete(file);
            }
        }

        [HttpPut("[action]")]
        public IActionResult Reset([FromBody] GameVm gameVm) {

            Game game = mapper.Map(gameVm, gameService.Game(gameVm.ID));
            var players = playerService.Players(game.ID);
            var winner = players.ToList().ElementAt(0);
            winner.Champs++;

            foreach (var p in players) {
                playerService.Reset(p);
                playerService.Edit(p);
                backupService.Edit(backupService.Mapper(p, true));
                if (!p.Active)
                    playerService.Delete(p);
            }

            game.ResetDate = DateTime.Now;
            gameService.Code(game);
            gameService.Edit(game);

            return Ok(gameVm);
        }

        public ArtVm Art(UserVm userVm)
        {
            var art = artService.Art(userVm.ArtID);
            if (art != null || userVm.ArtID != 0)
            {
                userVm.Art = mapper.Map<ArtVm>(art);
                userVm.Art.Index = artService.Index(userVm.ArtID) + 1;
            }
            else
            {
                userVm.Art = mapper.Map<ArtVm>(artService.Default());
                userVm.Art.FirstName = userVm.FirstName;
                userVm.Art.LastName = userVm.LastName;
                userVm.Art.Index = 0;
            }
            return userVm.Art;
        }

        public PickVm Pick(PlayerVm player) {
            if (player.PickID != 0)
            {
                player.Pick = mapper.Map<PickVm>(pickService.Pick(player.PickID));
                player.Pick.Team = mapper.Map<TeamVm>(teamService.Team(player.Pick.TeamID));
            }
            return player.Pick;
        }

        public void HitBy(PlayerVm player) {
            if (player.HitByID > 0) {
                User hitBy = userService.User(playerService.Player(player.HitByID).UserID);
                player.HitByName = hitBy.FullName;
            }
        }
    }
}
