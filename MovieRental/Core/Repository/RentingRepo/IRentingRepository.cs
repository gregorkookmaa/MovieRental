using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using MovieRental.Data;
using MovieRental.Models;

namespace MovieRental.Core.Repository.RentingRepo
{
    public interface IRentingRepository : IBaseRepository<Renting>
    {
        IEnumerable Clients { get; set; }
        Task<PagedResult<Renting>> GetPagedList(int page, int pageSize, string searchString = null, string sortOrder = null);

        IEnumerable<Renting> DropDownList();

        Task Delete(int id);
    }
}
