namespace NoteRush.Authentication
{
	public class SignInPayload
	{
		public string Username { get; set; }
		public string Role { get; set; }

		public SignInPayload(string username, string role)
		{
			Username = username;
			Role = role;
		}
	}
}