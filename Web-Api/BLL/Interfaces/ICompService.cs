using Pickfc.Model.Entities;

namespace Pickfc.BLL.Interfaces
{
    public interface ICompService
    {
        /// <summary>
        /// Gets a single comp via its unique identifier
        /// </summary>
        /// <param name="id">identifier</param>
        /// <returns>Single comp</returns>
        Comp Comp(int id);

        /// <summary>
        /// Gets default assigned Competiton
        /// </summary>
        /// <returns>Default Competiton</returns>
        Comp Default();

        /// <summary>
        /// Gets All comps from db
        /// </summary>
        /// <returns>All comps</returns>
        IEnumerable<Comp> Comps();

        /// <summary>
        /// Creates new comp db entry
        /// </summary>
        /// <param name="comp">Comp to be created</param>
        /// <returns>New comp</returns>
        Comp Add(Comp comp);

        /// <summary>
        /// Updates exsisting comp entry
        /// </summary>
        /// <param name="comp">Comp to be updated</param>
        void Edit (Comp comp);

        /// <summary>
        /// Removes comp entery from db
        /// </summary>
        /// <param name="comp">Comp to be removed</param>
        void Delete (Comp comp);

        /// <summary>
        /// Checks if competition is created by full admin
        /// </summary>
        /// <param name="id">cCompetiton idenitifier</param>
        /// <returns>True if competiton's creator is full admin otherwise false</returns>
        bool Legacy(int id);

        /// <summary>
        /// Checks if a default competiton exsist
        /// </summary>
        /// <returns>Return true if default exsist otherwise false</returns>
        bool DefaultExist();
    }
}
