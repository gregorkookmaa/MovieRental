using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MovieRental.Models.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class RentingMovieModel
    {
        public int ID { get; set; }
        public int RentingID { get; set; }
        public int MovieID { get; set; }
        [Display(Name = "Movies")]
        public string MovieName { get; set; }
        [Display(Name = "Client")]
        public string ClientName { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Renting date")]
        public DateTime RentingDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Expires date")]
        public DateTime ExpiresDate { get; set; }
        [Display(Name = "Client Rating")]
        public String ClientRating { get; set; }

        public ICollection<Renting> Rentings { get; set; }
        public ICollection<Movie> Movies { get; set; }
    }
}
