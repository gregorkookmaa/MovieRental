using System;
using System.Threading.Tasks;
using MovieRental.Core.IConfiguration;
using MovieRental.Core.Repository.ClientRepo;
using MovieRental.Core.Repository.MovieRepo;
using MovieRental.Core.Repository.RentingMovieRepo;
using MovieRental.Core.Repository.RentingRepo;

namespace MovieRental.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly RentingContext _context;

        public IClientRepository ClientRepository { get; private set; }

        public IMovieRepository MovieRepository { get; private set; }

        public IRentingRepository RentingRepository { get; private set; }

        public IRentingMovieRepository RentingMovieRepository { get; private set; }

        public UnitOfWork(RentingContext context,
                          IClientRepository clientRepository,
                          IMovieRepository movieRepository,
                          IRentingRepository rentingRepository,
                          IRentingMovieRepository rentingMovieRepository)
        {
            _context = context;

            ClientRepository = clientRepository;
            MovieRepository = movieRepository;
            RentingRepository = rentingRepository;
            RentingMovieRepository = rentingMovieRepository;
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task BeginTransaction()
        {
            await Task.CompletedTask;
        }

        public async Task Rollback()
        {
            await Task.CompletedTask;
        }
    }
}