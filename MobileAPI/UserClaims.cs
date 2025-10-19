namespace MobileAPI
{
    public class UserClaims
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public bool EmailVerified { get; set; }
        public string Username { get; set; }
        public string Nickname { get; set; }
        public string[] Groups { get; set; }
    }
}