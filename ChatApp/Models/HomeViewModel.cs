using ChatApp.Entities;

namespace ChatApp.Models
{
    public class HomeViewModel
    {
        public List<CustomIdentityUser> AllUsers { get; set; }
        public List<CustomIdentityUser> ActiveUsers { get; set; }
    }
}
