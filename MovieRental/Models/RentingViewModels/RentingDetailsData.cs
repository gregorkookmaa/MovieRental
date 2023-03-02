using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieRental.Models.RentingViewModels
{
    public class RentingDetailsData
    {
        
            public IEnumerable<Renting> Rentings { get; set; }
            public IEnumerable<RentingMovie> RentingMovies { get; set; }
            public IEnumerable<Movie> Movies { get; set; }

    }
}
