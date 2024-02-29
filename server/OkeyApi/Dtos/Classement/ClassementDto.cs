namespace OkeyApi.Dtos.Classement;

public class ClassementDto
{
    public string? Username { get; set; } = string.Empty;
    public int Classement { set; get; }
    public int Elo { get; set; }
}
