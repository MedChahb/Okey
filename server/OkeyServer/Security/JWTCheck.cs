namespace OkeyServer.Security;

using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

public class JWTCheck
{
    /// <summary>
    /// Attribut contenant la configuration de l'API
    /// </summary>
    private readonly IConfiguration _config;

    public JWTCheck(IConfiguration config)
    {
        this._config = config;
    }

    public static bool CheckToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidIssuer = "http://localhost:5246", // Set your issuer here
            ValidateAudience = true,
            ValidAudience = "http://localhost:5246", // Set your audience here
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(
                    "sdgfijjh3466iu345g87g08c24g7204gr803g30587ghh35807fg39074fvg80493745gf082b507807g807fgf"
                )
            ),
        };

        try
        {
            // Validate token and extract claims
            var claimsPrincipal = tokenHandler.ValidateToken(
                token,
                tokenValidationParameters,
                out var validatedToken
            );
        }
        catch (SecurityTokenValidationException)
        {
            return false;
        }
        return true;
    }
}
