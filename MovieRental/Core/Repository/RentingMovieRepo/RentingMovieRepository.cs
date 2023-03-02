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
    public class RentingMovieRepository : BaseRepository<RentingMovie>, IRentingMovieRepository
    {
        private readonly RentingContext _context;

        public RentingMovieRepository(RentingContext context) : base(context)
        {
            _context = context;
        }

        IEnumerable IRentingMovieRepository.Rentings { get; set; }
        IEnumerable IRentingMovieRepository.Movies { get; set; }

        public async Task<PagedResult<RentingMovie>> GetPagedList(int page, int pageSize, string searchString = null, string sortOrder = null)
        {
            IQueryable<RentingMovie> query = _context.RentingMovies
                                                .Include(te => te.Movie)
                                                .Include(te => te.Renting)
                                                .ThenInclude(t => t.Client);

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(te => te.Renting.Client.FirstName.Contains(searchString) ||
                                         te.Renting.Client.LastName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "clientName_asc":
                    query = query.OrderBy(te => (te.Renting.Client.FirstName + " " + te.Renting.Client.LastName))
                                 .ThenByDescending(te => te.Renting.Date);
                    break;
                case "clientName_desc":
                    query = query.OrderByDescending(te => (te.Renting.Client.FirstName + " " + te.Renting.Client.LastName))
                                  .ThenByDescending(te => te.Renting.Date);
                    break;
                case "date_asc":
                    query = query.OrderBy(te => te.Renting.Date);
                    break;
                default:
                    query = query.OrderByDescending(te => te.Renting.Date)
                                 .ThenBy(te => (te.Renting.Client.FirstName + " " + te.Renting.Client.LastName));
                    break;
            }

            return await query.GetPagedAsync(page, pageSize);
        }

        public override async Task<RentingMovie> GetById(int id)
        {
            return await _context.RentingMovies.Include(te => te.Movie)
                                                    .Include(te => te.Renting)
                                                    .ThenInclude(t => t.Client)
                                                    .FirstOrDefaultAsync(te => te.ID == id);
        }

        public async Task Delete(RentingMovie rentingMovie)
        {
            _context.RentingMovies.Remove(rentingMovie);
        }

        public async Task Delete(int id)
        {
            var rentingMovies = await GetById(id);
            await Delete(rentingMovies);
        }

    }
}
