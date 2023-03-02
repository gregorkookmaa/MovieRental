using AutoMapper;
using MovieRental.Data;
using MovieRental.Models;
using MovieRental.Models.ViewModels;

namespace MovieRental.MappingProfiles
{
    public class MoviesProfile : Profile
    {
        public MoviesProfile()
        {
            CreateMap<PagedResult<Movie>, PagedResult<MoviesModel>>();
            CreateMap<Movie, MoviesModel>();
            CreateMap<Movie, MoviesEditModel>();

            CreateMap<MoviesEditModel, Movie>()
              .ForMember(e => e.ID, e => e.Ignore())
              .ForMember(e => e.RentingMovies, e => e.Ignore());
        }
    }
}
