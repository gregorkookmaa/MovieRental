using AutoMapper;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using MovieRental.Core.IConfiguration;
using MovieRental.Core.Repository.ClientRepo;
using MovieRental.Core.Repository.RentingRepo;
using MovieRental.Data;
using MovieRental.Models;
using MovieRental.Models.ViewModels;
using MovieRental.Services;
using Xunit;

namespace MovieRental.UnitTests.ServiceTests
{
    public class RentingServiceTests
    {
        private readonly Mock<IRentingRepository> _rentingRepositoryMock;
        private readonly Mock<IClientRepository> _clientRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly RentingService _rentingService;

        public RentingServiceTests()
        {
            _rentingRepositoryMock = new Mock<IRentingRepository>();
            _clientRepositoryMock = new Mock<IClientRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(Program).Assembly);
            });
            var mapper = mapperConfig.CreateMapper();

            _unitOfWorkMock.SetupGet(uow => uow.RentingRepository)
                           .Returns(_rentingRepositoryMock.Object);

            _unitOfWorkMock.SetupGet(uow => uow.ClientRepository)
                           .Returns(_clientRepositoryMock.Object);

            _rentingService = new RentingService(_unitOfWorkMock.Object, mapper);
        }

        [Fact]
        public async Task List_returns_paged_list_of_renting_models()
        {
            // Arrange
            int page = 1;
            int pageSize = 10;
            _rentingRepositoryMock.Setup(tr => tr.GetPagedList(page, pageSize, "", ""))
                                  .ReturnsAsync(() => new PagedResult<Renting>())
                                  .Verifiable();

            // Act
            var result = await _rentingService.GetPagedList(page, pageSize, "", "");

            // Assert
            Assert.NotNull(result);
            Assert.IsType<PagedResult<RentingModel>>(result);
            _rentingRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task GetById_should_return_null_if_renting_was_not_found()
        {
            // Arrange
            var nonExistentId = -1;
            var nullRenting = (Renting)null;
            _rentingRepositoryMock.Setup(tr => tr.GetById(nonExistentId))
                                  .ReturnsAsync(() => nullRenting)
                                  .Verifiable();

            // Act
            var result = await _rentingService.GetById(nonExistentId);

            // Assert
            Assert.Null(result);
            _rentingRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task GetById_should_return_renting()
        {
            // Arrange
            var id = 1;
            var renting = new Renting { ID = id };
            _rentingRepositoryMock.Setup(tr => tr.GetById(id))
                                  .ReturnsAsync(() => renting)
                                  .Verifiable();

            // Act
            var result = await _rentingService.GetById(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<RentingModel>(result);
            _rentingRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task GetForEdit_should_return_null_if_renting_was_not_found()
        {
            // Arrange
            var nonExistentId = -1;
            var nullRenting = (Renting)null;
            _rentingRepositoryMock.Setup(tr => tr.GetById(nonExistentId))
                                  .ReturnsAsync(() => nullRenting)
                                  .Verifiable();

            // Act
            var result = await _rentingService.GetForEdit(nonExistentId);

            // Assert
            Assert.Null(result);
            _rentingRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task GetForEdit_should_return_renting()
        {
            // Arrange
            var id = 1;
            var renting = new Renting { ID = id };
            var clients = GetClientsPaged();
            _rentingRepositoryMock.Setup(tr => tr.GetById(id))
                                  .ReturnsAsync(() => renting)
                                  .Verifiable();
            _clientRepositoryMock.Setup(cr => cr.GetPagedList(1, 100, It.IsAny<string>(), It.IsAny<string>()))
                                       .ReturnsAsync(() => clients);

            // Act
            var result = await _rentingService.GetForEdit(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<RentingEditModel>(result);
            _rentingRepositoryMock.VerifyAll();
            _clientRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task Save_should_survive_null_model()
        {
            // Arrange
            var model = (RentingEditModel)null;

            // Act
            var response = await _rentingService.Save(model);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task Save_should_handle_missing_renting()
        {
            //Arrange
            var id = 1;
            var renting = new RentingEditModel { ID = id };
            var nullRenting = (Renting)null;
            _rentingRepositoryMock.Setup(tr => tr.GetById(id))
                                  .ReturnsAsync(() => nullRenting)
                                  .Verifiable();

            //Act
            var response = await _rentingService.Save(renting);

            //Assert
            Assert.NotNull(response);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task Save_should_handle_missing_client()
        {
            // Arrange
            var id = 1;
            var renting = new Renting { ID = id };
            var rentingModel = new RentingEditModel { ID = id, ClientID = id };
            var client = (Client)null;

            _rentingRepositoryMock.Setup(tr => tr.GetById(id))
                                  .ReturnsAsync(() => renting)
                                  .Verifiable();
            _clientRepositoryMock.Setup(cr => cr.GetById(id))
                                  .ReturnsAsync(() => client)
                                  .Verifiable();

            // Act
            var response = await _rentingService.Save(rentingModel);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.Success);
            _rentingRepositoryMock.VerifyAll();
            _clientRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task Save_should_save_valid_renting()
        {
            // Arrange
            var id = 1;
            var renting = new Renting { ID = id };
            var rentingModel = new RentingEditModel { ID = id, ClientID = id };
            var client = new Client { ID = id };

            _rentingRepositoryMock.Setup(tr => tr.GetById(id))
                                  .ReturnsAsync(() => renting)
                                  .Verifiable();
            _rentingRepositoryMock.Setup(tr => tr.Save(It.IsAny<Renting>()))
                                  .Verifiable();
            _clientRepositoryMock.Setup(cr => cr.GetById(id))
                                 .ReturnsAsync(() => client)
                                 .Verifiable();
            _unitOfWorkMock.Setup(uow => uow.CommitAsync())
                           .Verifiable();

            // Act
            var response = await _rentingService.Save(rentingModel);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.Success);
            _rentingRepositoryMock.VerifyAll();
            _clientRepositoryMock.VerifyAll();
            _unitOfWorkMock.VerifyAll();
        }

        [Fact]
        public async Task Delete_should_survive_null_model()
        {
            // Arrange
            var model = (RentingModel)null;

            // Act
            var response = await _rentingService.Delete(model);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task Delete_handles_null_renting()
        {
            // Arrange
            var id = 1;
            var rentingModelToDelete = new RentingModel { ID = id };
            var rentingToDelete = (Renting)null;

            _rentingRepositoryMock.Setup(tr => tr.GetById(id))
                                  .ReturnsAsync(() => rentingToDelete)
                                  .Verifiable();

            // Act
            var response = await _rentingService.Delete(rentingModelToDelete);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.Success);
            _rentingRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task Delete_deletes_renting()
        {
            // Arrange
            var id = 1;
            var rentingModelToDelete = new RentingModel { ID = id };
            var rentingToDelete = new Renting { ID = id };

            _rentingRepositoryMock.Setup(tr => tr.GetById(id))
                                  .ReturnsAsync(() => rentingToDelete)
                                  .Verifiable();
            _rentingRepositoryMock.Setup(tr => tr.Delete(id))
                                  .Verifiable();
            _unitOfWorkMock.Setup(uow => uow.CommitAsync())
                           .Verifiable();

            // Act
            var response = await _rentingService.Delete(rentingModelToDelete);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.Success);
            _rentingRepositoryMock.VerifyAll();
            _unitOfWorkMock.VerifyAll();
        }

        private PagedResult<Client> GetClientsPaged()
        {
            return new PagedResult<Client>
            {
                CurrentPage = 1,
                PageCount = 1,
                PageSize = 10,
                Results = new List<Client>
                {
                    new Client { ID = 1, FirstName = "Firstname1", LastName = "Lastname1" },
                    new Client { ID = 2, FirstName = "Firstname2", LastName = "Lastname2" }
                },
                RowCount = 2
            };
        }
    }
}
