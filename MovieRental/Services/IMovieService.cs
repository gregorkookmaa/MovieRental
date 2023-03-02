using System.Collections.Generic;
using System.Threading.Tasks;
using MovieRental.Data;
using MovieRental.Models;
using MovieRental.Models.ViewModels;

namespace MovieRental.Services
{
    public interface IMovieService
    {
        IEnumerable<Movie> DropDownList();
        Task<MoviesModel> GetById(int id);
        Task<MoviesEditModel> GetForEdit(int id);
        Task<PagedResult<MoviesModel>> GetPagedList(int page, int pageSize, string searchString = null, string sortOrder = null);
        Task<OperationResponse> Save(MoviesEditModel model);
        Task<OperationResponse> Delete(MoviesModel model);
    }
}
