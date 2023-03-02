using MovieRental.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieRental.FileAccess;
using MovieRental.Core.Repository;
using MovieRental.Core.IConfiguration;
using MovieRental.Core.Repository.ClientRepo;
using MovieRental.Core.Repository.RentingRepo;
using MovieRental.Core.Repository.RentingMovieRepo;
using MovieRental.Core.Repository.MovieRepo;
using MovieRental.Services;
using System.Diagnostics.CodeAnalysis;

namespace MovieRental
{
    public class Startup
    {
        [ExcludeFromCodeCoverage]
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<RentingContext>(options =>
              options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddAutoMapper(GetType().Assembly);

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddControllersWithViews();

            services.AddScoped<IFileClient, LocalFileClient>();

            services.AddScoped<IClientRepository, ClientRepository>();

            services.AddScoped<IRentingRepository, RentingRepository>();

            services.AddScoped<IRentingMovieRepository, RentingMovieRepository>();

            services.AddScoped<IMovieRepository, MovieRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<IRentingService, RentingService>();
            services.AddScoped<IRentingMovieService, RentingMovieService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
