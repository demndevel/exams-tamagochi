using System.ComponentModel.DataAnnotations.Schema;

namespace Exagochi.Api.Persistence.Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public int? ExagochiId { get; set; }
    public Exagochi? Exagochi { get; set; }
}