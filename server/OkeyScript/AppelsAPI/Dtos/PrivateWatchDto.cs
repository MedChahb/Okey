namespace Okey.AppelsAPI.Dtos;

public class PrivateWatchDto
{
    public string username { get; set; }
    public int elo { get; set; }
    public List<bool> achievements { get; set; }
}
