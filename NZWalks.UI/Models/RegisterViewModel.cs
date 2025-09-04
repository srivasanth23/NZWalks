using System.ComponentModel.DataAnnotations;

namespace NZWalks.UI.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; }

        // Optional: default role (so API won’t break)
        public string[] Roles { get; set; } = new string[] { "User" };
    }
}
