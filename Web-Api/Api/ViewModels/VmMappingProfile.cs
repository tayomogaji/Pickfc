using AutoMapper;
using Pickfc.Model.Entities;

namespace Pickfc.UI.ViewModels
{
    public class VmMappingProfile : Profile
    {
        public VmMappingProfile() 
        {
            CreateMap<User, UserVm>();
            CreateMap<UserVm, User>();

            CreateMap<Comp, CompVm>();
            CreateMap<CompVm, Comp>();

            CreateMap<Team, TeamVm>();
            CreateMap<TeamVm, Team>();

            CreateMap<Game, GameVm>();
            CreateMap<GameVm, Game>();

            CreateMap<Player, PlayerVm>();
            CreateMap<PlayerVm, Player>();

            CreateMap<Round, RoundVm>();
            CreateMap<RoundVm, Round>();

            CreateMap<Fixture, FixtureVm>();
            CreateMap<FixtureVm, Fixture>();

            CreateMap<Pick, PickVm>();
            CreateMap<PickVm, Pick>();
            
            CreateMap<Art, ArtVm>();
            CreateMap<ArtVm, Art>();
        }
    }
}
