using BitzArt.Blazor.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NoteRush.Services
{
	public class JwtService
	{
		private const string AUTH_KEY = "n0t3ru5hu53r4uth3nt1c4t10n53cr3tk3y";
		private readonly SigningCredentials _signingCredentials;
		private readonly JwtSecurityTokenHandler _tokenHandler;

		public JwtService(IConfiguration configuration)
		{
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AUTH_KEY));
			_signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
			_tokenHandler = new JwtSecurityTokenHandler();
		}

		public JwtPair BuildJwtPair()
		{
			var now = DateTime.UtcNow;

			var accessTokenDuration = TimeSpan.FromMinutes(1);
			var accessTokenExpiresAt = now + accessTokenDuration;
			var accessToken = _tokenHandler.WriteToken(
				new JwtSecurityToken(
					claims: [],
					notBefore: now,
					expires: accessTokenExpiresAt,
					signingCredentials: _signingCredentials
			));

			var refreshTokenDuration = TimeSpan.FromHours(1);
			var refreshTokenExpiresAt = now + refreshTokenDuration;
			var refreshToken = _tokenHandler.WriteToken(
				new JwtSecurityToken(
					notBefore: now,
					expires: refreshTokenExpiresAt,
					signingCredentials: _signingCredentials
			));

			return new JwtPair(accessToken, accessTokenExpiresAt, refreshToken, refreshTokenExpiresAt);
		}
	}
}