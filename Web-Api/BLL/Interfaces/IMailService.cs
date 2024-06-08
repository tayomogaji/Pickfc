namespace Pickfc.BLL.Interfaces
{
    public interface IMailService
    {
        /// <summary>
        /// Email's a code to user for account verification
        /// </summary>
        /// <param name="email">User's email</param>
        /// <param name="code">Users verification code</param>
        /// <param name="activationCode">Request to reset pw or activate account?</param>
        void CodeRequest(string email, string code, bool activationCode);

        /// <summary>
        /// Sends round deadline email reminder to user
        /// </summary>
        /// <param name="email">Email address of recipient</param>
        void RoundDeadline(string email);

        /// <summary>
        /// Sends notification of new game content 
        /// </summary>
        /// <param name="email">Email address of recipient</param>
        /// <param name="content">Higlighted item to notify</param>
        void NewContent(string email, string content);

        /// <summary>
        /// Sends password reset email to user 
        /// </summary>
        /// <param name="email">Users email</param>
        /// <param name="code">Users verification code</param>
    }
}
