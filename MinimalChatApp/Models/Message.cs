using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalChatApp.Models
{
    public class Message
    {
        [Required]
        public int MessageId { get; set; }

        [ForeignKey("UserId")]
        public User SenderId { get; set; }

        [ForeignKey("UserId")]
        public User ReceiverId { get; set; }
        public string Content { get; set; }
        public DateTime TimeStamp { get; set; }= DateTime.Now;
    }
}
