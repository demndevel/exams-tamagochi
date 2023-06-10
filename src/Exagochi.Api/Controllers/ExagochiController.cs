using Exagochi.Api.Models.Exagochi;
using Exagochi.Api.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Exagochi.Api.Controllers;

[ApiController]
[Route("exagochi")]
public class ExagochiController : ControllerBase
{
    private readonly ILogger<ExagochiController> _logger;
    private readonly AppDbContext _db;

    public ExagochiController(ILogger<ExagochiController> logger, AppDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateExagochi([FromBody] CreateExagochiModel model)
    {
        var username = User.Identity!.Name;
        
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
        
        if (user?.Exagochi is not null)
        {
            _logger.LogWarning("User already has an exagochi");
            
            return Conflict("User already has an exagochi");
        }

        var exagochi = new Persistence.Entities.Exagochi
        {
            Name = model.Name,
            Satiety = 0,
            LastMeal = DateTime.UtcNow,
            Points = 0,
            HardnessStep = model.HardnessStep,
            Level = 1
        };

        await _db.Exagochis.AddAsync(exagochi);
        user!.Exagochi = exagochi;
        
        await _db.SaveChangesAsync();
        
        return Ok("Successfully created exagochi with ID " + exagochi.Id);
    }
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetExagochi()
    {
        var username = User.Identity!.Name;
        
        var user = await _db.Users
            .Include(u => u.Exagochi)
            .FirstOrDefaultAsync(u => u.Username == username);
        
        if (user?.Exagochi is null)
        {
            _logger.LogWarning($"User {username} does not have an exagochi");
            
            return BadRequest("User does not have an exagochi");
        }
        
        return Ok(user.Exagochi);
    }
}