using Pickfc.DAL.Interfaces;
using Pickfc.BLL.Interfaces;
using Pickfc.DAL.Infrastructure;
using Pickfc.Model.Context;
using Pickfc.Model.Entities;

namespace Pickfc.BLL.Services
{
    public class CompService : ICompService
    {
        private readonly ICompRepository compRepository;
        private readonly ICompTeamRepository compTeamRepo;
        private readonly WorkUnit<PickfcContext> workUnit;

        public CompService(ICompRepository compRepository, ICompTeamRepository compTeamRepo, WorkUnit<PickfcContext> workUnit) {
            this.compRepository = compRepository;
            this.compTeamRepo = compTeamRepo;
            this.workUnit = workUnit;
        }

        public Comp Comp(int id) {
            return compRepository.SingleOrDefault(x => x.ID == id);
        }

        public Comp Default() {
            return compRepository.SingleOrDefault(x => x.Default);
        }

        public IEnumerable<Comp> Comps() { 
            return compRepository.GetAll(); 
        }

        public Comp Add(Comp comp) {
            comp.Active = false;
            comp.Timestamped = DateTime.Now;
            compRepository.Add(comp);
            workUnit.Commit();
            return comp;
        }

        public void Edit(Comp comp) {
            compRepository.Update(comp);
            workUnit.Commit();
        }

        public void Delete(Comp comp) { 
            if(comp != null) {
                IEnumerable<CompTeam> compTeams = compTeamRepo.GetMany(x => x.CompID == comp.ID);

                if (compTeams != null)
                    foreach (var ct in compTeams)
                        compTeamRepo.Delete(ct);

                compRepository.Delete(comp);
                workUnit.Commit();
            }
        }

        public bool Legacy(int id) {
            return compRepository.Any(x => x.ID == id && x.Legacy);
        }

        public bool DefaultExist() {
            return compRepository.Any(x => x.Default);
        }
    }
}
