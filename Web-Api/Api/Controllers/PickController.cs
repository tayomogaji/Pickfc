using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pickfc.BLL.Interfaces;
using Pickfc.Model.Entities;
using Pickfc.UI.ViewModels;

namespace Pickfc.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PickController : Controller
    {
        private readonly IPickService pickService;
        private readonly ITeamService teamService;
        private readonly IPlayerService playerService;
        private readonly IMapper mapper;

        public PickController(IPickService pickService, ITeamService teamService, IPlayerService playerService, IMapper mapper)
        {
            this.pickService = pickService;
            this.teamService = teamService;
            this.playerService = playerService;
            this.mapper = mapper;
        }

        [HttpGet("[action]")]
        public PickVm Pick(int id)
        {
            Pick pick = pickService.Pick(id);
            PickVm pickVm = mapper.Map<PickVm>(pick);
            pickVm.ID = id;
            pickVm.Team = Team(pick.TeamID);
            return pickVm;
        }

        [HttpGet("[action]")]
        public IEnumerable<PickVm> Picks(int roundid)
        {
            IEnumerable<Pick> pick = pickService.Picks(roundid);
            IEnumerable<PickVm> picksVm = mapper.Map<IEnumerable<PickVm>>(pick);

            foreach (var pvm in picksVm) {
                pvm.Team = Team(pvm.TeamID);
            }
            return picksVm;
        }

        [HttpGet("[action]")]
        public PickVm PlayerPick(int playerid, int gameid, int roundid)
        {
            var pick  = Pick(pickService.PlayerPick(playerid, gameid, roundid).ID);
            pick.Team = Team(pick.TeamID);
            return pick;
        }

        [HttpGet("[action]")]
        public bool PickExist(int playerid, int gameid, int roundid) {
            return pickService.PickExist(playerid, gameid, roundid);
        }

        //check this
        [HttpGet("[action]")]
        public IEnumerable<PickVm> PlayerPicks(int playerid, int gameid)
        {
            IEnumerable<Pick> pick = pickService.PlayerPicks(playerid, gameid);
            IEnumerable<PickVm> picksVm = mapper.Map<IEnumerable<PickVm>>(pick).OrderByDescending(x => x.RoundNumber);

            foreach (var pvm in picksVm) pvm.Team = Team(pvm.TeamID);
            return picksVm;
        }

        [HttpPost("[action]")]
        public IActionResult Add([FromBody] PickVm pickVm)
        {
            if(PickExist(pickVm.PlayerID, pickVm.GameID, pickVm.RoundID)) return BadRequest("Pick already made.");

            Pick pick = pickService.Add(mapper.Map<Pick>(pickVm));
            pickVm.ID = pick.ID;

            Player player = playerService.Player(pickVm.PlayerID);
            player.PickID = pick.ID;
            playerService.Edit(player);

            return Ok(pickVm);
        }

        [HttpPut("[action]")]
        public IActionResult Edit([FromBody] PickVm pickVm)
        {
            pickService.Edit(mapper.Map(pickVm, pickService.Pick(pickVm.ID)));

            return Ok(pickVm);
        }

        [HttpDelete("[action]")]
        public IActionResult Delete(int id)
        {
            pickService.Delete(pickService.Pick(id));
            return NoContent();
        }

        public TeamVm Team(int id)
        {
            return mapper.Map<TeamVm>(teamService.Team(id));
        }
    }
}
