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
    public class RentingMoviesControllerTests
    {
        private readonly Mock<IRentingMovieService> _rentingMovieServiceMock;
        private readonly RentingMoviesController _rentingMoviesController;

        public RentingMoviesControllerTests()
        {
            _rentingMovieServiceMock = new Mock<IRentingMovieService>();
            _rentingMoviesController = new RentingMoviesController(_rentingMovieServiceMock.Object);
        }


        [Fact]
        public async Task Index_should_return_list_of_rentingMovies()
        {
            // Arrange
            var page = 1;
            var rentingMovies = GetPagedRentingMovieList();
            _rentingMovieServiceMock.Setup(tes => tes.GetPagedList(page, It.IsAny<int>(), "", "")).
                               ReturnsAsync(() => rentingMovies);

            // Act
            var result = await _rentingMoviesController.Index("", "", page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.True(result.Model is PagedResult<RentingMovieModel>);
        }

        [Fact]
        public async Task Index_should_return_default_view()
        {
            // Arrange
            var defaultViewNames = new[] { null, "Index" };
            var page = 1;
            var rentingMovies = GetPagedRentingMovieList();
            _rentingMovieServiceMock.Setup(tes => tes.GetPagedList(page, It.IsAny<int>(), "", ""))
                               .ReturnsAsync(() => rentingMovies);

            // Act
            var result = await _rentingMoviesController.Index("", "", page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Contains(result.ViewName, defaultViewNames);
        }

        [Fact]
        public async Task Index_should_survive_null_model()
        {
            // Arrange
            var page = 1;
            var rentingMovies = GetPagedRentingMovieList();
            _rentingMovieServiceMock.Setup(tes => tes.GetPagedList(page, It.IsAny<int>(), "", "")).
                               ReturnsAsync(() => rentingMovies);

            // Act
            var result = await _rentingMoviesController.Index("", "") as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_should_return_not_found_if_id_is_null()
        {
            // Act
            var result = await _rentingMoviesController.Details(null) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_should_return_not_found_if_rentingMovie_is_null()
        {
            // Arrange
            var nonExistentId = -1;
            _rentingMovieServiceMock.Setup(tes => tes.GetById(It.IsAny<int>()))
                               .ReturnsAsync(() => null);

            // Act
            var result = await _rentingMoviesController.Details(nonExistentId) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_returns_correct_result_when_rentingMovie_is_found()
        {
            // Arrange
            var model = GetRentingMovie();
            var defaultViewNames = new[] { null, "Details" };
            _rentingMovieServiceMock.Setup(tes => tes.GetById(It.IsAny<int>()))
                               .ReturnsAsync(() => model);

            // Act
            var result = await _rentingMoviesController.Details(model.ID) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Contains(result.ViewName, defaultViewNames);
            Assert.NotNull(result.Model);
            Assert.IsType<RentingMovieModel>(result.Model);
        }

        [Fact]
        public async Task Edit_should_save_rentingMovie_data()
        {
            // Arrange
            var rentingMovie = GetRentingMovieEdit();
            var response = new OperationResponse();
            _rentingMovieServiceMock.Setup(tes => tes.Save(rentingMovie))
                               .ReturnsAsync(() => response)
                               .Verifiable();

            // Act
            var result = await _rentingMoviesController.EditPost(rentingMovie) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            _rentingMovieServiceMock.VerifyAll();
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_ids_does_not_match()
        {
            // Arrange
            var rentingMovieIdReal = 1;
            var rentingMovieIdDampered = 2;
            var rentingMovie = new RentingMovie();
            rentingMovie.ID = rentingMovieIdDampered;

            // Act
            var result = await _rentingMoviesController.Edit(rentingMovieIdReal) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_return_badresult_when_model_is_null()
        {
            // Arrange
            var rentingMovie = (RentingMovieEditModel)null;

            // Act
            var result = await _rentingMoviesController.EditPost(rentingMovie) as BadRequestResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_stay_on_form_when_model_is_invalid()
        {
            // Arrange
            var defaultViewNames = new[] { null, "Edit" };
            var rentingMovieId = 1;
            var rentingMovie = new RentingMovieEditModel();
            rentingMovie.ID = rentingMovieId;
            rentingMovie.ClientRating = "012345678901234567890123456789012345678901234567890123456789";

            // Act
            _rentingMoviesController.ModelState.AddModelError("Id", "ERROR");
            var result = await _rentingMoviesController.EditPost(rentingMovie);
            var typedResult = result as ViewResult;

            // Assert
            Assert.NotNull(typedResult);
            Assert.Contains(typedResult.ViewName, defaultViewNames);
            Assert.False(_rentingMoviesController.ModelState.IsValid);
            _rentingMovieServiceMock.VerifyAll();
        }

        [Fact]
        public async Task Delete_should_return_not_found_if_id_is_null()
        {
            // Arrange
            var rentingMovieId = (int?)null;

            // Act
            var result = await _rentingMoviesController.Delete(rentingMovieId) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_should_return_not_found_when_rentingMovie_does_not_exist()
        {
            // Arrange
            var rentingMovieId = -100;
            var rentingMovie = (RentingMovieModel)null;
            _rentingMovieServiceMock.Setup(tes => tes.GetById(It.IsAny<int>()))
                               .ReturnsAsync(() => rentingMovie);

            // Act
            var result = await _rentingMoviesController.Delete(rentingMovieId) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_should_show_confirmation_page()
        {
            // Arrange
            var defaultViewNames = new[] { null, "Delete" };
            var rentingMovie = GetRentingMovie();
            _rentingMovieServiceMock.Setup(tes => tes.GetById(rentingMovie.ID))
                               .ReturnsAsync(() => rentingMovie);

            // Act
            var result = await _rentingMoviesController.Delete(rentingMovie.ID) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Contains(result.ViewName, defaultViewNames);
            Assert.Equal(rentingMovie, result.Model);
        }

        [Fact]
        public async Task DeleteConfirmed_should_return_not_found_if_rentingMovie_is_null()
        {
            // Arrange
            var nonExistentId = -1;
            _rentingMovieServiceMock.Setup(tes => tes.GetById(It.IsAny<int>()))
                               .ReturnsAsync(() => null);

            // Act
            var result = await _rentingMoviesController.DeleteConfirmed(nonExistentId) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeleteConfirmed_should_delete_rentingMovie()
        {
            // Arrange
            var rentingMovie = GetRentingMovie();
            _rentingMovieServiceMock.Setup(tes => tes.GetById(rentingMovie.ID))
                               .ReturnsAsync(() => rentingMovie)
                               .Verifiable();
            _rentingMovieServiceMock.Setup(tes => tes.Delete(rentingMovie))
                               .Verifiable();

            // Act
            var result = await _rentingMoviesController.DeleteConfirmed(rentingMovie.ID) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            _rentingMovieServiceMock.VerifyAll();
        }

        private RentingMovieModel GetRentingMovie()
        {
            return GetRentingMovieList()[0];
        }

        private IList<RentingMovieModel> GetRentingMovieList()
        {
            return new List<RentingMovieModel>
            {
                new RentingMovieModel { ID = 1, RentingID = 1,  MovieID = 1, },
                new RentingMovieModel { ID = 2, RentingID = 2,  MovieID = 1, }
            };
        }

        private PagedResult<RentingMovieModel> GetPagedRentingMovieList()
        {
            return new PagedResult<RentingMovieModel>
            {
                CurrentPage = 1,
                PageCount = 1,
                PageSize = 10,
                Results = GetRentingMovieList(),
                RowCount = 2
            };
        }

        private RentingMovieEditModel GetRentingMovieEdit()
        {
            var model = GetRentingMovie();
            var editModel = new RentingMovieEditModel();

            editModel.ID = model.ID;
            editModel.RentingID = model.RentingID;
            editModel.MovieID = model.MovieID;

            return editModel;
        }
    }
}
