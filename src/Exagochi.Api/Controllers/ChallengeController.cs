using Exagochi.Api.Common;
using Exagochi.Api.Models.Challenge;
using Exagochi.Api.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Exagochi.Api.Controllers;

[ApiController]
[Route("challenge")]
public class ChallengeController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly ILogger<ChallengeController> _logger;

    public ChallengeController(
        AppDbContext db, 
        ILogger<ChallengeController> logger)
    {
        _db = db;
        _logger = logger;
    }

    [HttpGet]
    [Route("getRandomChallenge/{subject}/{hardness:int}")]
    public async Task<IActionResult> GetRandomChallenge(string subject, byte hardness)
    {
        var challengeCount = await _db.Challenges
            .CountAsync(c => c.Hardness == hardness);
        
        if (challengeCount == 0)
        {
            _logger.LogWarning("No challenges found");
            return NotFound("No challenges found");
        }
        
        var challenge = _db.Challenges
            .Where(c => c.Hardness == hardness)
            .Where(c => c.Subject == subject)
            .AsEnumerable()
            .Shuffle(new Random())
            .FirstOrDefault();
        
        return Ok(challenge);
    }
    
    [HttpPost("submitChallenge")]
    [Authorize]
    public async Task<IActionResult> SubmitChallenge([FromBody] SubmitChallengeModel model)
    {
        var username = User.Identity!.Name;
        
        var user = await _db.Users
            .Include(u => u.Exagochi)
            .FirstOrDefaultAsync(u => u.Username == username);
        
        if (user?.Exagochi is null)
        {
            _logger.LogWarning("User does not have an exagochi");
            
            return Conflict("User does not have an exagochi");
        }
        
        var challenge = await _db.Challenges.FirstOrDefaultAsync(c => c.Id == model.ChallengeId);
        
        if (challenge is null)
        {
            _logger.LogWarning("Challenge not found");
            
            return NotFound("Challenge not found");
        }
        
        if (challenge.Answer != model.Answer)
        {
            _logger.LogWarning("Wrong answer");
            
            return BadRequest("Wrong answer");
        }
        
        if (user.Exagochi.Satiety + user.Exagochi.HardnessStep >= ExagochiConstants.MaxSatiety)
            user.Exagochi.Satiety = ExagochiConstants.MaxSatiety;
        else
            user.Exagochi.Satiety += user.Exagochi.HardnessStep;

        user.Exagochi.Points += challenge.Hardness;
        
        user.Exagochi.LastMeal = DateTime.UtcNow;
        
        if (user.Exagochi.Points >= user.Exagochi.Level * 10)
        {
            user.Exagochi.Level++;
            user.Exagochi.Points = 0;
        }
        
        await _db.SaveChangesAsync();
        
        return Ok("Successfully submitted challenge");
    }
    
    [HttpPost("createChallenge")]
    [Authorize]
    public async Task<IActionResult> CreateChallenge([FromBody] CreateChallengeModel model)
    {
        string subject;

        switch (model.Subject)
        {
            case Subjects.Math:
                subject = Subjects.Math;
                break;
            case Subjects.Physics:
                subject = Subjects.Physics;
                break;
            case Subjects.Russian:
                subject = Subjects.Russian;
                break;
            case Subjects.Ukrainian:
                subject = Subjects.Ukrainian;
                break;
            default:
                return BadRequest("Invalid subject");
        }
        
        var challenge = new Persistence.Entities.Challenge
        {
            Name = model.Name,
            Subject = subject,
            Hardness = model.Hardness,
            Answer = model.Answer,
            Description = model.Description
        };
        
        await _db.Challenges.AddAsync(challenge);
        
        await _db.SaveChangesAsync();
        
        return Ok("Successfully created challenge");
    }
}