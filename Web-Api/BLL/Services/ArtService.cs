using Pickfc.DAL.Interfaces;
using Pickfc.BLL.Interfaces;
using Pickfc.DAL.Infrastructure;
using Pickfc.Model.Context;
using Pickfc.Model.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Pickfc.BLL.Services
{
    public class ArtService : IArtService
    {
        private readonly IArtRepository artRepo;
        //private readonly string placeholder = "/assets/_placeholder.png";
        private readonly WorkUnit<PickfcContext> workUnit;

        public ArtService(IArtRepository artRepo, WorkUnit<PickfcContext> workUnit) {
            this.artRepo = artRepo;
            this.workUnit = workUnit;
        }

        public Art Art(int id)
        {
            return artRepo.SingleOrDefault(x => x.ID == id);
        }

        public Art Default() {
            var art = new Art()
            {
                ID = 0,
                Path = "",
            };
            return art;
        }

        public Art ViaPath(string path) {
            return artRepo.SingleOrDefault(x => x.Path == path);
        }

        public IEnumerable<Art> Arts() {
            return artRepo.GetAll().Where(x => x.ID != 2).OrderBy(x => x.LastName).ThenBy(x => x.FirstName);
        }

        public Art Add(Art art) {
            art.Timestamped = DateTime.Now;
            artRepo.Add(art);
            workUnit.Commit();
            return art;
        }

        public void Edit(Art art) { 
            artRepo.Update(art);
            workUnit.Commit();
        }

        public void Delete(Art art) {
            if (art != null) {
                artRepo.Delete(art);
                workUnit.Commit();
            }
        }

        public int Index(int id) {
         var arts = Arts().ToList();
         return arts.IsNullOrEmpty() ? 0 : arts.FindIndex(x => x.ID == id);
        }

        public int RandomArtID()
        {
            int id;
            var arts = Arts().ToList();
            id = arts[new Random().Next(arts.Count())].ID;
            return id;
        }
    }
}
