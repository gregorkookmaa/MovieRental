using System.Threading.Tasks;
using MovieRental.Data;
using MovieRental.Models.ViewModels;

namespace MovieRental.Services
{
    public interface IRentingMovieService
    {
        Task<PagedResult<RentingMovieModel>> GetPagedList(int page, int pageSize, string searchString = null, string sortOrder = null);
        Task<RentingMovieModel> GetById(int id);
        Task<RentingMovieEditModel> GetForEdit(int id);
        Task<OperationResponse> Save(RentingMovieEditModel model);
        Task<OperationResponse> Delete(RentingMovieModel model);
        Task FillEditModel(RentingMovieEditModel model);

    }
}
