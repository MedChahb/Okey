namespace OkeyApi.Dtos.Compte;

/// <summary>
/// Dto d'un utilisateur inscrit/connecté
/// </summary>
public class NewUtilisateurDto
{
    /// <summary>
    /// Nom d'utilisateur
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// Token JWT à garder pour soi
    /// </summary>
    public string? Token { get; set; }
}
