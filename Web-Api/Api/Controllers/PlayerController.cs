using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pickfc.BLL.Interfaces;
using Pickfc.BLL.Services;
using Pickfc.Model.Entities;
using Pickfc.UI.ViewModels;

namespace Pickfc.UI.Controllers
{
    [Route("api/[controller]")]
    public class PlayerController : Controller
    {
        private readonly IPlayerService playerService;
        private readonly IBackupService backupService;
        private readonly IUserService userService;
        private readonly IPickService pickService;
        private readonly ITeamService teamService;
        private readonly IArtService artService;
        private readonly IMapper mapper;

        public PlayerController(IPlayerService playerService, IBackupService backupService, IPickService pickService, ITeamService teamService, IUserService userService, IArtService artService, IMapper mapper)
        {
            this.playerService = playerService;
            this.backupService = backupService;
            this.userService = userService;
            this.pickService = pickService;
            this.teamService = teamService;
            this.artService = artService;
            this.mapper = mapper;
        }

        [HttpGet("[action]")]
        public PlayerVm Player(int id)
        {
            Player player = playerService.Player(id);
            PlayerVm playerVm = mapper.Map<PlayerVm>(player);

            playerVm.User = mapper.Map<UserVm>(userService.User(player.UserID));
            //Art(playerVm.User);
            playerVm.Name = playerVm.User.FullName;
            playerVm.Pos = playerService.Pos(playerVm.ID);
            if (playerVm.HitByID > 0)
            {
                User hitBy = userService.User(playerService.Player(player.HitByID).UserID);
                playerVm.HitByName = hitBy.FullName;
            }
            GetPick(playerVm);
            playerVm.ID = id;
            return playerVm;
        }

        [HttpGet("[action]")]
        public IEnumerable<PlayerVm> Players(int gameId)
        {
            IEnumerable<Player> players = playerService.Players(gameId);
            IEnumerable<PlayerVm> playersVm = mapper.Map<IEnumerable<PlayerVm>>(players);

            foreach (var p in players)
                foreach (var pvm in playersVm)
                {
                    pvm.User = mapper.Map<UserVm>(userService.User(p.UserID));
                    //Art(pvm.User);
                    pvm.Name = pvm.User.FullName;
                    pvm.Pos = playerService.Pos(pvm.ID);
                    GetPick(pvm);
                    pvm.ID = p.ID;
                }
            return playersVm;
        }

        [HttpPut("[action]")]
        public IActionResult Edit([FromBody] PlayerVm playerVm)
        {
            Player player = playerService.Player(playerVm.ID);
            if (player == null)
                return NoContent();

            Backup backup = backupService.Backup(playerVm.ID);
            if (backup.BoostTotal != playerVm.BoostTotal || backup.HitsTotal != playerVm.HitsTotal)
            {
                backup.BoostTotal = playerVm.BoostTotal;
                backup.HitsTotal = playerVm.HitsTotal;
                backupService.Edit(backup);
            }
            if(backup.HitByID != playerVm.HitByID) backup.HitByID = playerVm.HitByID;

            playerService.Edit(mapper.Map(playerVm, player));
            return Ok(playerVm);
        }

        [HttpDelete("[action]")]
        public IActionResult Delete(int id)
        {
            Player player = playerService.Player(id);

            if (player != null)
                playerService.Delete(player);

            return NoContent();
        }

        [HttpGet("[action]")]
        public bool Exist(int gameId)
        {
            return playerService.Exist(userService.CurrentUser().ID, gameId);
        }

        [HttpGet("[action]")]
        public bool Active(int gameId)
        {
            return playerService.Active(userService.CurrentUser().ID, gameId);
        }

        public void GetPick(PlayerVm playerVm)
        {
            if (playerVm.PickID != 0)
            {
                playerVm.Pick = mapper.Map<PickVm>(pickService.Pick(playerVm.PickID));
                playerVm.Pick.Team = mapper.Map<TeamVm>(teamService.Team(playerVm.Pick.TeamID));
            }
        }

        public ArtVm Art(UserVm userVm)
        {
            var art = artService.Art(userVm.ArtID);
            if (art != null || userVm.ArtID != 0)
            {
                userVm.Art = mapper.Map<ArtVm>(art);
                userVm.Art.Index = artService.Index(userVm.ArtID);
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
    }
}
