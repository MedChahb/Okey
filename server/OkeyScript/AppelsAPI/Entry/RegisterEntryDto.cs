namespace Okey.AppelsAPI.Entry;

public class RegisterEntryDto
{
    public string username { get; set; }
    public string password { get; set; }

    public RegisterEntryDto(string username, string password)
    {
        this.username = username;
        this.password = password;
    }
}
