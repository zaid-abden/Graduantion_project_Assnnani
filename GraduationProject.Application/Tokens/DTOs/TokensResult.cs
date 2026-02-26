namespace GraduationProject.Application.Tokens.DTOs
{
    public class TokensResult
    {
        public string Token { get; set; }
        public RefreshToken refreshToken { get; set; }
    }
    public class RefreshToken
    {
        public string UserName { get; set; }
        public string RefToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}
