using AutoMapper;
using Moq;
using System.Threading.Tasks;
using MovieRental.Core.IConfiguration;
using MovieRental.Core.Repository.ClientRepo;
using MovieRental.Data;
using MovieRental.Models;
using MovieRental.Models.ViewModels;
using MovieRental.Services;
using Xunit;

namespace MovieRental.UnitTests.ServiceTests
{
    public class ClientServiceTests
    {
        private readonly Mock<IClientRepository> _clientRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly ClientService _clientService;

        public ClientServiceTests()
        {
            _clientRepositoryMock = new Mock<IClientRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(Program).Assembly);
            });
            var mapper = mapperConfig.CreateMapper();

            _unitOfWorkMock.SetupGet(uow => uow.ClientRepository)
                           .Returns(_clientRepositoryMock.Object);

            _clientService = new ClientService(_unitOfWorkMock.Object, mapper);
        }

        [Fact]
        public async Task List_returns_paged_list_of_client_models()
        {
            // Arrange
            int page = 1;
            int pageSize = 10;
            _clientRepositoryMock.Setup(cr => cr.GetPagedList(page, pageSize, "", ""))
                                  .ReturnsAsync(() => new PagedResult<Client>())
                                  .Verifiable();

            // Act
            var result = await _clientService.GetPagedList(page, pageSize, "", "");

            // Assert
            Assert.NotNull(result);
            Assert.IsType<PagedResult<ClientModel>>(result);
            _clientRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task GetById_should_return_null_if_client_was_not_found()
        {
            // Arrange
            var nonExistentId = -1;
            var nullClient = (Client)null;
            _clientRepositoryMock.Setup(cr => cr.GetById(nonExistentId))
                                  .ReturnsAsync(() => nullClient)
                                  .Verifiable();

            // Act
            var result = await _clientService.GetById(nonExistentId);

            // Assert
            Assert.Null(result);
            _clientRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task GetById_should_return_client()
        {
            // Arrange
            var id = 1;
            var client = new Client { ID = id };
            _clientRepositoryMock.Setup(cr => cr.GetById(id))
                                  .ReturnsAsync(() => client)
                                  .Verifiable();

            // Act
            var result = await _clientService.GetById(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ClientModel>(result);
            _clientRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task GetForEdit_should_return_null_if_client_was_not_found()
        {
            // Arrange
            var nonExistentId = -1;
            var nullClient = (Client)null;
            _clientRepositoryMock.Setup(cr => cr.GetById(nonExistentId))
                                  .ReturnsAsync(() => nullClient)
                                  .Verifiable();

            // Act
            var result = await _clientService.GetForEdit(nonExistentId);

            // Assert
            Assert.Null(result);
            _clientRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task GetForEdit_should_return_client()
        {
            // Arrange
            var id = 1;
            var client = new Client { ID = id };
            _clientRepositoryMock.Setup(cr => cr.GetById(id))
                                  .ReturnsAsync(() => client)
                                  .Verifiable();

            // Act
            var result = await _clientService.GetForEdit(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ClientEditModel>(result);
            _clientRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task Save_should_survive_null_model()
        {
            // Arrange
            var model = (ClientEditModel)null;

            // Act
            var response = await _clientService.Save(model);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task Save_should_handle_missing_client()
        {
            //Arrange
            var id = 1;
            var client = new ClientEditModel { ID = id };
            var nullClient = (Client)null;
            _clientRepositoryMock.Setup(cr => cr.GetById(id))
                                  .ReturnsAsync(() => nullClient)
                                  .Verifiable();

            //Act
            var response = await _clientService.Save(client);

            //Assert
            Assert.NotNull(response);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task Save_should_save_valid_client()
        {
            // Arrange
            var id = 1;
            var client = new Client { ID = id };
            var clientModel = new ClientEditModel { ID = id };

            _clientRepositoryMock.Setup(cr => cr.GetById(id))
                                  .ReturnsAsync(() => client)
                                  .Verifiable();
            _clientRepositoryMock.Setup(cr => cr.Save(It.IsAny<Client>()))
                                  .Verifiable();
            _unitOfWorkMock.Setup(uow => uow.CommitAsync())
                           .Verifiable();

            // Act
            var response = await _clientService.Save(clientModel);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.Success);
            _clientRepositoryMock.VerifyAll();
            _unitOfWorkMock.VerifyAll();
        }

        [Fact]
        public async Task Delete_should_survive_null_model()
        {
            // Arrange
            var model = (ClientModel)null;

            // Act
            var response = await _clientService.Delete(model);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task Delete_handles_null_client()
        {
            // Arrange
            var id = 1;
            var clientModelToDelete = new ClientModel { ID = id };
            var clientToDelete = (Client)null;

            _clientRepositoryMock.Setup(cr => cr.GetById(id))
                                  .ReturnsAsync(() => clientToDelete)
                                  .Verifiable();

            // Act
            var response = await _clientService.Delete(clientModelToDelete);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.Success);
            _clientRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task Delete_deletes_client()
        {
            // Arrange
            var id = 1;
            var clientModelToDelete = new ClientModel { ID = id };
            var clientToDelete = new Client { ID = id };

            _clientRepositoryMock.Setup(cr => cr.GetById(id))
                                  .ReturnsAsync(() => clientToDelete)
                                  .Verifiable();
            _clientRepositoryMock.Setup(cr => cr.Delete(id))
                                  .Verifiable();
            _unitOfWorkMock.Setup(uow => uow.CommitAsync())
                           .Verifiable();

            // Act
            var response = await _clientService.Delete(clientModelToDelete);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.Success);
            _clientRepositoryMock.VerifyAll();
            _unitOfWorkMock.VerifyAll();
        }

    }
}
