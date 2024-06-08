using Microsoft.AspNetCore.Mvc;
using Pickfc.BLL.Interfaces;
using Pickfc.Model.Entities;

namespace Pickfc.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : Controller
    {
        private readonly IArtService artService;
        private readonly ICompService compService;
        private readonly IGameService gameService;
        private readonly ITeamService teamService;
        private readonly IWebHostEnvironment iwhe;
        public string pic = "";

        public UploadController(IArtService artService, ICompService compService, IGameService gameService, ITeamService teamService, IWebHostEnvironment iwhe)
        {
            this.artService = artService;
            this.compService = compService;
            this.gameService = gameService;
            this.teamService = teamService;
            this.iwhe = iwhe;
        }

        public async Task<IActionResult> Upload(string folder)
        {
            var formCollection = await Request.ReadFormAsync();
            var file = formCollection.Files.First();
            if (file == null || file.Length == 0)
                return BadRequest("No file found");
            
            string path = Path.Combine(iwhe.WebRootPath, folder);
            string fileName = $"{DateTime.Now:yyyyMMddHHmmss}.{file.Name}";
            pic = $"{folder}/{fileName}";

            string dbPath = Path.Combine(path, fileName);
            using (var stream = new FileStream(dbPath, FileMode.Create))
                file.CopyTo(stream);
            
            return Ok(new { dbPath });
        }

        [HttpPost("[action]"), DisableRequestSizeLimit]
        public async Task<IActionResult> Art(int id)
        {
            Art art = artService.Art(id);

            if (art != null)
            {
                DeleteExisting(art.Path);
                await Upload("art");
                art.Path = pic;
                artService.Edit(art);
            }
            return NoContent();
        }

        [HttpPost("[action]"), DisableRequestSizeLimit]
        public async Task<IActionResult> Comp(int id)
        {
            Comp comp = compService.Comp(id);

            if (comp != null)
            {
                DeleteExisting(comp.Pic);
                await Upload("comp");
                comp.Pic = pic;
                compService.Edit(comp);

                Game publicGame = gameService.Public(id);
                if (publicGame != null)
                {
                    publicGame.Pic = pic;
                    gameService.Edit(publicGame);
                }
            }
            return NoContent();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Game(int id)
        {
            Game game = gameService.Game(id);

            if (game != null)
            {
                if(game.Pic != compService.Comp(game.CompID).Pic) DeleteExisting(game.Pic);
                await Upload("game");
                game.Pic = pic;
                gameService.Edit(game);
            }
            return NoContent();
        }

        [HttpPost("[action]"), DisableRequestSizeLimit]
        public async Task<IActionResult> Team(int id)
        {
            Team team = teamService.Team(id);

            if (team != null)
            {
                DeleteExisting(team.Pic);
                await Upload("team");
                team.Pic = pic;
                teamService.Edit(team);
            }
            return NoContent();
        }

        public void DeleteExisting(string pic)
        {
            if (pic != string.Empty) {
                string file = Path.Combine(iwhe.WebRootPath, pic);
                if (System.IO.File.Exists(file))
                    System.IO.File.Delete(file);
            }
        }
    }
}
