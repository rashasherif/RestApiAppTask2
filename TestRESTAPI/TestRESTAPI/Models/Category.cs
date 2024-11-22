using System.ComponentModel.DataAnnotations;

namespace TestRESTAPI.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }

        public ICollection<Product> products { get; set; }

    }
}
