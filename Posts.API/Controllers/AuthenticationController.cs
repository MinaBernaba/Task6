using PostsProject.api.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PostsProject.Application.Features.Authentication.Models;

namespace PostsProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(IMediator _mediator) : AppControllerBase
    {
        #region Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserCommand loginUser)
        {
            var response = await _mediator.Send(loginUser);

            if (response.Data != null)
                SetRefreshTokenInCookie(response.Data.RefreshToken, response.Data.RefreshTokenExpiration);
            else
                DeleteRefreshTokenFromCookie();

            return NewResult(response);
        }
        #endregion

        #region Resigter User
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser(RegisterUserCommand registerUser)
        {
            var response = await _mediator.Send(registerUser);
            if (response.Data != null)
                SetRefreshTokenInCookie(response.Data.RefreshToken, response.Data.RefreshTokenExpiration);

            return Ok(response);
        }
        #endregion

        #region Renew Tokens
        [HttpPost("RenewTokens")]
        public async Task<IActionResult> RenewTokens()
        {
            var refreshToken = Request.Cookies["RefreshToken"];

            if (string.IsNullOrWhiteSpace(refreshToken))
                return BadRequest("No refresh token provided!");

            var response = await _mediator.Send(new RenewTokensCommand() { RefreshToken = refreshToken });

            if (response.Data != null)
                SetRefreshTokenInCookie(response.Data.RefreshToken, response.Data.RefreshTokenExpiration);
            else
                DeleteRefreshTokenFromCookie();

            return NewResult(response);
        }
        #endregion

        #region Revoke Refresh Token
        [HttpPost("RevokeRefreshToken")]
        public async Task<IActionResult> RevokeRefreshToken([FromBody] RevokeRefreshTokenCommand? revokeToken)
        {
            var refreshToken = revokeToken?.RefreshToken ?? Request.Cookies["RefreshToken"];
            if (string.IsNullOrWhiteSpace(refreshToken))
                return BadRequest("Refresh Token is required!");

            return NewResult(await _mediator.Send(new RevokeRefreshTokenCommand() { RefreshToken = refreshToken }));
        }
        #endregion

        #region Set Refresh Token In Cookie
        private void SetRefreshTokenInCookie(string refreshToken, DateTime expiresOn)
        {
            var cookieOptions = new CookieOptions()
            {
                HttpOnly = true,
                Expires = expiresOn.ToLocalTime()
            };
            Response.Cookies.Append("RefreshToken", refreshToken, cookieOptions);
        }
        #endregion

        #region  Delete Refresh Token In Cookie
        private void DeleteRefreshTokenFromCookie()
        {
            var cookieOptions = new CookieOptions()
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(-1)
            };
            Response.Cookies.Append("RefreshToken", "", cookieOptions);
        }
        #endregion
    }
}