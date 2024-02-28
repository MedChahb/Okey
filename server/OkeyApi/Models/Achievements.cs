namespace OkeyApi.Models;

public class Achievements
{
    public int Id { get; set; }
    public bool Jouer5Parties { get; set; } = false;
    public bool GagnerUneFois { get; set; } = false;

    public string UserId { get; set; }
    public Utilisateur Utilisateur { get; set; }
}
