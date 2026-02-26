namespace GraduationProject.Application.Tokens.DTOs
{
    public class NewTokens
    {
        public TokensResult? Result { get; set; }
        public string? Error { get; set; }
        public bool Success { get; set; }
        public NewTokens(bool success = false, string? Error = null, TokensResult? tokens = null)
        {
            this.Error = Error;
            Result = tokens;
            Success = success;
        }
        public NewTokens Errors(string Error) => new(false, Error: Error);
        public NewTokens GetNewTokens(TokensResult token) => new(true, Error: null, token);

    }
}
