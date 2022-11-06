using Microsoft.AspNetCore.Identity;

namespace ChatApp.Entities
{
    public class CustomIdentityUser : IdentityUser
    {
        public CustomIdentityUser()
        {
            Messages = new List<Message>();
            Friends = new List<CustomIdentityUser>();
            FriendRequests = new List<FriendRequest>();
        }
        public virtual ICollection<Message> Messages { get; set; }
        public List<CustomIdentityUser> Friends { get; set; }
        public virtual ICollection<FriendRequest> FriendRequests { get; set; }
        public string ImageUrl { get; set; }
        public bool IsOnline { get; set; }
        public bool IsFriend { get; set; } = false;
        public DateTime DisConnectTime { get; set; } = DateTime.Now;

    }
}
