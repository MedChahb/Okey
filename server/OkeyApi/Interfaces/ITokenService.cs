namespace OkeyApi.Interfaces;

using OkeyApi.Models;

public interface ITokenService
{
    public string CreateToken(Utilisateur utilisateur);
}
