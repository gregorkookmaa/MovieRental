using MovieRental.Models;
using Microsoft.EntityFrameworkCore;

namespace MovieRental.Data
{
    public class RentingContext : DbContext
    {
        public RentingContext()
        {
        }
        public RentingContext(DbContextOptions<RentingContext> options) : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Renting> Rentings { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<RentingMovie> RentingMovies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().ToTable("Client");
            modelBuilder.Entity<Renting>().ToTable("Renting");
            modelBuilder.Entity<Movie>().ToTable("Movies");
            modelBuilder.Entity<RentingMovie>().ToTable("RentingMovie");
        }
    }
}
