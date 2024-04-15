namespace OkeyServer.Player;

using System.Collections.Concurrent;

public class PlayerDatas
{
    private string _username;

    //private int _elo;
    private ConcurrentDictionary<string, bool> _achievements;

    public PlayerDatas(string username)
    {
        this._username = username;
        this._achievements = new ConcurrentDictionary<string, bool>();
    }
}
