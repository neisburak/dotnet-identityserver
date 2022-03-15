namespace CustomMembershipAuthServer.Models
{
    public class AppUser
    {
        public int Id { get; set; }
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string City { get; set; } = default!;
    }
}