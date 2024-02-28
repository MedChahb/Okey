namespace OkeyApi.Models;

public class Achievements
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public bool Jouer5Parties { get; set; } = false;
    public bool GagnerUneFois { get; set; } = false;
}
