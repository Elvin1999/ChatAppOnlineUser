using ChatApp.Entities;

namespace ChatApp.Helpers
{
    public class UserHelper
    {
        public static CustomIdentityUser CurrentUser { get; set; }
        public static CustomIdentityUser ReceiverUser { get; set; }
        public static List<CustomIdentityUser> ActiveUsers { get; set; } = new List<CustomIdentityUser>();
    }
}
