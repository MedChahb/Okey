namespace Okey.AppelsAPI.Entry;

public class LoginEntryDto
{
    public string userName { get; set; }
    public string password { get; set; }

    public LoginEntryDto(string username, string password)
    {
        this.userName = username;
        this.password = password;
    }
}
