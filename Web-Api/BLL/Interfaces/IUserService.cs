using Pickfc.Model.Entities;
using System.Security.Claims;

namespace Pickfc.BLL.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Gets user by unique identifyer
        /// </summary>
        /// <param name="id">User's identifier</param>
        /// <returns>User data of assigned identifyer</returns>
        User User(int id);

        /// <summary>
        /// Gets user by email address
        /// </summary>
        /// <param name="email">User's email</param>
        /// <returns>User data of assigned email address</returns>
        User ViaEmail(string email);

        /// <summary>
        /// Gets user by verification code
        /// </summary>
        /// <param name="code">Users verification code</param>
        /// <returns>User data of assigned verification</returns>
        User ViaCode(string code);

        /// <summary>
        /// Gets data of current logged in user
        /// </summary>
        /// <returns>logged in user data</returns>
        User CurrentUser();

        /// <summary>
        /// Gets all current users in db
        /// </summary>
        /// <returns>All users in db</returns>
        IEnumerable<User> Users();

        /// <summary>
        /// Creates new user and adds it to db
        /// </summary>
        /// <param name="user">User to add</param>
        /// <returns>Newley created user in db</returns>
        User Add(User user);

        /// <summary>
        /// Updates user entery data from db
        /// </summary>
        /// <param name="user">User's entery to be updates</param>
        void Edit(User user);

        /// <summary>
        /// Verifies account by matching users code 
        /// </summary>
        /// <param name="code">User's code to verify</param>
        void Verify(string code);

        /// <summary>
        /// Removes user from db
        /// </summary>
        /// <param name="user">User to be removed</param>
        void Delete(User user);

        /// <summary>
        /// Checks of given email is assigned to a user 
        /// </summary>
        /// <param name="email">email to check</param>
        /// <returns>True if email is assigned to user</returns>
        bool Exist(string email);

        /// <summary>
        /// Checks if given code is assigned to a user
        /// </summary>
        /// <param name="code">Code to check</param>
        /// <returns>True if code is assigned to user</returns>
        bool VerifiedExist(string code);

        /// <summary>
        /// Checks users email and password for user validity when authenticating
        /// </summary>
        /// <param name="email">User's email</param>
        /// <param name="password">User's password</param>
        /// <returns>True if both email and password match the enteries in users db</returns>
        bool AuthValid(string email, string password);

        /// <summary>
        /// Generates a jwt string for user authentication
        /// </summary>
        /// <param name="duration">Length of time token is valid for</param>
        /// <param name="authClaims">List of user's identifying claims</param>
        /// <returns>Token identifier</returns>
        string TokenJwt(int duration, List<Claim> authClaims);

        /// <summary>
        /// generates a random code for the user to authenticate with
        /// </summary>
        /// <param name="user">User's code to update</param>
        void Code(User user);
    }
}
