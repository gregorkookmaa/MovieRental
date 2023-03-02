using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MovieRental.Models.ViewModels
{
    public enum MovieGenre
    {
        Action, Comedy, Sci_fi, Horror, Fantasy, Thriller
    }

    [ExcludeFromCodeCoverage]
    public class MoviesModel
    {
        public int ID { get; set; }
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
