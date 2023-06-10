using System.ComponentModel.DataAnnotations;

namespace Exagochi.Api.Models.Challenge;

public class CreateChallengeModel
{
    [Required] public string Name { get; set; } = null!;
    [Required] public string Subject { get; set; } = null!;
    [Required] [MinLength(1)] public string Answer { get; set; } = null!;
    
    [Required] [MinLength(1)] public string Description { get; set; } = null!;
    [Required] [Range(1, 20)] public byte Hardness { get; set; }
}