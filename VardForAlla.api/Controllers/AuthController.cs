using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VardForAlla.api.Dtos;
using VardForAlla.Api.Dtos;
using VardForAlla.Application.Interfaces;

namespace VardForAlla.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto loginDto)
    {
        var (success, token, user) = await _authService.LoginAsync(loginDto.Email, loginDto.Password);

        if (!success || user == null)
        {
            return Unauthorized(new { message = "Felaktig email eller lösenord" });
        }

        var roles = await _authService.GetUserRolesAsync(user.Id);

        return Ok(new LoginResponseDto
        {
            Token = token,
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = roles
            }
        });
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterUserDto registerDto)
    {
        var (success, message) = await _authService.RegisterUserAsync(
            registerDto.Email,
            registerDto.Password,
            registerDto.FirstName,
            registerDto.LastName);

        if (!success)
        {
            return BadRequest(new { message });
        }

        return Ok(new { message });
    }

    [HttpPost("request-password-reset")]
    public async Task<ActionResult> RequestPasswordReset([FromBody] RequestPasswordResetDto requestDto)
    {
        var (success, message) = await _authService.RequestPasswordResetAsync(requestDto.Email);
        return Ok(new { message });
    }

    [HttpPost("reset-password")]
    public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordDto resetDto)
    {
        var (success, message) = await _authService.ResetPasswordAsync(
            resetDto.Token,
            resetDto.NewPassword);

        if (!success)
        {
            return BadRequest(new { message });
        }

        return Ok(new { message });
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await _authService.GetUserByIdAsync(userId);

        if (user == null)
        {
            return NotFound();
        }

        var roles = await _authService.GetUserRolesAsync(userId);

        return Ok(new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Roles = roles
        });
    }
}
