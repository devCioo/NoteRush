using BitzArt.Blazor.Auth;
using BitzArt.Blazor.Auth.Server;
using Microsoft.IdentityModel.Tokens;
using NoteRush.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NoteRush.Services
{
	public class NoteRushAuthenticationService : AuthenticationService<SignInPayload>
	{
		private const string AUTH_KEY = "n0t3ru5hu53r4uth3nt1c4t10n53cr3tk3y";
		private readonly SigningCredentials _signingCredentials;
		private readonly JwtSecurityTokenHandler _tokenHandler;

		public NoteRushAuthenticationService()
		{
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AUTH_KEY));
			_signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
			_tokenHandler = new JwtSecurityTokenHandler();
		}

		public override Task<AuthenticationResult> SignInAsync(SignInPayload signInPayload, CancellationToken cancellationToken = default)
		{
			var jwtPair = BuildJwtPair(signInPayload.Username, signInPayload.Role);
			var authResult = Success(jwtPair);

			return Task.FromResult(authResult);
		}

		public override Task<AuthenticationResult> RefreshJwtPairAsync(string refreshToken, CancellationToken cancellationToken = default)
		{
			var token = _tokenHandler.ReadJwtToken(refreshToken);
			var username = token.Claims.First(c => c.Type == ClaimTypes.Name).Value;
			var role = token.Claims.First(c => c.Type == ClaimTypes.Role).Value;

			var jwtPair = BuildJwtPair(username, role);
			var authResult = Success(jwtPair);

			return Task.FromResult(authResult);
		}

		private JwtPair BuildJwtPair(string username, string role)
		{
			var now = DateTime.UtcNow;

			var accessTokenDuration = TimeSpan.FromMinutes(1);
			var accessTokenExpiresAt = now + accessTokenDuration;
			var accessToken = _tokenHandler.WriteToken(
				new JwtSecurityToken(
					claims:
					[
						new Claim(ClaimTypes.Name, username),
						new Claim(ClaimTypes.Role, role)
					],
					notBefore: now,
					expires: accessTokenExpiresAt,
					signingCredentials: _signingCredentials
			));

			var refreshTokenDuration = TimeSpan.FromHours(1);
			var refreshTokenExpiresAt = now + refreshTokenDuration;
			var refreshToken = _tokenHandler.WriteToken(
				new JwtSecurityToken(
					claims:
					[
						new Claim(ClaimTypes.Name, username),
						new Claim(ClaimTypes.Role, role)
					],
					notBefore: now,
					expires: refreshTokenExpiresAt,
					signingCredentials: _signingCredentials
			));

			return new JwtPair(accessToken, accessTokenExpiresAt, refreshToken, refreshTokenExpiresAt);
		}
	}
}