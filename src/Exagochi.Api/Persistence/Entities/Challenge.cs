namespace Exagochi.Api.Persistence.Entities;

public class Challenge
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Answer { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public byte Hardness { get; set; }
}