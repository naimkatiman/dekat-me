using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DekatMe.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DekatMe.Api.Auth
{
    public class JwtAuthenticationService : IJwtAuthenticationService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<JwtAuthenticationService> _logger;

        public JwtAuthenticationService(
            IOptions<JwtSettings> jwtSettings,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<JwtAuthenticationService> logger)
        {
            _jwtSettings = jwtSettings.Value;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task<AuthResponse> AuthenticateAsync(string email, string password)
        {
            // Find user by email
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("Authentication failed: User not found with email {Email}", email);
                return new AuthResponse { Success = false, Message = "Invalid email or password." };
            }

            // Check if the email is confirmed (if required)
            if (_jwtSettings.RequireEmailConfirmation && !user.EmailConfirmed)
            {
                _logger.LogWarning("Authentication failed: Email not confirmed for user {Email}", email);
                return new AuthResponse { Success = false, Message = "Please confirm your email before logging in." };
            }

            // Verify password
            if (!await _userManager.CheckPasswordAsync(user, password))
            {
                _logger.LogWarning("Authentication failed: Invalid password for user {Email}", email);
                // Increment failed access attempts
                await _userManager.AccessFailedAsync(user);
                return new AuthResponse { Success = false, Message = "Invalid email or password." };
            }

            // Check if the account is locked out
            if (await _userManager.IsLockedOutAsync(user))
            {
                var lockoutEnd = await _userManager.GetLockoutEndDateAsync(user);
                _logger.LogWarning("Authentication failed: User {Email} is locked out until {LockoutEnd}", email, lockoutEnd);
                return new AuthResponse
                {
                    Success = false,
                    Message = $"Your account is locked out. Try again after {lockoutEnd}."
                };
            }

            // Reset the access failed count
            await _userManager.ResetAccessFailedCountAsync(user);

            // Get user roles
            var userRoles = await _userManager.GetRolesAsync(user);

            // Generate the JWT token
            var token = await GenerateJwtTokenAsync(user, userRoles);

            // Generate refresh token
            var refreshToken = GenerateRefreshToken();

            // Save refresh token to user
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenValidityInDays);
            await _userManager.UpdateAsync(user);

            // Update last login date
            user.LastLoginDate = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            _logger.LogInformation("User {Email} authenticated successfully", email);

            return new AuthResponse
            {
                Success = true,
                Token = token,
                RefreshToken = refreshToken,
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Roles = userRoles.ToList()
            };
        }

        public async Task<AuthResponse> RefreshTokenAsync(string token, string refreshToken)
        {
            var principal = GetPrincipalFromExpiredToken(token);
            if (principal == null)
            {
                _logger.LogWarning("Refresh token failed: Unable to get principal from expired token");
                return new AuthResponse { Success = false, Message = "Invalid token" };
            }

            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                _logger.LogWarning("Refresh token failed: No user ID claim found in token");
                return new AuthResponse { Success = false, Message = "Invalid token" };
            }

            var user = await _userManager.FindByIdAsync(userIdClaim.Value);
            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                if (user == null)
                {
                    _logger.LogWarning("Refresh token failed: User not found with ID {UserId}", userIdClaim.Value);
                }
                else if (user.RefreshToken != refreshToken)
                {
                    _logger.LogWarning("Refresh token failed: Refresh token does not match for user {UserId}", userIdClaim.Value);
                }
                else
                {
                    _logger.LogWarning("Refresh token failed: Refresh token expired for user {UserId}", userIdClaim.Value);
                }

                return new AuthResponse { Success = false, Message = "Invalid client request" };
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var newToken = await GenerateJwtTokenAsync(user, userRoles);
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenValidityInDays);
            await _userManager.UpdateAsync(user);

            _logger.LogInformation("User {UserId} refreshed token successfully", userIdClaim.Value);

            return new AuthResponse
            {
                Success = true,
                Token = newToken,
                RefreshToken = newRefreshToken,
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Roles = userRoles.ToList()
            };
        }

        public async Task<bool> RevokeTokenAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("Revoke token failed: User not found with ID {UserId}", userId);
                return false;
            }

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            _logger.LogInformation("Token revoked for user {UserId}", userId);
            return true;
        }

        private async Task<string> GenerateJwtTokenAsync(ApplicationUser user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            // Add role claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));

                // Add role-specific permissions if needed
                var roleEntity = await _roleManager.FindByNameAsync(role);
                if (roleEntity != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(roleEntity);
                    claims.AddRange(roleClaims);
                }
            }

            // Add user claims
            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.TokenValidityInMinutes),
                SigningCredentials = credentials,
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                ValidateLifetime = false // This is important for expired tokens
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
                
                if (securityToken is not JwtSecurityToken jwtSecurityToken || 
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }

                return principal;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating token");
                return null;
            }
        }
    }
}
