

using System.ComponentModel.DataAnnotations;

namespace TestRESTAPI.DTO
{
    public class dtoLogin
    {

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
