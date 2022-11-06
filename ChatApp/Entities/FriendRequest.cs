namespace ChatApp.Entities
{
    public class FriendRequest
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string SenderId { get; set; }
        public virtual CustomIdentityUser CustomIdentityUser { get; set; }
    }
}
