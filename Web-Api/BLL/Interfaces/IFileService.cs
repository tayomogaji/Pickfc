//using MailKit;
//using Microsoft.AspNetCore.Http;
//using Pickfc.Model.Entities;

//namespace Pickfc.BLL.Interfaces
//{
//    public interface IFileService
//    {
//        /// <summary>
//        /// Retrieves file from Azure storage account
//        /// </summary>
//        /// <param name="container">Where file is located</param>
//        /// <param name="placeholder">File if null or empty</param>
//        /// <param name="name">Of file to retrieve</param>
//        /// <returns></returns>
//        string GetFile(string container, string placeholder, string name);

//        /// <summary>
//        /// Uploads file to Azure storage account
//        /// </summary>
//        /// <param name="blob">File to upload</param>
//        /// <param name="container">Where file is located</param>
//        Task<string> Upload(IFormFile blob, string container);

//        /// <summary>
//        /// Deletes file from Azure storage account
//        /// </summary>
//        /// <param name="container">Where file is located</param>
//        /// <param name="name">Of file to delete</param>
//        void Delete(string container, string name);
//    }
//}
