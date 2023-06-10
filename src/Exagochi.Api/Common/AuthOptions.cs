using Microsoft.IdentityModel.Tokens;

namespace Exagochi.Api.Common;

public static class AuthOptions
{
    public const string Issuer = "ExagochiAPI";
    public const string Audience = "ExagochiAPI";

    public static SecurityKey GetSymmetricSecurityKey(string input = "SomeFunnyKey228!!!!!!!!!")
        => new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(input));
}