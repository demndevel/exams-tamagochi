using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Exagochi.Api.Common;
using Exagochi.Api.Models.User;
using Exagochi.Api.Persistence;
using Exagochi.Api.Persistence.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Exagochi.Api.Controllers;

[ApiController]
[Route("user")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly AppDbContext _db;

    public UserController(ILogger<UserController> logger, AppDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCredentialsModel credentials)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Username == credentials.Username);
        
        if (user is null || user.Password != credentials.Password)
        {
            _logger.LogWarning("User tried to log in with invalid creds");
            return Unauthorized("Credentials are incorrect");
        }

        var claims = new List<Claim>
        {
            new (ClaimTypes.Name, user.Username)
        };

        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.Issuer,
            audience: AuthOptions.Audience,
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: new SigningCredentials(
                AuthOptions.GetSymmetricSecurityKey(), 
                SecurityAlgorithms.HmacSha256));
        
        _logger.LogInformation("User logged in successfully");

        return Ok(new JwtSecurityTokenHandler().WriteToken(jwt));
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        if (await _db.Users.AnyAsync(u => u.Username == model.Username))
        {
            _logger.LogWarning("User already exists");
            return Conflict("User already exists");
        }
        
        var user = new User
        {
            Username = model.Username,
            Password = model.Password
        };
        
        await _db.Users.AddAsync(user);
        
        await _db.SaveChangesAsync();
        
        _logger.LogInformation("User registered successfully with username {Username}", model.Username);
        return Ok("Registered successfully");
    }
}