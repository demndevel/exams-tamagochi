using System.ComponentModel.DataAnnotations;

namespace Exagochi.Api.Models.Exagochi;

public class CreateExagochiModel
{
    public string Name { get; set; } = null!;
    [Required]
    [Range(1, 10)]
    public byte HardnessStep { get; set; }
}