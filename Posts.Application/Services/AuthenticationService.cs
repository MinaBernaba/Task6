using PostsProject.Application.Features.Authentication.Responses;
using PostsProject.Application.ServiceInterfaces;
using PostsProject.Data.Helper;
using PostsProject.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PostsProject.Application.Services
{
    public class AuthenticationService(IOptionsMonitor<JwtOptions> _jwtOptions, UserManager<User> userManager) : IAuthenticationService
    {
        private RefreshToken CreateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);

            string token = Convert.ToBase64String(randomNumber);

            return new RefreshToken()
            {
                Token = token,
                ExpiresOn = DateTime.UtcNow.AddDays(_jwtOptions.CurrentValue.RefreshTokenLifeTime),
                CreatedOn = DateTime.UtcNow
            };
        }
        private async Task<string> CreateJWTTokenAsync(User user)
        {
            var userClaims = await userManager.GetClaimsAsync(user);

            var roles = await userManager.GetRolesAsync(user);

            var rolesClaims = new List<Claim>();

            foreach (var role in roles)
                rolesClaims.Add(new Claim(ClaimTypes.Role, role));

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier , user.Id.ToString()),
                new Claim(ClaimTypes.Name , user.FullName),
                new Claim("username" ,  user.UserName!),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!)
            }
            .Union(userClaims)
            .Union(rolesClaims);


            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = _jwtOptions.CurrentValue.Issuer,
                Audience = _jwtOptions.CurrentValue.Audience,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(_jwtOptions.CurrentValue.LifeTime),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.CurrentValue.SigningKey)), SecurityAlgorithms.HmacSha256)
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);
            return accessToken;
        }
        public async Task<JwtAuthResponse> LoginUser(User user)
        {
            string jwtToken = await CreateJWTTokenAsync(user);

            var rolesList = await userManager.GetRolesAsync(user);

            var authResponse = new JwtAuthResponse()
            {
                UserName = user.UserName!,
                Email = user.Email!,
                Token = jwtToken,
                Roles = rolesList.ToList()
            };


            // chech if user already has an active refresh token
            var activeRefreshToken = user.RefreshTokens.FirstOrDefault(x => x.IsActive);

            if (activeRefreshToken != null)
            {
                activeRefreshToken.ExpiresOn = activeRefreshToken.ExpiresOn.AddDays(_jwtOptions.CurrentValue.LifeTime * 10);
                authResponse.RefreshToken = activeRefreshToken.Token;
                authResponse.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
            }

            else
            {
                var newRefreshToken = CreateRefreshToken();
                authResponse.RefreshToken = newRefreshToken.Token;
                authResponse.RefreshTokenExpiration = newRefreshToken.ExpiresOn;
                user.RefreshTokens.Add(newRefreshToken);
            }

            await userManager.UpdateAsync(user);

            return authResponse;
        }
        public async Task<JwtAuthResponse?> RenewTokensAsync(string refreshToken)
        {
            var user = await userManager.Users
                .SingleOrDefaultAsync(x => x.RefreshTokens.Any(rt => rt.Token == refreshToken));

            if (user == null)
                return null;

            var userRefreshToken = user.RefreshTokens.Single(x => x.Token.Equals(refreshToken));


            if (!userRefreshToken.IsActive)
                return null;


            userRefreshToken.RevokedOn = DateTime.UtcNow;

            var newRefreshToken = CreateRefreshToken();

            user.RefreshTokens.Add(newRefreshToken);

            await userManager.UpdateAsync(user);

            string jwtToken = await CreateJWTTokenAsync(user);

            var roles = await userManager.GetRolesAsync(user);

            return new JwtAuthResponse()
            {
                Token = jwtToken,
                Email = user.Email!,
                UserName = user.UserName!,
                Roles = roles.ToList(),
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpiration = newRefreshToken.ExpiresOn
            };
        }
        public async Task<bool> RevokeRefreshTokenAsync(string refreshToken)
        {
            var user = await userManager.Users.SingleOrDefaultAsync(x => x.RefreshTokens.Any(rt => rt.Token == refreshToken));

            if (user == null)
                return false;

            var userRefreshToken = user.RefreshTokens.Single(x => x.Token.Equals(refreshToken));


            if (!userRefreshToken.IsActive)
                return false;

            userRefreshToken.RevokedOn = DateTime.UtcNow;

            await userManager.UpdateAsync(user);

            return true;
        }

    }
}