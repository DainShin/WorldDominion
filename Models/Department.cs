using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;

namespace WorldDominion.Models
{
    public class Department
    {
        [Key] // data annotaion (specifies this is a primary key)
        public int Id {get; set;} = 0;

        [Required, StringLength(300)]
        public string Name {get; set;} = String.Empty;

        [StringLength(1000)]
        public string? Description {get; set;} = String.Empty;

        // Relationship with Products and place to store product
        public virtual ICollection<Product>? Products {get; set;}

    }
}