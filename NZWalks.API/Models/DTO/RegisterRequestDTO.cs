using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class RegisterRequestDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; }

        public string[] Roles { get; set; } = new string[] { };

    }
}
