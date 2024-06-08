using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pickfc.BLL.Interfaces;
using Pickfc.BLL.Services;
using Pickfc.Model.Entities;
using Pickfc.UI.ViewModels;

namespace Pickfc.UI.Controllers
{
    [Route("api/[controller]")]
    public class ArtController : Controller
    {
        private readonly IArtService artService;
        private readonly IUserService userService;
        private readonly IWebHostEnvironment iwhe;
        private readonly IMapper mapper;

        public ArtController(IArtService artService, IUserService userService, IWebHostEnvironment iwhe, IMapper mapper)
        {
            this.artService = artService;
            this.userService = userService;
            this.iwhe = iwhe;
            this.mapper = mapper;
        }

        [HttpGet("[action]")]
        public ArtVm Art(int id) {
            var art = mapper.Map<ArtVm>(artService.Art(id)); 
            FinalView(art);
            return art;
        }

        [HttpGet("[action]")]
        public IEnumerable<ArtVm> Arts() {
            var arts = mapper.Map<IEnumerable<ArtVm>>(artService.Arts());
            foreach(var a in arts)
                FinalView(a);
            return arts;
        }

        [HttpPost("[action]")]
        public IActionResult Add([FromBody] ArtVm artVm) { 
            Art art = artService.Add(mapper.Map<Art>(artVm));
            artVm.ID = art.ID;
            FinalView(artVm);
            return Ok(artVm);
        }

        [HttpPut("[action]")]
        public IActionResult Edit([FromBody] ArtVm artVm)
        {
            Art art = artService.Art(artVm.ID);
            if (art == null)
                return NotFound();

            artService.Edit(mapper.Map(artVm, art));
            FinalView(artVm);
            return Ok(artVm);
        }

        [HttpDelete("[action]")]
        public IActionResult Delete(int id) {
            Art art = artService.Art(id);
            foreach (var u in userService.Users().Where(x => x.ArtID == id)) {
                u.ArtID = 0;
                userService.Edit(u);
            }
            if (art.Path != string.Empty)
            {
                string file = Path.Combine(iwhe.WebRootPath, art.Path);
                if (System.IO.File.Exists(file))
                    System.IO.File.Delete(file);
            }
            artService.Delete(art);
            return Ok(art);
        }

        public void FinalView(ArtVm art) {
            art.Index = artService.Index(art.ID) + 1;
            art.FullName = art.FirstName.ToLower() + " " + art.LastName.ToLower();
        }
    }
}
