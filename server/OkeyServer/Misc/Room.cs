namespace OkeyServer.Misc;

public class Room
{
    private string _roomName;
    private int _capacite;
    private int _nbJoueurs;
    private List<string> _joueursId;

    public Room(string roomName)
    {
        this._roomName = roomName;
        this._capacite = 4;
        this._nbJoueurs = 0;
        this._joueursId = new List<string>();
    }

    public bool? JoueurJoinRoom(string joueurId)
    {
        if (this._joueursId.Count >= this._capacite)
        {
            return null;
        }
        this._nbJoueurs++;
        this._joueursId.Add(joueurId);
        if (this._nbJoueurs == this._capacite)
        {
            return false;
        }
        return true;
    }

    public void JoueurLeaveRoom(string joueurId)
    {
        this._nbJoueurs--;
        this._joueursId.Remove(joueurId);
    }

    public string GetRoomName() => this._roomName;

    public int GetNbCurrent() => this._nbJoueurs;

    public int GetCapacity() => this._capacite;

    public List<string> GetUserIds() => this._joueursId;
}
