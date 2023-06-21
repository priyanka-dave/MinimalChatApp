using System.ComponentModel.DataAnnotations;

namespace MinimalChatApp.Dtos
{
    public class UserDto
    {

       
        public string UserName { get; set; }
      
        public string UserEmail { get; set; }
      
        public string UserPassword { get; set; }
    }
}
