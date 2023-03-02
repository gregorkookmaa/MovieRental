using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using MovieRental.Core.IConfiguration;
using MovieRental.Core.Repository.ClientRepo;
using MovieRental.Data;
using MovieRental.Models;
using MovieRental.Models.ViewModels;

namespace MovieRental.Services
{
    public class ClientService : IClientService
    {
        private readonly IMapper _objectMapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClientRepository _clientRepository;

        public ClientService(IUnitOfWork unitOfWork, IMapper objectMapper)
        {
            _objectMapper = objectMapper;
            _unitOfWork = unitOfWork;
            _clientRepository = unitOfWork.ClientRepository;
        }


        public async Task<ClientModel> GetById(int id)
        {
            var client = await _clientRepository.GetById(id);

            if (client == null)
            {
                return null;
            }

            return _objectMapper.Map<ClientModel>(client);
        }

        public async Task<ClientEditModel> GetForEdit(int id)
        {
            var client = await _clientRepository.GetById(id);
            if (client == null)
            {
                return null;
            }

            var model = _objectMapper.Map<ClientEditModel>(client);

            return model;
        }

        public async Task<PagedResult<ClientModel>> GetPagedList(int page, int pageSize, string searchString = null, string sortOrder = null)
        {
            var clients = await _clientRepository.GetPagedList(page, pageSize, searchString, sortOrder);

            return _objectMapper.Map<PagedResult<ClientModel>>(clients);
        }

        public IEnumerable<Client> DropDownList()
        {
            return _clientRepository.DropDownList();
        }

        public async Task<OperationResponse> Save(ClientEditModel model)
        {
            var response = new OperationResponse();

            if (model == null)
            {
                return response.AddError("", "Model was null");
            }

            var client = new Client();

            if (model.ID != 0)
            {
                client = await _clientRepository.GetById(model.ID);
                if (client == null)
                {
                    return response.AddError("", "Cannot find client with id " + model.ID);
                }
            }

            _objectMapper.Map(model, client);

            if (!response.Success)
            {
                return response;
            }

            await _clientRepository.Save(client);
            await _unitOfWork.CommitAsync();

            return response;
        }

        public async Task<OperationResponse> Delete(ClientModel model)
        {

            var response = new OperationResponse();
            if (model == null)
            {
                return response.AddError("", "Model was null");
            }

            var client = await _clientRepository.GetById(model.ID);
            if (client == null)
            {
                return response.AddError("", "Cannot find client with id " + model.ID);
            }
            await _clientRepository.Delete(model.ID);
            await _unitOfWork.CommitAsync();

            return response;
        }
    }
}
