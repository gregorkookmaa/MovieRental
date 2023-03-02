using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using MovieRental.Core.IConfiguration;
using MovieRental.Core.Repository.MovieRepo;
using MovieRental.Data;
using MovieRental.Models;
using MovieRental.Models.ViewModels;

namespace MovieRental.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMapper _objectMapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMovieRepository _movieRepository;

        public MovieService(IUnitOfWork unitOfWork, IMapper objectMapper)
        {
            _objectMapper = objectMapper;
            _unitOfWork = unitOfWork;
            _movieRepository = unitOfWork.MovieRepository;
        }

        public async Task<PagedResult<MoviesModel>> GetPagedList(int page, int pageSize, string searchString = null, string sortOrder = null)
        {
            var movies = await _movieRepository.GetPagedList(page, pageSize, searchString, sortOrder);

            return _objectMapper.Map<PagedResult<MoviesModel>>(movies);
        }

        public async Task<MoviesModel> GetById(int id)
        {
            var movie = await _movieRepository.GetById(id);

            if (movie == null)
            {
                return null;
            }

            return _objectMapper.Map<MoviesModel>(movie);
        }

        public async Task<MoviesEditModel> GetForEdit(int id)
        {
            var movie = await _movieRepository.GetById(id);
            if (movie == null)
            {
                return null;
            }

            var model = _objectMapper.Map<MoviesEditModel>(movie);

            return model;
        }

        public IEnumerable<Movie> DropDownList()
        {
            return _movieRepository.DropDownList();
        }

        public async Task<OperationResponse> Save(MoviesEditModel model)
        {
            var response = new OperationResponse();

            if (model == null)
            {
                return response.AddError("", "Model was null");
            }

            var movie = new Movie();

            if (model.ID != 0)
            {
                movie = await _movieRepository.GetById(model.ID);
                if (movie == null)
                {
                    return response.AddError("", "Cannot find Movie with id " + model.ID);
                }
            }

            _objectMapper.Map(model, movie);

            if (!response.Success)
            {
                return response;
            }

            await _movieRepository.Save(movie);
            await _unitOfWork.CommitAsync();

            return response;
        }

        public async Task<OperationResponse> Delete(MoviesModel model)
        {

            var response = new OperationResponse();
            if (model == null)
            {
                return response.AddError("", "Model was null");
            }

            var Movie = await _movieRepository.GetById(model.ID);
            if (Movie == null)
            {
                return response.AddError("", "Cannot find Movie with id " + model.ID);
            }
            await _movieRepository.Delete(model.ID);
            await _unitOfWork.CommitAsync();

            return response;
        }
    }
}
