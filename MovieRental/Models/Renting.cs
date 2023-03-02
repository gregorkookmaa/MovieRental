using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieRental.Models
{
    public class Renting : Entity
    {
        [DataType(DataType.Date)]
        [Display(Name = "Renting date")]
        public DateTime Date { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Renting Expires")]
        public DateTime Expires { get; set; }

        public Client Client { get; set; }

        public ICollection<RentingMovie> RentingMovies { get; set; }
    }
}
