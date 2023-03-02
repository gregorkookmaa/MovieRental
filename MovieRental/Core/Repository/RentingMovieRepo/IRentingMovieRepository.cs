using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieRental.Data;
using MovieRental.Models;

namespace MovieRental.Core.Repository.RentingMovieRepo
{
    public interface IRentingMovieRepository : IBaseRepository<RentingMovie>
    {
        IEnumerable Rentings { get; set; }

        IEnumerable Movies { get; set; }

        Task<PagedResult<RentingMovie>> GetPagedList(int page, int pageSize, string searchString = null, string sortOrder = null);

        Task Delete(int id);
    }
}
