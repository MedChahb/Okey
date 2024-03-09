namespace OkeyApi.Dtos.Compte;

/// <summary>
/// Dto pour un utilisateur non connecté/public
/// </summary>
public class PublicUtilisateurDto
{
    /// <summary>
    /// Nom d'utilisateur
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Elo Score
    /// </summary>
    public int Elo { get; set; }
}
