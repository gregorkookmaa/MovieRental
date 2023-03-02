using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;
using MovieRental.Core.IConfiguration;
using MovieRental.Core.Repository.MovieRepo;
using MovieRental.Core.Repository.RentingMovieRepo;
using MovieRental.Core.Repository.RentingRepo;
using MovieRental.Data;
using MovieRental.Models;
using MovieRental.Models.ViewModels;

namespace MovieRental.Services
{
    public class RentingMovieService : IRentingMovieService
    {
        private readonly IMapper _objectMapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRentingMovieRepository _rentingMovieRepository;
        private readonly IRentingRepository _rentingRepository;
        private readonly IMovieRepository _movieRepository;

        public RentingMovieService(IUnitOfWork unitOfWork, IMapper objectMapper)
        {
            _objectMapper = objectMapper;
            _unitOfWork = unitOfWork;
            _rentingMovieRepository = unitOfWork.RentingMovieRepository;
            _rentingRepository = unitOfWork.RentingRepository;
            _movieRepository = unitOfWork.MovieRepository;
        }

        public async Task<PagedResult<RentingMovieModel>> GetPagedList(int page, int pageSize, string searchString = null, string sortOrder = null)
        {
            var rentingMovies = await _rentingMovieRepository.GetPagedList(page, pageSize, searchString, sortOrder);

            return _objectMapper.Map<PagedResult<RentingMovieModel>>(rentingMovies);
        }

        public async Task<RentingMovieModel> GetById(int id)
         {
            var rentingMovie = await _rentingMovieRepository.GetById(id);

            if (rentingMovie == null)
            {
                return null;
            }

            return _objectMapper.Map<RentingMovieModel>(rentingMovie);
        }

        public async Task<RentingMovieEditModel> GetForEdit(int id)
        {
            var rentingMovie = await _rentingMovieRepository.GetById(id);
            if (rentingMovie == null)
            {
                return null;
            }

            var model = _objectMapper.Map<RentingMovieEditModel>(rentingMovie);

            await FillEditModel(model);

            return model;
        }

        public async Task<OperationResponse> Save(RentingMovieEditModel model)
        {
            var response = new OperationResponse();

            if (model == null)
            {
                return response.AddError("", "Model was null");
            }

            var rentingMovie = new RentingMovie();

            if (model.ID != 0)
            {
                rentingMovie = await _rentingMovieRepository.GetById(model.ID);
                if (rentingMovie == null)
                {
                    return response.AddError("", "Cannot find Renting Movie with id " + model.ID);
                }
            }

            _objectMapper.Map(model, rentingMovie);

            rentingMovie.Renting = await _rentingRepository.GetById(model.RentingID);
            if (rentingMovie.Renting == null)
            {
                response.AddError("RentingID", "Cannot find Renting with id " + model.ID);
            }

            rentingMovie.Movie = await _movieRepository.GetById(model.MovieID);
            if (rentingMovie.Movie == null)
            {
                response.AddError("MovieID", "Cannot find Movie with id " + model.ID);
            }

            if (!response.Success)
            {
                return response;
            }

            await _rentingMovieRepository.Save(rentingMovie);
            await _unitOfWork.CommitAsync();

            return response;
        }

        public async Task<OperationResponse> Delete(RentingMovieModel model)
        {

            var response = new OperationResponse();
            if (model == null)
            {
                return response.AddError("", "Model was null");
            }

            var rentingMovie = await _rentingMovieRepository.GetById(model.ID);
            if (rentingMovie == null)
            {
                return response.AddError("", "Cannot find Renting Movie with id " + model.ID);
            }
            await _rentingMovieRepository.Delete(model.ID);
            await _unitOfWork.CommitAsync();

            return response;
        }

        public async Task FillEditModel(RentingMovieEditModel model)
        {
            var rentings = await _rentingRepository.GetPagedList(1, 100);
            var movies = await _movieRepository.GetPagedList(1, 100);

            model.Rentings = rentings.Results
                                   .OrderByDescending(t => t.Date)
                                   .Select(t => new SelectListItem
                                   {
                                       Text = t.Client.FullName.ToString() + " " + t.Date.ToShortDateString(),
                                       Value = t.ID.ToString(),
                                       Selected = model.RentingID == t.ID
                                   })
                                  .ToList();
            model.Movies = movies.Results
                                  .OrderBy(e => e.Title)
                                  .Select(e => new SelectListItem
                                  {
                                      Text = e.Title,
                                      Value = e.ID.ToString(),
                                      Selected = model.MovieID == e.ID
                                  })
                                 .ToList();
        }
    }
}
