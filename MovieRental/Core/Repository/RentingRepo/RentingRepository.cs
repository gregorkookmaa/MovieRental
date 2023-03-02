using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieRental.Data;
using MovieRental.Models;

namespace MovieRental.Core.Repository.RentingRepo
{
    public class RentingRepository : BaseRepository<Renting>, IRentingRepository
    {
        private readonly RentingContext _context;

        public RentingRepository(RentingContext context) : base(context)
        {
            _context = context;
        }

        IEnumerable IRentingRepository.Clients { get ; set; }

        public async Task<PagedResult<Renting>> GetPagedList(int page, int pageSize, string searchString = null, string sortOrder = null)
        {
            IQueryable<Renting> query = _context.Rentings.Include(t => t.Client)
                                          .Include(t => t.RentingMovies)
                                          .ThenInclude(te => te.Movie);

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(t => t.Client.FirstName.Contains(searchString) ||
                                         t.Client.LastName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "date_asc":
                    query = query.OrderBy(t => t.Date);
                    break;
                case "fullName_asc":
                    query = query.OrderBy(t => (t.Client.FirstName + " " + t.Client.LastName));
                    break;
                case "fullName_desc":
                    query = query.OrderByDescending(t => (t.Client.FirstName + " " + t.Client.LastName));
                    break;
                default:
                    query = query.OrderByDescending(t => t.Date);
                    break;
            }

            return await query.GetPagedAsync(page, pageSize);
        }

        public IEnumerable<Renting> DropDownList()
        {
            return _context.Rentings;
        }

        public override async Task<Renting> GetById(int id)
        {
            return await _context.Rentings.Include(t => t.Client)
                                           .Include(t => t.RentingMovies)
                                           .ThenInclude(te => te.Movie)
                                           .Where(t => t.ID == id)
                                           .OrderBy(t => t.Date)
                                           .FirstOrDefaultAsync();
        }

        public async Task Delete(Renting renting)
        {
            _context.Rentings.Remove(renting);
        }

        public async Task Delete(int id)
        {
            var renting = await GetById(id);
            await Delete(renting);
        }
    }
}