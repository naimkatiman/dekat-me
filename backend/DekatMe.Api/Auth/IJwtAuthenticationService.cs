namespace DekatMe.Api.Auth
{
    public interface IJwtAuthenticationService
    {
        Task<AuthResponse> AuthenticateAsync(string email, string password);
        Task<AuthResponse> RefreshTokenAsync(string token, string refreshToken);
        Task<bool> RevokeTokenAsync(string userId);
    }

    public class AuthResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public List<string>? Roles { get; set; }
    }

    public class JwtSettings
    {
        public string Secret { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int TokenValidityInMinutes { get; set; } = 60;
        public int RefreshTokenValidityInDays { get; set; } = 7;
        public bool RequireEmailConfirmation { get; set; } = true;
    }
}
