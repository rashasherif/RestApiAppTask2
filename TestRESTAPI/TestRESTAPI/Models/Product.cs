using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestRESTAPI.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [ForeignKey("category")]
        public int CategoryId { get; set; }
        public Category category { get; set; }

    }
}
