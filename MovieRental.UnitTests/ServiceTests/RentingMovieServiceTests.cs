using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MovieRental.Core.IConfiguration;
using MovieRental.Core.Repository.MovieRepo;
using MovieRental.Core.Repository.RentingMovieRepo;
using MovieRental.Core.Repository.RentingRepo;
using MovieRental.Data;
using MovieRental.Models;
using MovieRental.Models.ViewModels;
using MovieRental.Services;
using Xunit;

namespace MovieRental.UnitTests.ServiceTests
{
    public class RentingMovieServiceTests
    {
        private readonly Mock<IRentingMovieRepository> _rentingMovieRepositoryMock;
        private readonly Mock<IMovieRepository> _movieRepositoryMock;
        private readonly Mock<IRentingRepository> _rentingRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly RentingMovieService _rentingMovieService;

        public RentingMovieServiceTests()
        {
            _rentingMovieRepositoryMock = new Mock<IRentingMovieRepository>();
            _movieRepositoryMock = new Mock<IMovieRepository>();
            _rentingRepositoryMock = new Mock<IRentingRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(Program).Assembly);
            });
            var mapper = mapperConfig.CreateMapper();

            _unitOfWorkMock.SetupGet(uow => uow.RentingMovieRepository)
                           .Returns(_rentingMovieRepositoryMock.Object);

            _unitOfWorkMock.SetupGet(uow => uow.MovieRepository)
                         .Returns(_movieRepositoryMock.Object);

            _unitOfWorkMock.SetupGet(uow => uow.RentingRepository)
                         .Returns(_rentingRepositoryMock.Object);

            _rentingMovieService = new RentingMovieService(_unitOfWorkMock.Object, mapper);
        }

        [Fact]
        public async Task List_returns_paged_list_of_rentingMovie_models()
        {
            // Arrange
            int page = 1;
            int pageSize = 10;
            _rentingMovieRepositoryMock.Setup(ter => ter.GetPagedList(page, pageSize, "", ""))
                                  .ReturnsAsync(() => new PagedResult<RentingMovie>())
                                  .Verifiable();

            // Act
            var result = await _rentingMovieService.GetPagedList(page, pageSize, "", "");

            // Assert
            Assert.NotNull(result);
            Assert.IsType<PagedResult<RentingMovieModel>>(result);
            _rentingMovieRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task GetById_should_return_null_if_rentingMovie_was_not_found()
        {
            // Arrange
            var nonExistentId = -1;
            var nullRentingMovie = (RentingMovie)null;
            _rentingMovieRepositoryMock.Setup(ter => ter.GetById(nonExistentId))
                                  .ReturnsAsync(() => nullRentingMovie)
                                  .Verifiable();

            // Act
            var result = await _rentingMovieService.GetById(nonExistentId);

            // Assert
            Assert.Null(result);
            _rentingMovieRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task GetById_should_return_rentingMovie()
        {
            // Arrange
            var id = 1;
            var rentingMovie = new RentingMovie { ID = id };
            _rentingMovieRepositoryMock.Setup(ter => ter.GetById(id))
                                  .ReturnsAsync(() => rentingMovie)
                                  .Verifiable();

            // Act
            var result = await _rentingMovieService.GetById(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<RentingMovieModel>(result);
            _rentingMovieRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task GetForEdit_should_return_null_if_rentingMovie_was_not_found()
        {
            // Arrange
            var nonExistentId = -1;
            var nullRentingMovie = (RentingMovie)null;
            _rentingMovieRepositoryMock.Setup(ter => ter.GetById(nonExistentId))
                                  .ReturnsAsync(() => nullRentingMovie)
                                  .Verifiable();

            // Act
            var result = await _rentingMovieService.GetForEdit(nonExistentId);

            // Assert
            Assert.Null(result);
            _rentingMovieRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task GetForEdit_should_return_rentingMovie()
        {
            // Arrange
            var id = 1;
            var rentingMovie = new RentingMovie { ID = id };
            var movies = GetMoviesPaged();
            var rentings = GetRentingsPaged();
            _rentingMovieRepositoryMock.Setup(ter => ter.GetById(id))
                                  .ReturnsAsync(() => rentingMovie)
                                  .Verifiable();
            _movieRepositoryMock.Setup(er => er.GetPagedList(1, 100, It.IsAny<string>(), It.IsAny<string>()))
                                       .ReturnsAsync(() => movies);
            _rentingRepositoryMock.Setup(tr => tr.GetPagedList(1, 100, It.IsAny<string>(), It.IsAny<string>()))
                                       .ReturnsAsync(() => rentings);

            // Act
            var result = await _rentingMovieService.GetForEdit(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<RentingMovieEditModel>(result);
            _rentingMovieRepositoryMock.VerifyAll();
            _movieRepositoryMock.VerifyAll();
            _rentingRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task Save_should_survive_null_model()
        {
            // Arrange
            var model = (RentingMovieEditModel)null;

            // Act
            var response = await _rentingMovieService.Save(model);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task Save_should_handle_missing_rentingMovie()
        {
            //Arrange
            var id = 1;
            var rentingMovie = new RentingMovieEditModel { ID = id };
            var nullRentingMovie = (RentingMovie)null;
            _rentingMovieRepositoryMock.Setup(ter => ter.GetById(id))
                                  .ReturnsAsync(() => nullRentingMovie)
                                  .Verifiable();

            //Act
            var response = await _rentingMovieService.Save(rentingMovie);

            //Assert
            Assert.NotNull(response);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task Save_should_handle_missing_movie()
        {
            // Arrange
            var id = 1;
            var rentingMovie = new RentingMovie { ID = id };
            var rentingMovieModel = new RentingMovieEditModel { ID = id, MovieID = id };
            var movie = (Movie)null;

            _rentingMovieRepositoryMock.Setup(ter => ter.GetById(id))
                                  .ReturnsAsync(() => rentingMovie)
                                  .Verifiable();
            _movieRepositoryMock.Setup(er => er.GetById(id))
                                  .ReturnsAsync(() => movie)
                                  .Verifiable();

            // Act
            var response = await _rentingMovieService.Save(rentingMovieModel);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.Success);
            _rentingMovieRepositoryMock.VerifyAll();
            _movieRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task Save_should_handle_missing_renting()
        {
            // Arrange
            var id = 1;
            var rentingMovie = new RentingMovie { ID = id };
            var rentingMovieModel = new RentingMovieEditModel { ID = id, RentingID = id };
            var renting = (Renting)null;

            _rentingMovieRepositoryMock.Setup(ter => ter.GetById(id))
                                  .ReturnsAsync(() => rentingMovie)
                                  .Verifiable();
            _rentingRepositoryMock.Setup(tr => tr.GetById(id))
                                  .ReturnsAsync(() => renting)
                                  .Verifiable();

            // Act
            var response = await _rentingMovieService.Save(rentingMovieModel);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.Success);
            _rentingMovieRepositoryMock.VerifyAll();
            _rentingRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task Save_should_save_valid_rentingMovie()
        {
            // Arrange
            var id = 1;
            var rentingMovie = new RentingMovie { ID = id };
            var rentingMovieModel = new RentingMovieEditModel { ID = id, MovieID = id, RentingID = id };
            var movie = new Movie { ID = id };
            var renting = new Renting { ID = id };

            _rentingMovieRepositoryMock.Setup(ter => ter.GetById(id))
                                  .ReturnsAsync(() => rentingMovie)
                                  .Verifiable();
            _rentingMovieRepositoryMock.Setup(ter => ter.Save(It.IsAny<RentingMovie>()))
                                  .Verifiable();
            _movieRepositoryMock.Setup(er => er.GetById(id))
                                .ReturnsAsync(() => movie)
                                .Verifiable();
            _rentingRepositoryMock.Setup(tr => tr.GetById(id))
                                .ReturnsAsync(() => renting)
                                .Verifiable();
            _unitOfWorkMock.Setup(uow => uow.CommitAsync())
                           .Verifiable();

            // Act
            var response = await _rentingMovieService.Save(rentingMovieModel);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.Success);
            _rentingMovieRepositoryMock.VerifyAll();
            _movieRepositoryMock.VerifyAll();
            _rentingRepositoryMock.VerifyAll();
            _unitOfWorkMock.VerifyAll();
        }

        [Fact]
        public async Task Delete_should_survive_null_model()
        {
            // Arrange
            var model = (RentingMovieModel)null;

            // Act
            var response = await _rentingMovieService.Delete(model);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task Delete_handles_null_rentingMovie()
        {
            // Arrange
            var id = 1;
            var rentingMovieModelToDelete = new RentingMovieModel { ID = id };
            var rentingMovieToDelete = (RentingMovie)null;

            _rentingMovieRepositoryMock.Setup(ter => ter.GetById(id))
                                  .ReturnsAsync(() => rentingMovieToDelete)
                                  .Verifiable();

            // Act
            var response = await _rentingMovieService.Delete(rentingMovieModelToDelete);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.Success);
            _rentingMovieRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task Delete_deletes_rentingMovie()
        {
            // Arrange
            var id = 1;
            var rentingMovieModelToDelete = new RentingMovieModel { ID = id };
            var rentingMovieToDelete = new RentingMovie { ID = id };

            _rentingMovieRepositoryMock.Setup(ter => ter.GetById(id))
                                  .ReturnsAsync(() => rentingMovieToDelete)
                                  .Verifiable();
            _rentingMovieRepositoryMock.Setup(ter => ter.Delete(id))
                                  .Verifiable();
            _unitOfWorkMock.Setup(uow => uow.CommitAsync())
                           .Verifiable();

            // Act
            var response = await _rentingMovieService.Delete(rentingMovieModelToDelete);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.Success);
            _rentingMovieRepositoryMock.VerifyAll();
            _unitOfWorkMock.VerifyAll();
        }

        private PagedResult<Movie> GetMoviesPaged()
        {
            return new PagedResult<Movie>
            {
                CurrentPage = 1,
                PageCount = 1,
                PageSize = 10,
                Results = new List<Movie>
                {
                    new Movie { ID = 1, Title = "MovieTitle1"},
                    new Movie { ID = 2, Title = "MovieTitle2" }
                },
                RowCount = 2
            };
        }

        private PagedResult<Renting> GetRentingsPaged()
        {
            return new PagedResult<Renting>
            {
                CurrentPage = 1,
                PageCount = 1,
                PageSize = 10,
                Results = new List<Renting>
                {
                    new Renting { ID = 1, Date=DateTime.Parse("2021-08-02"), Client = new Client{FirstName="Rose",LastName="Rosalie"}},
                    new Renting { ID = 2, Date=DateTime.Parse("2021-08-03"), Client = new Client{FirstName="Larry",LastName="Labrador"}}
                },
                RowCount = 2
            };
        }
    }
}
