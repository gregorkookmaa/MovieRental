using AutoMapper;
using MovieRental.Data;
using MovieRental.Models;
using MovieRental.Models.ViewModels;

namespace MovieRental.MappingProfiles
{
    public class RentingMovieProfile : Profile
    {
        public RentingMovieProfile()
        {
            CreateMap<PagedResult<RentingMovie>, PagedResult<RentingMovieModel>>();
            CreateMap<RentingMovie, RentingMovieModel>()
                .ForMember(tem => tem.MovieName, tem => tem.MapFrom(te => te.Movie.Title))
                .ForMember(tem => tem.ClientName, tem => tem.MapFrom(te => te.Renting.Client.FullName))
                .ForMember(tem => tem.RentingDate, tem => tem.MapFrom(te => te.Renting.Date))
                .ForMember(tem => tem.ExpiresDate, tem => tem.MapFrom(te => te.Renting.Expires));
            CreateMap<RentingMovie, RentingMovieEditModel>();

            CreateMap<RentingMovieEditModel, RentingMovie>()
              .ForMember(te => te.ID, te => te.Ignore())
              .ForMember(te => te.Renting, te => te.Ignore())
              .ForMember(te => te.Movie, te => te.Ignore());
        }
    }
}
