using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VideoPlayer.Models
{
    public class HomeModel : PageModel
    {
        public string LocatorId { get; set; }
        public string VideoUrl { get; set; }
        public string Token { get; set; }
    }
}