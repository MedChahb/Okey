namespace OkeyServer.Player;

public class PlayerPublicData
{
    public string Username { get; set; }
    public int Elo { get; set; }
    public int Photo { get; set; }
    public DateTime DateInscription { get; set; }
    public int Experience { get; set; }
    public int NombreParties { get; set; }
    public int NombrePartiesGagnees { get; set; }

    public PlayerPublicData(
        string Username,
        int Elo,
        int Photo,
        DateTime DateInscription,
        int Experience,
        int NombreParties,
        int NombrePartiesGagnees
    )
    {
        this.Username = Username;
        this.Elo = Elo;
        this.Photo = Photo;
        this.DateInscription = DateInscription;
        this.Experience = Experience;
        this.NombreParties = NombreParties;
        this.NombrePartiesGagnees = NombrePartiesGagnees;
    }

    public override string ToString()
    {
        return "username : "
            + this.Username
            + " elo : "
            + this.Elo
            + " photo : "
            + this.Photo
            + " date d'inscription : "
            + this.DateInscription
            + " exp : "
            + this.Experience
            + " nb parties jouées : "
            + this.NombreParties
            + " nb parties gagnées : "
            + this.NombrePartiesGagnees;
    }
}
