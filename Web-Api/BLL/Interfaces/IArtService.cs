using Pickfc.Model.Entities;

namespace Pickfc.BLL.Interfaces
{
    public interface IArtService
    {
        /// <summary>
        /// Gets single art entity by id
        /// </summary>
        /// <param name="id">Art identifier</param>
        /// <returns>Single art entity</returns>
        Art Art(int id);

        /// <summary>
        /// Creates default art model
        /// </summary>
        /// <returns>Defualt art model</returns>
        Art Default();

        /// <summary>
        /// Gets single art entity by path directory
        /// </summary>
        /// <param name="path">Directory of art</param>
        /// <returns>Single art entity</returns>
        Art ViaPath(string path);

        /// <summary>
        /// Gets all art entities from Db
        /// </summary>
        /// <returns>All art entities</returns>
        IEnumerable<Art> Arts();

        /// <summary>
        /// Adds single art entity to Db
        /// </summary>
        /// <param name="art">Entity to add</param>
        /// <returns>New art entity</returns>
        Art Add(Art art);

        /// <summary>
        /// Updates single art entity in Db
        /// </summary>
        /// <param name="art">Entity to update</param>
        void Edit(Art art);

        /// <summary>
        /// Removes single art entity from Db
        /// </summary>
        /// <param name="art">Entity to remove</param>
        void Delete(Art art);

        /// <summary>
        /// Gets numbered position of art in list
        /// </summary>
        /// <param name="id">Art identifier</param>
        /// <returns>Numbered position of art in list</returns>
        int Index(int id);

        /// <summary>
        /// Gets random ID from art db
        /// </summary>
        /// <returns>Random art ID</returns>
        int RandomArtID();
    }
}
