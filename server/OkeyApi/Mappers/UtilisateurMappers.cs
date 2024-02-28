namespace OkeyApi.Mappers;

using Dtos.Compte;
using Models;

public static class UtilisateurMappers
{
    public static PublicUtilisateurDto ToPublicUtilisateurDto(this Utilisateur userModel)
    {
        return new PublicUtilisateurDto { Username = userModel.UserName, Elo = userModel.Elo };
    }
}
