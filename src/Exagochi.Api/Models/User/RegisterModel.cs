using System.ComponentModel.DataAnnotations;

namespace Exagochi.Api.Models.User;

public record RegisterModel(
    [MinLength(2), MaxLength(20)] string Username, 
    [MinLength(5)] string Password);
