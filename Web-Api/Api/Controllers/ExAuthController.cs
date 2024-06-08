using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pickfc.BLL.Interfaces;
using Pickfc.BLL.Services;
using Pickfc.Model.Entities;
using Pickfc.UI.ViewModels;
using static Org.BouncyCastle.Math.EC.ECCurve;
using System.Net.Http;

namespace Pickfc.UI.Controllers
{
    [Route("api/[controller]")]
    public class ExAuthController : Controller
    {
        private readonly IUserService userService;
        private readonly IConfiguration config;
        private readonly HttpClient http;

        public ExAuthController(IUserService userService, IConfiguration config, HttpClient http)
        {
            this.userService = userService;
            this.config = config;
            this.http = http;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Facebook([FromBody] string credential)
        {
            //string metaApi = "https://graph.facebook.com/{your-user-id}/accounts?access_token={user-access-token}";
            HttpResponseMessage tokenResponce = await http.GetAsync("https://graph.facebook.com/debug_token?input_token=" + credential + $"&access_token={config.GetSection("Facebook:ID")}|{config.GetSection("Facebook:Secret")}");
            var tokenRes = await tokenResponce.Content.ReadAsStringAsync();
            var userFb = JsonConvert.DeserializeObject<UserFB>(tokenRes);
            if (userFb == null)
                return BadRequest("User not found");
            if (!userFb.Data.IsValid)
                return Unauthorized();

            HttpResponseMessage meResponse = await http.GetAsync("https://graph.facebook.com/me/?fields=first_name,last_name,email,id&access_token=" + credential);

            var userContent = await meResponse.Content.ReadAsStringAsync();
            var userContentObj = JsonConvert.DeserializeObject<UserSocial>(userContent);

            if (userContentObj == null)
                return BadRequest("User not found");
            string email = userContentObj.Email;

            if (userService.Exist(email))
            {
                User user = userService.ViaEmail(email);
                AuthVm authVm = new AuthVm
                {
                    Email = email,
                    Password = user.Password,
                    RememberMe = true,
                };
                //Login(authVm);
            }
            else
            {
                UserVm userVm = new UserVm
                {
                    Email = email,
                    FirstName = userContentObj.FirstName,
                    LastName = userContentObj.LastName,
                    FullName = userContentObj.FirstName + " " + userContentObj.LastName,
                };
                //Add(userVm);
            }
            return Ok();
        }
    }
}
