using Microsoft.AspNetCore.Identity;

namespace PostsProject.Data.Identity
{
    public class User : IdentityUser<int>
    {
        public string FullName { get; set; } = null!;
        public string? Address { get; set; } = null!;
        public string? Country { get; set; }
        public virtual List<RefreshToken> RefreshTokens { get; set; } = new();
    }
}
