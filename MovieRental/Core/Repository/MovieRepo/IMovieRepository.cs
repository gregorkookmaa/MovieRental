using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieRental.Data;
using MovieRental.Models;

namespace MovieRental.Core.Repository.MovieRepo
{
    public interface IMovieRepository : IBaseRepository<Movie>
    {
        IEnumerable<Movie> DropDownList();
        Task<PagedResult<Movie>> GetPagedList(int page, int pageSize, string searchString = null, string sortOrder = null);
        Task Delete(int id);
    }
}
