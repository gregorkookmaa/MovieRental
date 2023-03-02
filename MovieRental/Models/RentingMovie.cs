using System;
using System.ComponentModel.DataAnnotations;

namespace MovieRental.Models
{
    public class RentingMovie : Entity
    {
        [Display(Name = "Client Rating")]
        public String ClientRating { get; set; }

        public Renting Renting { get; set; }

        public Movie Movie { get; set; }
    }
}
