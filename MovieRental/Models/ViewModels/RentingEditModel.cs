using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MovieRental.Models.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class RentingEditModel
    {
        public int ID { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Renting date")]
        public DateTime Date { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Renting Expires")]
        public DateTime Expires { get; set; }
        public int ClientID { get; set; }
        public IList<SelectListItem> Clients { get; set; }
    }
}
