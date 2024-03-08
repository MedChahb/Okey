namespace OkeyApi.Dtos.Compte;

public class PrivateUtilisateurDto
{
    public string? Username { get; set; } = string.Empty;
    public int Elo { get; set; }
    public List<bool> Achievements { get; set; } = new List<bool>();
}
