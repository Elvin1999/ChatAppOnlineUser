using ChatApp.Entities;

namespace ChatApp.Models
{
    public class ChatViewModel
    {
        public List<Chat> Chats { get; set; }
        public Chat ChatCurrent { get; set; }
    }
}
