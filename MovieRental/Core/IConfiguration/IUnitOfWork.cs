using System.Threading.Tasks;
using MovieRental.Core.Repository.ClientRepo;
using MovieRental.Core.Repository.MovieRepo;
using MovieRental.Core.Repository.RentingMovieRepo;
using MovieRental.Core.Repository.RentingRepo;

namespace MovieRental.Core.IConfiguration
{
    public interface IUnitOfWork
    {
        IClientRepository ClientRepository { get; }

        IMovieRepository MovieRepository { get; }

        IRentingRepository RentingRepository { get; }

        IRentingMovieRepository RentingMovieRepository { get; }

        Task BeginTransaction();

        Task CommitAsync();

        Task Rollback();
    }
}
