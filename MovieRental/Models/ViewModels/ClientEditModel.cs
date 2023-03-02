using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MovieRental.Models.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class ClientEditModel
    {
        public int ID { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Birthday")]
        public DateTime DateOfBirth { get; set; }
        [StringLength(20)]
        public String Phone { get; set; }
        [StringLength(50)]
        [Display(Name = "E-mail")]
        public String Email { get; set; }

        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Display(Name = "Membership")]
        public string Membership { get; set; }

        [Display(Name = "Additional info")]
        public String AdditionalInfo { get; set; }
    }
}
