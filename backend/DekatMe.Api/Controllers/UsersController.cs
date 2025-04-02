using DekatMe.Api.Models;
using DekatMe.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DekatMe.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        
        // GET: api/users
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
        
        // GET: api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationUser>> GetUser(string id)
        {
            // Allow users to access their own profile or admins to access any profile
            string userId = User.FindFirst("sub")?.Value ?? User.Identity?.Name ?? string.Empty;
            if (id != userId && !User.IsInRole("Admin"))
                return Forbid();
                
            var user = await _userService.GetUserByIdAsync(id);
            
            if (user == null)
                return NotFound();
                
            return Ok(user);
        }
        
        // GET: api/users/email/user@example.com
        [HttpGet("email/{email}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApplicationUser>> GetUserByEmail(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            
            if (user == null)
                return NotFound();
                
            return Ok(user);
        }
        
        // PUT: api/users/5/profile
        [HttpPut("{id}/profile")]
        public async Task<IActionResult> UpdateUserProfile(string id, ApplicationUser userProfile)
        {
            // Allow users to update their own profile or admins to update any profile
            string userId = User.FindFirst("sub")?.Value ?? User.Identity?.Name ?? string.Empty;
            if (id != userId && !User.IsInRole("Admin"))
                return Forbid();
                
            var result = await _userService.UpdateUserProfileAsync(id, userProfile);
            
            if (!result)
                return NotFound();
                
            return NoContent();
        }
        
        // POST: api/users/5/favorites/add/10
        [HttpPost("{userId}/favorites/add/{businessId}")]
        public async Task<IActionResult> AddBusinessToFavorites(string userId, string businessId)
        {
            // Allow users to modify their own favorites or admins to modify any user's favorites
            string currentUserId = User.FindFirst("sub")?.Value ?? User.Identity?.Name ?? string.Empty;
            if (userId != currentUserId && !User.IsInRole("Admin"))
                return Forbid();
                
            var result = await _userService.AddBusinessToFavoritesAsync(userId, businessId);
            
            if (!result)
                return NotFound();
                
            return NoContent();
        }
        
        // POST: api/users/5/favorites/remove/10
        [HttpPost("{userId}/favorites/remove/{businessId}")]
        public async Task<IActionResult> RemoveBusinessFromFavorites(string userId, string businessId)
        {
            // Allow users to modify their own favorites or admins to modify any user's favorites
            string currentUserId = User.FindFirst("sub")?.Value ?? User.Identity?.Name ?? string.Empty;
            if (userId != currentUserId && !User.IsInRole("Admin"))
                return Forbid();
                
            var result = await _userService.RemoveBusinessFromFavoritesAsync(userId, businessId);
            
            if (!result)
                return NotFound();
                
            return NoContent();
        }
        
        // GET: api/users/5/favorites
        [HttpGet("{userId}/favorites")]
        public async Task<ActionResult<IEnumerable<Business>>> GetUserFavorites(string userId)
        {
            // Allow users to access their own favorites or admins to access any user's favorites
            string currentUserId = User.FindFirst("sub")?.Value ?? User.Identity?.Name ?? string.Empty;
            if (userId != currentUserId && !User.IsInRole("Admin"))
                return Forbid();
                
            var favorites = await _userService.GetUserFavoriteBusinessesAsync(userId);
            return Ok(favorites);
        }
    }
}
