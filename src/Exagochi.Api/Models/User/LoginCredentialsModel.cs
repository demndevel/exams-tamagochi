using System.ComponentModel.DataAnnotations;

namespace Exagochi.Api.Models.User;

public record LoginCredentialsModel(
    [MinLength(3)] [MaxLength(200)]
    string Username,
    [MinLength(5)]
    string Password);