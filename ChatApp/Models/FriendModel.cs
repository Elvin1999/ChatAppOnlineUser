using ChatApp.Entities;

namespace ChatApp.Models
{
    public class FriendModel
    {
        public int Id { get; set; }
        public string OwnId { get; set; }
        public string YourFriendId { get; set; }
        public virtual CustomIdentityUser YourFriend { get; set; }
        public int LastMessageCount { get; set; } = 0;
    }
}
