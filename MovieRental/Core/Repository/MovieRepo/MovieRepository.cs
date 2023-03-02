using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieRental.Data;
using MovieRental.Models;

namespace MovieRental.Core.Repository.MovieRepo
{
    public class MovieRepository : BaseRepository<Movie>, IMovieRepository
    {
        private readonly RentingContext _context;

        public MovieRepository(RentingContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Movie> DropDownList()
        {
            return _context.Movies;
        }

        public override async Task<Movie> GetById(int id)
        {
            return await _context.Movies
                                 .FirstOrDefaultAsync(e => e.ID == id);
        }

        public async Task<PagedResult<Movie>> GetPagedList(int page, int pageSize, string searchString = null, string sortOrder = null)
        {
            IQueryable<Movie> query = _context.Movies;

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(e => e.Title.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    query = query.OrderByDescending(e => e.Title);
                    break;
                case "date_asc":
                    query = query.OrderBy(t => t.ReleaseDate);
                    break;
                case "date_desc":
                    query= query.OrderByDescending(t => t.ReleaseDate);
                    break;
                case "genre_asc":
                    query = query.OrderBy(e => e.MovieGenre);
                    break;
                case "genre_desc":
                    query = query.OrderByDescending(e => e.MovieGenre);
                    break;
                default:
                    query = query.OrderBy(e => e.Title);
                    break;
            }

            return await query.GetPagedAsync(page, pageSize);
        }

        public async Task Save(Movie movie)
        {
            if (movie.ID == 0)
            {
                await _context.Movies.AddAsync(movie);
            }
            else
            {
                _context.Movies.Update(movie);
            }
        }

        public async Task Delete(Movie movie)
        {
            _context.Movies.Remove(movie);
        }

        public async Task Delete(int id)
        {
            var movie = await GetById(id);
            await Delete(movie);
        }
    }
}