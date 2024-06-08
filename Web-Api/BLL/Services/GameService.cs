using Pickfc.DAL.Interfaces;
using Pickfc.BLL.Interfaces;
using Pickfc.DAL.Infrastructure;
using Pickfc.Model.Context;
using Pickfc.Model.Entities;
using Microsoft.AspNetCore.Hosting;

namespace Pickfc.BLL.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository gameRepository;
        private readonly string placeholder = "comp/_placeholder.png";
        private readonly WorkUnit<PickfcContext> workUnit;

        public GameService(IGameRepository gameRepository, WorkUnit<PickfcContext> workUnit) {
            this.gameRepository = gameRepository;
            this.workUnit = workUnit;
        }

        public Game Game(int id) {
            return gameRepository.SingleOrDefault(x => x.ID == id); ;
        }

        public Game GameViaCode(string code) {
            return gameRepository.SingleOrDefault(x => x.Code == code);
        }

        public Game Public(int compId) {
            return gameRepository.SingleOrDefault(x => x.CompID == compId && x.Public); ;
        }

        public Game PublicSet(Comp comp, int creatorId) {
            Game game = new Game
            {
                CreatorID = creatorId,
                CompID = comp.ID,
                Name = comp.Name,
                Pic = comp.Pic,
                Code = "",
                Public = true,
                Legacy = comp.Legacy,
                Deadline = false,
            };
            return game;
        }

        public void PublicGameMapper(Game pg, Comp c) { 
            pg.Name = c.Name;
            pg.Pic = c.Pic;
            pg.Legacy = c.Legacy;
        }

        public IEnumerable<Game> Publics()
        {
            return gameRepository.GetMany(x => x.Public);
        }

        public IEnumerable<Game> Games()
        {
            return gameRepository.GetAll(); ;
        }

        public IEnumerable<Game> Comps(int compID)
        {
            return gameRepository.GetMany(x => x.CompID == compID);
        }

        public IEnumerable<Game> Legacies() {
            return gameRepository.GetMany(x => x.Legacy); ;
        }

        public IEnumerable<Game> Users(IEnumerable<Player> players)
        {
            List<Game> games = new List<Game>();
            foreach (var p in players)
                games.Add(gameRepository.SingleOrDefault(x => x.ID == p.GameID && p.Active));

            return games;
        }

        public Game Add(Game game) 
        {
            game.Timestamped = DateTime.Now;
            if (!game.Deadline)
                game.DeadlineDate = game.Timestamped.AddYears(100);

            if (!game.Public)
                Code(game);

            gameRepository.Add(game);
            workUnit.Commit();
            return game;
        }

        public void Edit(Game game) 
        {
            if (!game.Deadline)
                game.DeadlineDate = null;

            gameRepository.Update(game);
            workUnit.Commit();
        }

        public void Delete(Game game)
        {
            if (game != null) {
                gameRepository.Delete(game);
                workUnit.Commit();
            }
        }

        public bool ValidCode(string code) 
        {
            return gameRepository.Any(x => x.Code == code);
        }

        public void Code(Game game) {
            Random rand = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            do{
                game.Code = new (Enumerable.Repeat(chars, 6).Select(s => s[rand.Next(s.Length)]).ToArray());
            } 
            while (gameRepository.Any(x => x.Code == game.Code));

            if (game.Public)
                game.Code = string.Empty;
        }

        public int Creations(int uid) { 
            return gameRepository.GetMany(x => x.CreatorID == uid).Count();
        }
    }
}
