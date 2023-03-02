using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieRental.Core.IConfiguration;
using MovieRental.Core.Repository.ClientRepo;
using MovieRental.Core.Repository.RentingRepo;
using MovieRental.Data;
using MovieRental.Models;
using MovieRental.Models.ViewModels;

namespace MovieRental.Services
{
    public class RentingService : IRentingService
    {
        private readonly IMapper _objectMapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRentingRepository _rentingRepository;
        private readonly IClientRepository _clientRepository;

        public RentingService(IUnitOfWork unitOfWork, IMapper objectMapper)
        {
            _objectMapper = objectMapper;
            _unitOfWork = unitOfWork;
            _rentingRepository = unitOfWork.RentingRepository;
            _clientRepository = unitOfWork.ClientRepository;
        }

        public IEnumerable<Renting> DropDownList()
        {
            return _rentingRepository.DropDownList();
        }

        public async Task<RentingModel> GetById(int id)
        {
            var renting = await _rentingRepository.GetById(id);

            if (renting == null)
            {
                return null;
            }

            return _objectMapper.Map<RentingModel>(renting);
        }

        public async Task<RentingEditModel> GetForEdit(int id)
        {
            var renting = await _rentingRepository.GetById(id);
            if (renting == null)
            {
                return null;
            }

            var model = _objectMapper.Map<RentingEditModel>(renting);

            await FillEditModel(model);

            return model;
        }

        public async Task<PagedResult<RentingModel>> GetPagedList(int page, int pageSize, string searchString = null, string sortOrder = null)
        {
            var rentings = await _rentingRepository.GetPagedList(page, pageSize, searchString, sortOrder);

            return _objectMapper.Map<PagedResult<RentingModel>>(rentings);
        }

        public async Task<OperationResponse> Save(RentingEditModel model)
        {
            var response = new OperationResponse();

            if (model == null)
            {
                return response.AddError("", "Model was null");
            }

            var renting = new Renting();

            if (model.ID != 0)
            {
                renting = await _rentingRepository.GetById(model.ID);
                if (renting == null)
                {
                    return response.AddError("", "Cannot find Renting with id " + model.ID);
                }
            }

            _objectMapper.Map(model, renting);

            renting.Client = await _clientRepository.GetById(model.ClientID);
            if (renting.Client == null)
            {
                response.AddError("ClientID", "Cannot find client with id " + model.ID);
            }

            if (!response.Success)
            {
                return response;
            }

            await _rentingRepository.Save(renting);
            await _unitOfWork.CommitAsync();

            return response;
        }

        public async Task<OperationResponse> Delete(RentingModel model)
        {

            var response = new OperationResponse();
            if (model == null)
            {
                return response.AddError("", "Model was null");
            }

            var renting = await _rentingRepository.GetById(model.ID);
            if (renting == null)
            {
                return response.AddError("", "Cannot find Renting with id " + model.ID);
            }
            await _rentingRepository.Delete(model.ID);
            await _unitOfWork.CommitAsync();

            return response;
        }

        public async Task FillEditModel(RentingEditModel model)
        {
            var clients = await _clientRepository.GetPagedList(1, 100);

            model.Clients = clients.Results
                                   .OrderBy(c => c.FullName)
                                   .Select(c => new SelectListItem
                                   {
                                      Text = c.FullName,
                                      Value = c.ID.ToString(),
                                      Selected = model.ClientID == c.ID
                                   })
                                  .ToList();
        }
    }
}
