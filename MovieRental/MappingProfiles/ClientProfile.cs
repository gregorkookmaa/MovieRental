using AutoMapper;
using MovieRental.Data;
using MovieRental.Models;
using MovieRental.Models.ViewModels;

namespace MovieRental.MappingProfiles
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<PagedResult<Client>, PagedResult<ClientModel>>();
            CreateMap<Client, ClientModel>();
            CreateMap<Client, ClientEditModel>();

            CreateMap<ClientEditModel, Client>()
              .ForMember(c => c.ID, c => c.Ignore())
              .ForMember(c => c.RentingMovies, c => c.Ignore())
              .ForMember(c => c.Rentings, c => c.Ignore());
        }
    }
}
