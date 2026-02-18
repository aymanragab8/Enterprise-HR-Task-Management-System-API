namespace WebApplication2.Dtos.Auth
{
    public class LoginDetailsDto
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
