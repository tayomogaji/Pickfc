using Microsoft.AspNetCore.Mvc;
using Pickfc.BLL.Interfaces;
using Pickfc.Model.Entities;
using Pickfc.UI.ViewModels;

namespace Pickfc.UI.Controllers
{
    [Route("api/[controller]")]
    public class MailController : Controller
    {
        private readonly IMailService mailService;
        private readonly IUserService userService;
        private readonly ICompService compService;
        private readonly IRoundService roundService;
        private readonly IPlayerService playerService;
        private readonly string alreadyNotified = "Users already notified";

        public MailController(IMailService mailService, IUserService userService, ICompService compService, IRoundService roundService, IPlayerService playerService)
        {
            this.mailService = mailService;
            this.userService = userService;
            this.compService = compService;
            this.roundService = roundService;
            this.playerService = playerService;
        }

        [HttpPost("[action]")]
        public IActionResult CodeRequest([FromBody] AuthVm authVm)
        {
            if (!userService.Exist(authVm.Email))
                return BadRequest("Email not recognised");

            User user = userService.ViaEmail(authVm.Email);
            userService.Code(user);
            userService.Edit(user);

            mailService.CodeRequest(authVm.Email, user.Code, authVm.ActivationCode);
            return Ok();
        }

        [HttpPost("[action]")]
        public IActionResult NewContent([FromBody] NotifyVm notifyVm)
        {
            if (notifyVm.Comp)
            {
                Comp comp = compService.Comp(notifyVm.ID);
                if (!comp.OpenNotified)
                {
                    comp.OpenNotified = true;
                    compService.Edit(comp);
                    MailUsers("competition");
                }
                else
                    return BadRequest(alreadyNotified);
            }
            else {
                Round round = roundService.Round(notifyVm.ID);
                if (!round.StartNotified)
                {
                    round.StartNotified = true;
                    roundService.Edit(round);
                    MailUsers("round");
                }
                else return BadRequest(alreadyNotified);
            }
            return Ok();
        }

        [HttpPost("[action]")]
        public IActionResult RoundDeadline([FromBody] NotifyVm notifyVm) { 
           Round round = roundService.Round(notifyVm.ID);
            if (!round.DeadlineNotified)
            {
                round.DeadlineNotified = true;
                roundService.Edit(round);
                MailUsers(string.Empty);
            }
            else
                return BadRequest(alreadyNotified);
            
            return Ok();
        }

        private void MailUsers(string content) 
        {
            var users = userService.Users().Where(x => x.Notify && x.Active);
            if (users == null)
                return;

            foreach (var u in users) 
            {
                var players = playerService.UserPlayers(u.ID).Where(x => x.PickID == 0 && !x.Eliminated);
                if (players == null) return;

                if (players.Count() > 0)
                    if (content == string.Empty) mailService.RoundDeadline(u.Email);
                    else mailService.NewContent(u.Email, content);
            }
        }
    }
}
