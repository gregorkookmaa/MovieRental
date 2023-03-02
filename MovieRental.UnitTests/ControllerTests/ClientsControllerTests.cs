using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using MovieRental.Controllers;
using MovieRental.Data;
using MovieRental.Models;
using MovieRental.Models.ViewModels;
using MovieRental.Services;
using Xunit;

namespace MovieRental.UnitTests.ControllerTests
{
    public class ClientsControllerTests
    {
        private readonly Mock<IClientService> _clientServiceMock;
        private readonly ClientsController _clientsController;

        public ClientsControllerTests()
        {
            _clientServiceMock = new Mock<IClientService>();
            _clientsController = new ClientsController(_clientServiceMock.Object);
        }


        [Fact]
        public async Task Index_should_return_list_of_clients()
        {
            // Arrange
            var page = 1;
            var clients = GetPagedClientList();
            _clientServiceMock.Setup(cs => cs.GetPagedList(page, It.IsAny<int>(), "", "")).
                               ReturnsAsync(() => clients);

            // Act
            var result = await _clientsController.Index("", "", page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.True(result.Model is PagedResult<ClientModel>);
        }

        [Fact]
        public async Task Index_should_return_default_view()
        {
            // Arrange
            var defaultViewNames = new[] { null, "Index" };
            var page = 1;
            var clients = GetPagedClientList();
            _clientServiceMock.Setup(cs => cs.GetPagedList(page, It.IsAny<int>(), "", ""))
                               .ReturnsAsync(() => clients);

            // Act
            var result = await _clientsController.Index("", "", page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Contains(result.ViewName, defaultViewNames);
        }

        [Fact]
        public async Task Index_should_survive_null_model()
        {
            // Arrange
            var page = 1;
            var clients = GetPagedClientList();
            _clientServiceMock.Setup(cs => cs.GetPagedList(page, It.IsAny<int>(), "", "")).
                               ReturnsAsync(() => clients);

            // Act
            var result = await _clientsController.Index("", "") as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_should_return_not_found_if_id_is_null()
        {
            // Act
            var result = await _clientsController.Details(null) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_should_return_not_found_if_client_is_null()
        {
            // Arrange
            var nonExistentId = -1;
            _clientServiceMock.Setup(cs => cs.GetById(It.IsAny<int>()))
                               .ReturnsAsync(() => null);

            // Act
            var result = await _clientsController.Details(nonExistentId) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_returns_correct_result_when_client_is_found()
        {
            // Arrange
            var model = GetClient();
            var defaultViewNames = new[] { null, "Details" };
            _clientServiceMock.Setup(cs => cs.GetById(It.IsAny<int>()))
                               .ReturnsAsync(() => model);

            // Act
            var result = await _clientsController.Details(model.ID) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Contains(result.ViewName, defaultViewNames);
            Assert.NotNull(result.Model);
            Assert.IsType<ClientModel>(result.Model);
        }

        [Fact]
        public async Task Edit_should_save_client_data()
        {
            // Arrange
            var client = GetClientEdit();
            var response = new OperationResponse();
            _clientServiceMock.Setup(cs => cs.Save(client))
                               .ReturnsAsync(() => response)
                               .Verifiable();

            // Act
            var result = await _clientsController.EditPost(client) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            _clientServiceMock.VerifyAll();
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_ids_do_not_match()
        {
            // Arrange
            var clientIdReal = 1;
            var clientIdDampered = 2;
            var client = new Client();
            client.ID = clientIdDampered;

            // Act
            var result = await _clientsController.Edit(clientIdReal) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_return_badresult_when_model_is_null()
        {
            // Arrange
            var client = (ClientEditModel)null;

            // Act
            var result = await _clientsController.EditPost(client) as BadRequestResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_stay_on_form_when_model_is_invalid()
        {
            // Arrange
            var defaultViewNames = new[] { null, "Edit" };
            var clientId = 1;
            var client = new ClientEditModel();
            client.ID = clientId;
            client.FirstName = "012345678901234567890123456789012345678901234567890123456789";

            // Act
            _clientsController.ModelState.AddModelError("Id", "ERROR");
            var result = await _clientsController.EditPost(client);
            var typedResult = result as ViewResult;

            // Assert
            Assert.NotNull(typedResult);
            Assert.Contains(typedResult.ViewName, defaultViewNames);
            Assert.False(_clientsController.ModelState.IsValid);
            _clientServiceMock.VerifyAll();
        }

        [Fact]
        public async Task Delete_should_return_not_found_if_id_is_null()
        {
            // Arrange
            var clientId = (int?)null;

            // Act
            var result = await _clientsController.Delete(clientId) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_should_return_not_found_when_client_does_not_exist()
        {
            // Arrange
            var clientId = -100;
            var client = (ClientModel)null;
            _clientServiceMock.Setup(cs => cs.GetById(It.IsAny<int>()))
                               .ReturnsAsync(() => client);

            // Act
            var result = await _clientsController.Delete(clientId) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_should_show_confirmation_page()
        {
            // Arrange
            var defaultViewNames = new[] { null, "Delete" };
            var client = GetClient();
            _clientServiceMock.Setup(cs => cs.GetById(client.ID))
                               .ReturnsAsync(() => client);

            // Act
            var result = await _clientsController.Delete(client.ID) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Contains(result.ViewName, defaultViewNames);
            Assert.Equal(client, result.Model);
        }

        [Fact]
        public async Task DeleteConfirmed_should_return_not_found_if_client_is_null()
        {
            // Arrange
            var nonExistentId = -1;
            _clientServiceMock.Setup(cs => cs.GetById(It.IsAny<int>()))
                               .ReturnsAsync(() => null);

            // Act
            var result = await _clientsController.DeleteConfirmed(nonExistentId) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeleteConfirmed_should_delete_client()
        {
            // Arrange
            var client = GetClient();
            _clientServiceMock.Setup(cs => cs.GetById(client.ID))
                               .ReturnsAsync(() => client)
                               .Verifiable();
            _clientServiceMock.Setup(cs => cs.Delete(client))
                               .Verifiable();

            // Act
            var result = await _clientsController.DeleteConfirmed(client.ID) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            _clientServiceMock.VerifyAll();
        }

        private ClientModel GetClient()
        {
            return GetClientList()[0];
        }

        private IList<ClientModel> GetClientList()
        {
            return new List<ClientModel>
            {
                new ClientModel { ID = 1, FirstName = "ClientFirstName1", LastName = "ClientLastName1" },
                new ClientModel { ID = 2, FirstName = "ClientFirstName2", LastName = "ClientLastName2" }
            };
        }

        private PagedResult<ClientModel> GetPagedClientList()
        {
            return new PagedResult<ClientModel>
            {
                CurrentPage = 1,
                PageCount = 1,
                PageSize = 10,
                Results = GetClientList(),
                RowCount = 2
            };
        }

        private ClientEditModel GetClientEdit()
        {
            var model = GetClient();
            var editModel = new ClientEditModel();

            editModel.ID = model.ID;
            editModel.FirstName = model.FirstName;
            editModel.LastName = model.LastName;

            return editModel;
        }
    }
}
