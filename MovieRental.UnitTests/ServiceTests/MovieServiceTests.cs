using AutoMapper;
using Moq;
using System.Threading.Tasks;
using MovieRental.Core.IConfiguration;
using MovieRental.Core.Repository.MovieRepo;
using MovieRental.Data;
using MovieRental.Models;
using MovieRental.Models.ViewModels;
using MovieRental.Services;
using Xunit;

namespace MovieRental.UnitTests.ServiceTests
{
    public class MovieServiceTests
    {
        private readonly Mock<IMovieRepository> _movieRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly MovieService _movieService;

        public MovieServiceTests()
        {
            _movieRepositoryMock = new Mock<IMovieRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(Program).Assembly);
            });
            var mapper = mapperConfig.CreateMapper();

            _unitOfWorkMock.SetupGet(uow => uow.MovieRepository)
                           .Returns(_movieRepositoryMock.Object);

            _movieService = new MovieService(_unitOfWorkMock.Object, mapper);
        }

        [Fact]
        public async Task List_returns_paged_list_of_movie_models()
        {
            // Arrange
            int page = 1;
            int pageSize = 10;
            _movieRepositoryMock.Setup(er => er.GetPagedList(page, pageSize, "", ""))
                                  .ReturnsAsync(() => new PagedResult<Movie>())
                                  .Verifiable();

            // Act
            var result = await _movieService.GetPagedList(page, pageSize, "", "");

            // Assert
            Assert.NotNull(result);
            Assert.IsType<PagedResult<MoviesModel>>(result);
            _movieRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task GetById_should_return_null_if_movie_was_not_found()
        {
            // Arrange
            var nonExistentId = -1;
            var nullMovie = (Movie)null;
            _movieRepositoryMock.Setup(er => er.GetById(nonExistentId))
                                  .ReturnsAsync(() => nullMovie)
                                  .Verifiable();

            // Act
            var result = await _movieService.GetById(nonExistentId);

            // Assert
            Assert.Null(result);
            _movieRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task GetById_should_return_movie()
        {
            // Arrange
            var id = 1;
            var movie = new Movie { ID = id };
            _movieRepositoryMock.Setup(er => er.GetById(id))
                                  .ReturnsAsync(() => movie)
                                  .Verifiable();

            // Act
            var result = await _movieService.GetById(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<MoviesModel>(result);
            _movieRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task GetForEdit_should_return_null_if_movie_was_not_found()
        {
            // Arrange
            var nonExistentId = -1;
            var nullMovie = (Movie)null;
            _movieRepositoryMock.Setup(er => er.GetById(nonExistentId))
                                  .ReturnsAsync(() => nullMovie)
                                  .Verifiable();

            // Act
            var result = await _movieService.GetForEdit(nonExistentId);

            // Assert
            Assert.Null(result);
            _movieRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task GetForEdit_should_return_movie()
        {
            // Arrange
            var id = 1;
            var movie = new Movie { ID = id };
            _movieRepositoryMock.Setup(er => er.GetById(id))
                                  .ReturnsAsync(() => movie)
                                  .Verifiable();

            // Act
            var result = await _movieService.GetForEdit(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<MoviesEditModel>(result);
            _movieRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task Save_should_survive_null_model()
        {
            // Arrange
            var model = (MoviesEditModel)null;

            // Act
            var response = await _movieService.Save(model);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task Save_should_handle_missing_movie()
        {
            //Arrange
            var id = 1;
            var movie = new MoviesEditModel { ID = id };
            var nullMovie = (Movie)null;
            _movieRepositoryMock.Setup(er => er.GetById(id))
                                  .ReturnsAsync(() => nullMovie)
                                  .Verifiable();

            //Act
            var response = await _movieService.Save(movie);

            //Assert
            Assert.NotNull(response);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task Save_should_save_valid_movie()
        {
            // Arrange
            var id = 1;
            var movie = new Movie { ID = id };
            var movieModel = new MoviesEditModel { ID = id };

            _movieRepositoryMock.Setup(er => er.GetById(id))
                                  .ReturnsAsync(() => movie)
                                  .Verifiable();
            _movieRepositoryMock.Setup(er => er.Save(It.IsAny<Movie>()))
                                  .Verifiable();
            _unitOfWorkMock.Setup(uow => uow.CommitAsync())
                           .Verifiable();

            // Act
            var response = await _movieService.Save(movieModel);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.Success);
            _movieRepositoryMock.VerifyAll();
            _unitOfWorkMock.VerifyAll();
        }

        [Fact]
        public async Task Delete_should_survive_null_model()
        {
            // Arrange
            var model = (MoviesModel)null;

            // Act
            var response = await _movieService.Delete(model);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task Delete_handles_null_movie()
        {
            // Arrange
            var id = 1;
            var movieModelToDelete = new MoviesModel { ID = id };
            var movieToDelete = (Movie)null;

            _movieRepositoryMock.Setup(er => er.GetById(id))
                                  .ReturnsAsync(() => movieToDelete)
                                  .Verifiable();

            // Act
            var response = await _movieService.Delete(movieModelToDelete);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.Success);
            _movieRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task Delete_deletes_movie()
        {
            // Arrange
            var id = 1;
            var movieModelToDelete = new MoviesModel { ID = id };
            var movieToDelete = new Movie { ID = id };

            _movieRepositoryMock.Setup(er => er.GetById(id))
                                  .ReturnsAsync(() => movieToDelete)
                                  .Verifiable();
            _movieRepositoryMock.Setup(er => er.Delete(id))
                                  .Verifiable();
            _unitOfWorkMock.Setup(uow => uow.CommitAsync())
                           .Verifiable();

            // Act
            var response = await _movieService.Delete(movieModelToDelete);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.Success);
            _movieRepositoryMock.VerifyAll();
            _unitOfWorkMock.VerifyAll();
        }
    }
}
