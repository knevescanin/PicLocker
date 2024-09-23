using BlazorApp.Areas.Identity.Data;

namespace BlazorApp.Models
{
    public class ImageFile
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime DateUploaded { get; set; }
         public ApplicationUser User { get; set; }

    }
}