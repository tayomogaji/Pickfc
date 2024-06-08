using Pickfc.DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Pickfc.BLL.Interfaces;
using Pickfc.DAL.Infrastructure;
using Pickfc.Model.Context;
using Pickfc.Model.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Pickfc.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IConfiguration configuration;
        private readonly IUserRepository userRepository;
        private readonly WorkUnit<PickfcContext> workUnit;

        public UserService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IUserRepository userRepository, WorkUnit<PickfcContext> workUnit) {
            this.httpContextAccessor = httpContextAccessor;
            this.configuration = configuration;
            this.userRepository = userRepository;
            this.workUnit = workUnit;
        }

        public User User(int id)
        {
            return userRepository.SingleOrDefault(x => x.ID == id);
        }

        public User CurrentUser()
        {
            return userRepository.SingleOrDefault(x => x.ID == (Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirst("id").Value)));
        }

        public User ViaEmail(string email)
        {
            return userRepository.SingleOrDefault(x => x.Email == email);
        }

        public User ViaCode(string code) {
            return userRepository.SingleOrDefault(x => x.Code == code);
        }

        public IEnumerable<User> Users() 
        {
            return userRepository.GetAll();
        }

        public User Add(User user) 
        {
            user.Timestamped = DateTime.Now;
            user.Active = true;
            user.Notify = true;
            Code(user);
            userRepository.Add(user);
            workUnit.Commit();
            return user;
        }
        public void Edit(User user) 
        {
            userRepository.Update(user);
            workUnit.Commit();
        }

        public void Delete(User user)
        {
            if (user != null) {
                userRepository.Delete(user);
                workUnit.Commit();
            }
        }

        public bool Exist(string email) 
        {
            return userRepository.Any(x => x.Email == email);
        }
        
        public bool AuthValid(string email, string password) 
        {
            return userRepository.Any(x => x.Email == email && x.Password.Equals(password));
        }

        public string TokenJwt (int duration, List<Claim>authClaims) {

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AppSettings:Secret"]));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["AppSettings:URL"],
                audience: configuration["AppSettings:URL"],
                claims: authClaims,
                expires: DateTime.Now.AddDays(duration),
                signingCredentials: signinCredentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public void Verify(string code) {
            User user = userRepository.SingleOrDefault(x => x.Code == code);
            if (user != null && user.VerifyTime == null)
            {
                user.VerifyTime = DateTime.Now;
                Edit(user);
            }
        }

        public bool VerifiedExist(string code) {
            return userRepository.Any(x => x.Code == code);
        }

        public void Code(User user)
        {
            Random rand = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            do
                user.Code = new(Enumerable.Repeat(chars, 6).Select(s => s[rand.Next(s.Length)]).ToArray());
            while (userRepository.Any(x => x.Code == user.Code));

            if(user.VerifyTime != null)
                user.CodeExpires = DateTime.Now.AddDays(1);
        }

    }
}
