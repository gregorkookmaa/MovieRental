using System.Collections.Generic;
using System.Threading.Tasks;
using MovieRental.Data;
using MovieRental.Models;

namespace MovieRental.Core.Repository.ClientRepo
{
    public interface IClientRepository : IBaseRepository<Client>
    {
        IEnumerable<Client> DropDownList();
        Task<PagedResult<Client>> GetPagedList(int page, int pageSize, string searchString = null, string sortOrder = null);
        Task Delete(int id);
    }
}