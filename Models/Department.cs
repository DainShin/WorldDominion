using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;

namespace WorldDominion.Models
{
    public class Department
    {
        [Key] // data annotaion (specifies this is a primary key)
        public int Id {get; set;} = 0; // Auto Increment  자동으로 생성됨

        [Required, StringLength(300)]
        public string Name {get; set;} = String.Empty;

        [StringLength(1000)]
        public string? Description {get; set;} = String.Empty;

        // Relationship with Products and place to store product
        // ICollection : Alias for List
        public virtual ICollection<Product> Products {get; set;} = new List<Product>();

    }
}