using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pickfc.BLL.Interfaces;
using Pickfc.Model.Entities;
using Pickfc.UI.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using Pickfc.Model.DTOs;

namespace Pickfc.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IArtService artService;
        private readonly IGameService gameService;
        private readonly IPlayerService playerService;
        private readonly IMailService mailService;
        private readonly IMapper mapper;
        private readonly string invalid = "Invalid credentials";

        public UserController(IUserService userService, IArtService artService, IGameService gameService, IPlayerService playerService, IMailService mailService, IMapper mapper)
        {
            this.userService = userService;
            this.artService = artService;
            this.gameService = gameService;
            this.playerService = playerService;
            this.mailService = mailService;
            this.mapper = mapper;
        }

        [HttpGet("[action]")]
        public UserVm User(int id)
        {
            User user = userService.User(id);
            if (user.VerifyTime == null)
                return null;

            UserVm userVm = mapper.Map<UserVm>(user);
            userVm.ID = user.ID;
            CanAdd(userVm);
            Art(userVm);
            return userVm;
        }

        [HttpGet("[action]"), Authorize]
        public UserVm CurrentUser()
        {
            User user = userService.CurrentUser();
            UserVm userVm = mapper.Map<UserVm>(user);
            userVm.ID = user.ID;
            CanAdd(userVm);
            Art(userVm);
            return userVm;
        }

        [HttpGet("[action]")]
        public UserVm ViaCode(string code) { 
            User user = userService.ViaCode(code);
            UserVm userVm = mapper.Map<UserVm>(user);
            userVm.ID = user.ID;
            CanAdd(userVm);
            Art(userVm);
            return userVm;
        }

        [HttpGet("[action]")]
        public UserVm ViaEmail(string email) {
            User user = userService.ViaEmail(email);
            UserVm userVm = mapper.Map<UserVm>(user);
            userVm.ID = user.ID;
            CanAdd(userVm);
            Art(userVm);
            return userVm;
        }

        [HttpGet("[action]")]
        public IEnumerable<UserVm> Users()
        {
            return mapper.Map<IEnumerable<UserVm>>(userService.Users());
        }

        [HttpGet("[action]")]
        public bool Exist(string email)
        {
            return userService.Exist(email);
        }

        [HttpGet("[action]")]
        public bool VerifiedExist(string code) {
            return userService.VerifiedExist(code);
        }

        [HttpPost("[action]")]
        public IActionResult CodeRequest([FromBody] AuthVm authVm)
        {
            if (!userService.Exist(authVm.Email))
                return BadRequest("Email not recognised");

            User user = userService.ViaEmail(authVm.Email);
            user.Code = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
            user.CodeExpires = DateTime.Now.AddDays(1);
            userService.Edit(user);

            mailService.CodeRequest(authVm.Email, user.Code, authVm.ActivationCode);
            return Ok();
        }

        [HttpPost("[action]")]
        public IActionResult Verify([FromBody] AuthVm authVm)
        {

            if (!userService.VerifiedExist(authVm.Code))
                return BadRequest("Invalid code");

            userService.Verify(authVm.Code);
            return Ok();
        }

        [HttpPost("[action]")]
        public bool ValidUser([FromBody] AuthVm authVm)
        {
            var validUser = userService.AuthValid(authVm.Email, authVm.Password);

            if (validUser)
                new Claim("email", authVm.Email);

            return validUser;
        }

        [HttpPost("[action]")]
        public IActionResult Add([FromBody] UserVm userVm)
        {
            if (userVm.Email is null || userVm.Password is null)
                return BadRequest(invalid);

            if (Exist(userVm.Email))
                return BadRequest(invalid);

            User user = mapper.Map<User>(userVm);
            userService.Add(user);
            mailService.CodeRequest(user.Email, user.Code, true);
            userVm.ID = user.ID;
            Art(userVm);

            foreach (Game game in gameService.Legacies())
                playerService.Add(playerService.Set(game, user.ID, false));

            return Ok(userVm);
        }

        [HttpPost("[action]")]
        public IActionResult Login([FromBody] AuthVm authVm)
        {
            if (authVm is null)
                return BadRequest(invalid);

            User user = userService.ViaEmail(authVm.Email);
            if(user == null)
                return NotFound(invalid);

            if (user.VerifyTime == null)
                return BadRequest("Unverified");

            if (user.Code != string.Empty) {
                user.Code = string.Empty;
                user.CodeExpires = null;
                userService.Edit(user);
            }

            int duration = authVm.RememberMe ? 30 : 1;

            List<Claim> authCliams = new List<Claim> {
                new Claim(ClaimTypes.Name, user.FirstName.ToLower() + user.LastName.ToLower()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("id", user.ID.ToString()),
            };

            if (authVm.Password == user.Password)
            {
                return Ok(new UserToken
                {
                    Token = userService.TokenJwt(duration, authCliams),
                    Created = DateTime.Now,
                    Expires = DateTime.Now.AddDays(duration)
                });
            }
            else {
                return BadRequest(invalid);
            }
        }

        [HttpPut("[action]")]
        public IActionResult Edit([FromBody] UserVm userVm)
        {
            User user = userService.User(userVm.ID);
            if (user == null)
                return NotFound();

            if (userVm.ArtID == 0)
                user.ArtID = artService.RandomArtID();

            userService.Edit(mapper.Map(userVm, user));
            Art(userVm);
            return Ok(userVm);
        }

        [HttpDelete("[action]")]
        public IActionResult Delete(int id)
        {
            User user = userService.User(id);

            foreach(var p in playerService.UserPlayers(id))
                playerService.Delete(p);

            if (user.FullAdmin)
                return BadRequest("Pick fc admins cannot be removed");
            else
                userService.Delete(user);

            return NoContent();
        }
        public ArtVm Art(UserVm userVm)
        {
            var art = artService.Art(userVm.ArtID);
            if (art != null || userVm.ArtID != 0)
            {
                userVm.Art = mapper.Map<ArtVm>(art);
                userVm.Art.Index = artService.Index(userVm.ArtID) + 1;
            }
            else {
                userVm.Art = mapper.Map<ArtVm>(artService.Default());
                userVm.Art.FirstName = userVm.FirstName;
                userVm.Art.LastName = userVm.LastName;
                userVm.Art.Index = 0;
            }
            return userVm.Art;
        }

        public void CanAdd(UserVm userVm) {
            userVm.CanAdd = true;
            if (gameService.Creations(userVm.ID) >= 3 && !userVm.FullAdmin)
                userVm.CanAdd = false;
        }
    }
}
