namespace OkeyApi.Dtos.Compte;

/// <summary>
/// Dto pour un utilisateur connecté pouvant accéder a ses données
/// </summary>
public class PrivateUtilisateurDto
{
    /// <summary>
    /// Nom d'utilisateur
    /// </summary>
    public string? Username { get; set; } = string.Empty;

    /// <summary>
    /// Elo Score
    /// </summary>
    public int Elo { get; set; }

    /// <summary>
    /// Liste des achievements de l'utilisateur
    /// </summary>
    public List<bool> Achievements { get; set; } = new List<bool>();
}
