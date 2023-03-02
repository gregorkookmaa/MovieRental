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
    public class MoviesControllerTests
    {
        private readonly Mock<IMovieService> _movieServiceMock;
        private readonly MoviesController _movieController;

        public MoviesControllerTests()
        {
            _movieServiceMock = new Mock<IMovieService>();
            _movieController = new MoviesController(_movieServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_list_of_movies()
        {
            // Arrange
            var page = 1;
            var movies = GetPagedMovieList();
            _movieServiceMock.Setup(es => es.GetPagedList(page, It.IsAny<int>(), "", "")).
                               ReturnsAsync(() => movies);

            // Act
            var result = await _movieController.Index("", "", page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.True(result.Model is PagedResult<MoviesModel>);
        }

        [Fact]
        public async Task Index_should_return_default_view()
        {
            // Arrange
            var defaultViewNames = new[] { null, "Index" };
            var page = 1;
            var movies = GetPagedMovieList();
            _movieServiceMock.Setup(es => es.GetPagedList(page, It.IsAny<int>(), "", ""))
                               .ReturnsAsync(() => movies);

            // Act
            var result = await _movieController.Index("", "", page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Contains(result.ViewName, defaultViewNames);
        }

        [Fact]
        public async Task Index_should_survive_null_model()
        {
            // Arrange
            var page = 1;
            var movies = GetPagedMovieList();
            _movieServiceMock.Setup(es => es.GetPagedList(page, It.IsAny<int>(), "", "")).
                               ReturnsAsync(() => movies);

            // Act
            var result = await _movieController.Index("", "") as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_should_return_not_found_if_id_is_null()
        {
            // Act
            var result = await _movieController.Details(null) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_should_return_not_found_if_movie_is_null()
        {
            // Arrange
            var nonExistentId = -1;
            _movieServiceMock.Setup(es => es.GetById(It.IsAny<int>()))
                               .ReturnsAsync(() => null);

            // Act
            var result = await _movieController.Details(nonExistentId) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_returns_correct_result_when_movie_is_found()
        {
            // Arrange
            var model = GetMovie();
            var defaultViewNames = new[] { null, "Details" };
            _movieServiceMock.Setup(es => es.GetById(It.IsAny<int>()))
                               .ReturnsAsync(() => model);

            // Act
            var result = await _movieController.Details(model.ID) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Contains(result.ViewName, defaultViewNames);
            Assert.NotNull(result.Model);
            Assert.IsType<MoviesModel>(result.Model);
        }

        [Fact]
        public async Task Edit_should_save_movie_data()
        {
            // Arrange
            var movie = GetMovieEdit();
            var response = new OperationResponse();
            _movieServiceMock.Setup(es => es.Save(movie))
                               .ReturnsAsync(() => response)
                               .Verifiable();

            // Act
            var result = await _movieController.EditPost(movie) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            _movieServiceMock.VerifyAll();
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_ids_does_not_match()
        {
            // Arrange
            var movieIdReal = 1;
            var movieIdDampered = 2;
            var movie = new Movie();
            movie.ID = movieIdDampered;

            // Act
            var result = await _movieController.Edit(movieIdReal) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_return_badresult_when_model_is_null()
        {
            // Arrange
            var movie = (MoviesEditModel)null;

            // Act
            var result = await _movieController.EditPost(movie) as BadRequestResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_stay_on_form_when_model_is_invalid()
        {
            // Arrange
            var defaultViewNames = new[] { null, "Edit" };
            var movieId = 1;
            var movie = new MoviesEditModel();
            movie.ID = movieId;
            movie.Title = "012345678901234567890123456789012345678901234567890123456789";

            // Act
            _movieController.ModelState.AddModelError("Id", "ERROR");
            var result = await _movieController.EditPost(movie);
            var typedResult = result as ViewResult;

            // Assert
            Assert.NotNull(typedResult);
            Assert.Contains(typedResult.ViewName, defaultViewNames);
            Assert.False(_movieController.ModelState.IsValid);
            _movieServiceMock.VerifyAll();
        }

        [Fact]
        public async Task Delete_should_return_not_found_if_id_is_null()
        {
            // Arrange
            var movieId = (int?)null;

            // Act
            var result = await _movieController.Delete(movieId) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_should_return_not_found_when_movie_does_not_exist()
        {
            // Arrange
            var movieId = -100;
            var movie = (MoviesModel)null;
            _movieServiceMock.Setup(es => es.GetById(It.IsAny<int>()))
                               .ReturnsAsync(() => movie);

            // Act
            var result = await _movieController.Delete(movieId) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_should_show_confirmation_page()
        {
            // Arrange
            var defaultViewNames = new[] { null, "Delete" };
            var movie = GetMovie();
            _movieServiceMock.Setup(es => es.GetById(movie.ID))
                               .ReturnsAsync(() => movie);

            // Act
            var result = await _movieController.Delete(movie.ID) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Contains(result.ViewName, defaultViewNames);
            Assert.Equal(movie, result.Model);
        }

        [Fact]
        public async Task DeleteConfirmed_should_return_not_found_if_movie_is_null()
        {
            // Arrange
            var nonExistentId = -1;
            _movieServiceMock.Setup(es => es.GetById(It.IsAny<int>()))
                               .ReturnsAsync(() => null);

            // Act
            var result = await _movieController.DeleteConfirmed(nonExistentId) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeleteConfirmed_should_delete_movie()
        {
            // Arrange
            var movie = GetMovie();
            _movieServiceMock.Setup(es => es.GetById(movie.ID))
                               .ReturnsAsync(() => movie)
                               .Verifiable();
            _movieServiceMock.Setup(es => es.Delete(movie))
                               .Verifiable();

            // Act
            var result = await _movieController.DeleteConfirmed(movie.ID) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            _movieServiceMock.VerifyAll();
        }

        private MoviesModel GetMovie()
        {
            return GetMovieList()[0];
        }

        private IList<MoviesModel> GetMovieList()
        {
            return new List<MoviesModel>
            {
                new MoviesModel { ID = 1, Title = "MovieTitle1"
                                    //, MovieGenre ="MovieGenre1"
                                    },
                new MoviesModel { ID = 2, Title = "MovieTitle2"
                                    //, MovieGenre = "MovieGenre2" 
                                    }
            };
        }

        private PagedResult<MoviesModel> GetPagedMovieList()
        {
            return new PagedResult<MoviesModel>
            {
                CurrentPage = 1,
                PageCount = 1,
                PageSize = 10,
                Results = GetMovieList(),
                RowCount = 2
            };
        }

        private MoviesEditModel GetMovieEdit()
        {
            var model = GetMovie();
            var editModel = new MoviesEditModel();

            editModel.ID = model.ID;
            editModel.Title = model.Title;
            editModel.MovieGenre = model.MovieGenre;

            return editModel;
        }
    }
}
