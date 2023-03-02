using AutoMapper;
using MovieRental.Data;
using MovieRental.Models;
using MovieRental.Models.ViewModels;

namespace MovieRental.MappingProfiles
{
    public class RentalProfile : Profile
    {
        public RentalProfile()
        {
            CreateMap<PagedResult<Renting>, PagedResult<RentingModel>>();
            CreateMap<Renting, RentingModel>()
                .ForMember(tm => tm.ClientName, tm => tm.MapFrom(t => t.Client.FullName));
            CreateMap<Renting, RentingEditModel>();

            CreateMap<RentingEditModel, Renting>()
              .ForMember(tm => tm.ID, tm => tm.Ignore())
              .ForMember(tm => tm.Client, tm => tm.Ignore())
              .ForMember(tm => tm.RentingMovies, tm => tm.Ignore());
        }
    }
}
