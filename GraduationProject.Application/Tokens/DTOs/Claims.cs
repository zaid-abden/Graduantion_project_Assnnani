namespace GraduationProject.Application.Tokens.DTOs
{
    public class Claims
    {
        public int userid { get; set; }
        public List<userclim> Uerclaims { get; set; }
    }
    public class userclim
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
