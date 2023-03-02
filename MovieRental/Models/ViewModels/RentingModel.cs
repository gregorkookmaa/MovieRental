using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MovieRental.Models.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class RentingModel
    {
        public int ID { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Renting date")]
        public DateTime Date { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Renting Expires")]
        public DateTime Expires { get; set; }
        public int ClientID { get; set; }
        [Display(Name = "Client name")]
        public string ClientName { get; set; }

        public ICollection<RentingMovie> RentingMovies { get; set; }
        public ICollection<Client> Clients { get; set; }
        public ICollection<Movie> Movies { get; set; }
    }
}
