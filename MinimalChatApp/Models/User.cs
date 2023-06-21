using System.ComponentModel.DataAnnotations;

namespace MinimalChatApp.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string UserName { get; set; }
        [Required]
        public string UserEmail { get; set; }

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        //[Required]
        //public string UserPassword { get; set; }
    }
}
