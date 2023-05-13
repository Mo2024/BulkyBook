using System.ComponentModel.DataAnnotations;

namespace BulkyBookWeb.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public bool IsEmailVerified { get; set; }
        public Guid EmailVerificationToken { get; set; }



    }
}
