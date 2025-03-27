namespace PostsProject.Data.Helper
{
    public class JwtOptions
    {
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public int LifeTime { get; set; }
        public string SigningKey { get; set; } = null!;
        public int RefreshTokenLifeTime { get; set; }
    }
}
