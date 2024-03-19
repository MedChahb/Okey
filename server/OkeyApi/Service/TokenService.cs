namespace OkeyApi.Service;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using OkeyApi.Interfaces;
using OkeyApi.Models;

/// <summary>
/// Classe Service Token, Service implémentant les JWT
/// </summary>
public class TokenService : ITokenService
{
    /// <summary>
    /// Attribut contenant la configuration de l'API
    /// </summary>
    private readonly IConfiguration _config;

    /// <summary>
    /// Attribut contenant la clé secrète
    /// </summary>
    private readonly SymmetricSecurityKey _key;

    /// <summary>
    /// Constructeur de la classe
    /// </summary>
    /// <param name="config">Configuration de l'API</param>
    public TokenService(IConfiguration config)
    {
        _config = config;
        _key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["JWT:SigningKey"]));
    }

    /// <summary>
    /// Création d'un Token pour un utilisateur
    /// </summary>
    /// <param name="user">Utilisateur associé au Token</param>
    /// <returns>Token sous forme de chaîne de caractère</returns>
    public string CreateToken(Utilisateur user)
    {
        try
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.GivenName, user.UserName)
            };
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = creds,
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
