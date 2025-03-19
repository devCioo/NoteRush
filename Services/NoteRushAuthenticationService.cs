using BitzArt.Blazor.Auth;
using BitzArt.Blazor.Auth.Server;
using NoteRush.Authentication;

namespace NoteRush.Services
{
	public class NoteRushAuthenticationService(JwtService jwtService) : AuthenticationService<SignInPayload>
	{
		public override Task<AuthenticationResult> SignInAsync(SignInPayload signInPayload, CancellationToken cancellationToken = default)
		{
			var jwtPair = jwtService.BuildJwtPair();
			var authResult = Success(jwtPair);

			return Task.FromResult(authResult);
		}

		public override Task<AuthenticationResult> RefreshJwtPairAsync(string refreshToken, CancellationToken cancellationToken = default)
		{
			var jwtPair = jwtService.BuildJwtPair();
			var authResult = Success(jwtPair);

			return Task.FromResult(authResult);
		}
	}
}