using System.ComponentModel.DataAnnotations;

namespace MovieRental.Models
{
    public abstract class Entity
    {
        [Key]
        public int ID { get; set; }
    }
}
