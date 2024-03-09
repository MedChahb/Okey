namespace OkeyApi.Mappers;

using Dtos.Compte;
using Models;

/// <summary>
/// Classe Mapper pour faire passer un Dto de Utilisateur vers un Modèle Utilisateur
/// </summary>
public static class UtilisateurMappers
{
    /// <summary>
    /// Methode qui rend un permet d'obtenir les données à afficher publiquement
    /// </summary>
    /// <param name="userModel">Modèle utilisateur utilisé pour l'implémentation des méthodes</param>
    /// <returns>Dto d'un profil public</returns>
    public static PublicUtilisateurDto ToPublicUtilisateurDto(this Utilisateur userModel)
    {
        return new PublicUtilisateurDto { Username = userModel.UserName, Elo = userModel.Elo };
    }

    /// <summary>
    /// Methode qui rend un permet d'obtenir les données privée à afficher
    /// </summary>
    /// <param name="userModel">Modèle utilisateur utilisé pour l'implémentation des méthodes</param>
    /// <returns>Dto d'un profil privé</returns>
    public static PrivateUtilisateurDto ToPrivateUtilisateurDto(this Utilisateur userModel)
    {
        return new PrivateUtilisateurDto
        {
            Username = userModel.UserName,
            Elo = userModel.Elo,
            Achievements = new List<bool>()
        };
    }
}
