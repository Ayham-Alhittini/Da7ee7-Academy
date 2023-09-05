using Microsoft.AspNetCore.Identity;

namespace Da7ee7_Academy.Entities
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string UserPhotoUrl { get; set; }
        public string FileId { get; set; }
        public AppFile File { get; set; }
        public string Role { get; set; }
        public string FullName { get; set; }
    }
}
