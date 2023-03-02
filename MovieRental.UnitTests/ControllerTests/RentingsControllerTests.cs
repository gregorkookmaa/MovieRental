using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
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
    public class RentingsControllerTests
    {
        private readonly Mock<IRentingService> _rentingServiceMock;
        private readonly RentingsController _rentingsController;

        public RentingsControllerTests()
        {
            _rentingServiceMock = new Mock<IRentingService>();
            _rentingsController = new RentingsController(_rentingServiceMock.Object);
        }


        [Fact]
        public async Task Index_should_return_list_of_rentings()
        {
            // Arrange
            var page = 1;
            var rentings = GetPagedClientList();
            _rentingServiceMock.Setup(ts => ts.GetPagedList(page, It.IsAny<int>(), "", "")).
                               ReturnsAsync(() => rentings);

            // Act
            var result = await _rentingsController.Index("", "", page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.True(result.Model is PagedResult<RentingModel>);
        }

        [Fact]
        public async Task Index_should_return_default_view()
        {
            // Arrange
            var defaultViewNames = new[] { null, "Index" };
            var page = 1;
            var rentings = GetPagedClientList();
            _rentingServiceMock.Setup(ts => ts.GetPagedList(page, It.IsAny<int>(), "", ""))
                               .ReturnsAsync(() => rentings);

            // Act
            var result = await _rentingsController.Index("", "", page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Contains(result.ViewName, defaultViewNames);
        }

        [Fact]
        public async Task Index_should_survive_null_model()
        {
            // Arrange
            var page = 1;
            var rentings = GetPagedClientList();
            _rentingServiceMock.Setup(ts => ts.GetPagedList(page, It.IsAny<int>(), "", "")).
                               ReturnsAsync(() => rentings);

            // Act
            var result = await _rentingsController.Index("", "") as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_should_return_not_found_if_id_is_null()
        {
            // Act
            var result = await _rentingsController.Details(null) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_should_return_not_found_if_renting_is_null()
        {
            // Arrange
            var nonExistentId = -1;
            _rentingServiceMock.Setup(ts => ts.GetById(It.IsAny<int>()))
                               .ReturnsAsync(() => null);

            // Act
            var result = await _rentingsController.Details(nonExistentId) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_returns_correct_result_when_renting_is_found()
        {
            // Arrange
            var model = GetRenting();
            var defaultViewNames = new[] { null, "Details" };
            _rentingServiceMock.Setup(ts => ts.GetById(It.IsAny<int>()))
                               .ReturnsAsync(() => model);

            // Act
            var result = await _rentingsController.Details(model.ID) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Contains(result.ViewName, defaultViewNames);
            Assert.NotNull(result.Model);
            Assert.IsType<RentingModel>(result.Model);
        }

        [Fact]
        public async Task Edit_should_save_renting_data()
        {
            // Arrange
            var renting = GetRentingEdit();
            var response = new OperationResponse();
            _rentingServiceMock.Setup(ts => ts.Save(renting))
                               .ReturnsAsync(() => response)
                               .Verifiable();

            // Act
            var result = await _rentingsController.EditPost(renting) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            _rentingServiceMock.VerifyAll();
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_ids_does_not_match()
        {
            // Arrange
            var rentingIdReal = 1;
            var rentingIdDampered = 2;
            var renting = new Renting();
            renting.ID = rentingIdDampered;

            // Act
            var result = await _rentingsController.Edit(rentingIdReal) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_return_badresult_when_model_is_null()
        {
            // Arrange
            var renting = (RentingEditModel)null;

            // Act
            var result = await _rentingsController.EditPost(renting) as BadRequestResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_stay_on_form_when_model_is_invalid()
        {
            // Arrange
            var defaultViewNames = new[] { null, "Edit" };
            var rentingId = 1;
            var renting = new RentingEditModel();
            renting.ID = rentingId;
            renting.ClientID = 999999999;

            // Act
            _rentingsController.ModelState.AddModelError("Id", "ERROR");
            var result = await _rentingsController.EditPost(renting);
            var typedResult = result as ViewResult;

            // Assert
            Assert.NotNull(typedResult);
            Assert.Contains(typedResult.ViewName, defaultViewNames);
            Assert.False(_rentingsController.ModelState.IsValid);
            _rentingServiceMock.VerifyAll();
        }

        [Fact]
        public async Task Delete_should_return_not_found_if_id_is_null()
        {
            // Arrange
            var rentingId = (int?)null;

            // Act
            var result = await _rentingsController.Delete(rentingId) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_should_return_not_found_when_renting_does_not_exist()
        {
            // Arrange
            var rentingId = -100;
            var renting = (RentingModel)null;
            _rentingServiceMock.Setup(ts => ts.GetById(It.IsAny<int>()))
                               .ReturnsAsync(() => renting);

            // Act
            var result = await _rentingsController.Delete(rentingId) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_should_show_confirmation_page()
        {
            // Arrange
            var defaultViewNames = new[] { null, "Delete" };
            var renting = GetRenting();
            _rentingServiceMock.Setup(ts => ts.GetById(renting.ID))
                               .ReturnsAsync(() => renting);

            // Act
            var result = await _rentingsController.Delete(renting.ID) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Contains(result.ViewName, defaultViewNames);
            Assert.Equal(renting, result.Model);
        }

        [Fact]
        public async Task DeleteConfirmed_should_return_not_found_if_renting_is_null()
        {
            // Arrange
            var nonExistentId = -1;
            _rentingServiceMock.Setup(ts => ts.GetById(It.IsAny<int>()))
                               .ReturnsAsync(() => null);

            // Act
            var result = await _rentingsController.DeleteConfirmed(nonExistentId) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeleteConfirmed_should_delete_renting()
        {
            // Arrange
            var renting = GetRenting();
            _rentingServiceMock.Setup(ts => ts.GetById(renting.ID))
                               .ReturnsAsync(() => renting)
                               .Verifiable();
            _rentingServiceMock.Setup(ts => ts.Delete(renting))
                               .Verifiable();

            // Act
            var result = await _rentingsController.DeleteConfirmed(renting.ID) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            _rentingServiceMock.VerifyAll();
        }

        private RentingModel GetRenting()
        {
            return GetRentingList()[0];
        }

        private IList<RentingModel> GetRentingList()
        {
            return new List<RentingModel>
            {
                new RentingModel { ID = 1, Date=DateTime.Parse("2021-08-02")},
                new RentingModel { ID = 2, Date=DateTime.Parse("2021-08-03")}
            };
        }

        private PagedResult<RentingModel> GetPagedClientList()
        {
            return new PagedResult<RentingModel>
            {
                CurrentPage = 1,
                PageCount = 1,
                PageSize = 10,
                Results = GetRentingList(),
                RowCount = 2
            };
        }

        private RentingEditModel GetRentingEdit()
        {
            var model = GetRenting();
            var editModel = new RentingEditModel();

            editModel.ID = model.ID;
            editModel.Date = model.Date;

            return editModel;
        }
    }
}
