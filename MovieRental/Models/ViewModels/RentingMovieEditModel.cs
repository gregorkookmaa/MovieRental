using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MovieRental.Models.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class RentingMovieEditModel
    {
        public int ID { get; set; }
        public int RentingID { get; set; }
        public int MovieID { get; set; }
        [StringLength(10)]
        [Display(Name = "Client Rating")]
        public String ClientRating { get; set; }
        public IList<SelectListItem> Rentings { get; set; }
        public IList<SelectListItem> Movies { get; set; }
    }
}
