using System.Collections.Generic;
using System.Threading.Tasks;
using MovieRental.Data;
using MovieRental.Models;
using MovieRental.Models.ViewModels;

namespace MovieRental.Services
{
    public interface IRentingService
    {
        Task<PagedResult<RentingModel>> GetPagedList(int page, int pageSize, string searchString = null, string sortOrder = null);
        IEnumerable<Renting> DropDownList();
        Task<RentingModel> GetById(int id);
        Task<RentingEditModel> GetForEdit(int id);
        Task<OperationResponse> Save(RentingEditModel model);
        Task<OperationResponse> Delete(RentingModel model);
        Task FillEditModel(RentingEditModel model);
    }
}
