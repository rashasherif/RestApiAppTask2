using System.ComponentModel.DataAnnotations;

namespace TestRESTAPI.DTO
{
    public class dtoNewUser
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]

        public string Email { get; set; }


        public string? phoneNumber { get; set; }


    }
}
