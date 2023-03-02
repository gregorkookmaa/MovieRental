using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MovieRental.Models.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class MoviesEditModel
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
    }
}
