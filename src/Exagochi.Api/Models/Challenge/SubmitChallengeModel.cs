using System.ComponentModel.DataAnnotations;

namespace Exagochi.Api.Models.Challenge;

public class SubmitChallengeModel
{
    [Required]
    public int ChallengeId { get; set; }

    [Required] [MinLength(1)] public string Answer { get; set; } = null!;
}