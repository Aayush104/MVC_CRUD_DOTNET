namespace DigitalApp2.Models
{
    public class UserEdit
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;

        public string UserPassword { get; set; } = null!;

        public string EmailAddress { get; set; } = null!;

        public string UserProfile { get; set; } = null!;

        public bool UserStatus { get; set; }
        public IFormFile? UserFile { get; set; } 
        public string  EncId { get; set; } = string.Empty;
    }
}
