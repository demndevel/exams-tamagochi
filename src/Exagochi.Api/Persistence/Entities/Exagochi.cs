using System.ComponentModel.DataAnnotations.Schema;
using Exagochi.Api.Common;

namespace Exagochi.Api.Persistence.Entities;

public class Exagochi
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Satiety { get; set; }
    public byte HardnessStep { get; set; }
    public int Level { get; set; }
    public int Points { get; set; }
    public DateTime LastMeal { get; set; }
    
    public void Starve()
    {
        if (Satiety - HardnessStep < ExagochiConstants.MinSatiety) 
            Satiety = ExagochiConstants.MinSatiety;
        else
            Satiety -= HardnessStep;
    }
}