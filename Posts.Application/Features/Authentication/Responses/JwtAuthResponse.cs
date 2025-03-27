using System.Text.Json.Serialization;

namespace PostsProject.Application.Features.Authentication.Responses
{
    public class JwtAuthResponse
    {
        public string Token { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public List<string>? Roles { get; set; }

        [JsonIgnore]
        public string RefreshToken { get; set; } = null!;
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
