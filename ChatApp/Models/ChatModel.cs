using ChatApp.Entities;

namespace ChatApp.Models
{
    public class ChatModel
    {
        public CustomIdentityUser ReceiverUser { get; set; }
        public CustomIdentityUser SenderUser { get; set; }
    }
}
