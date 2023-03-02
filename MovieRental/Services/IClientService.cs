using System.Collections.Generic;
using System.Threading.Tasks;
using MovieRental.Data;
using MovieRental.Models;
using MovieRental.Models.ViewModels;

namespace MovieRental.Services
{
    public interface IClientService
    {
        Task<PagedResult<ClientModel>> GetPagedList(int page, int pageSize, string searchString = null, string sortOrder = null);
        Task<ClientModel> GetById(int id);
        Task<ClientEditModel> GetForEdit(int id);
        IEnumerable<Client> DropDownList();
        Task<OperationResponse> Save(ClientEditModel model);
        Task<OperationResponse> Delete(ClientModel model);
    }
}
