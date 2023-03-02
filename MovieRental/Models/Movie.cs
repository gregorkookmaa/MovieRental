using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieRental.Models
{
    public enum MovieGenre
    {
        Action, Comedy, Sci_fi, Horror, Fantasy, Thriller
    }
    public class Movie : Entity
    {
        [Required]
        [Display(Name = "Movies name")]
        public string Title { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Release date")]
        public DateTime ReleaseDate { get; set; }

        [Display(Name = "Movie genre")]
        public MovieGenre? MovieGenre { get; set; }

        public ICollection<RentingMovie> RentingMovies { get; set; }

    }
}
