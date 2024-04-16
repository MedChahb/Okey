namespace OkeyServer.Player;

using System.Collections.Concurrent;

public class PlayerDatas
{
    private string Username { get; set; }
    private int Elo { get; set; }
    private ConcurrentDictionary<string, bool> _achievements;

    public PlayerDatas(string username, int elo)
    {
        this.Username = username;
        this.Elo = elo;
        this._achievements = new ConcurrentDictionary<string, bool>();
    }

    public bool CheckAchievement(string achievement)
    {
        return this._achievements[achievement];
    }

    public void UpdateElo(int value)
    {
        this.Elo = this.Elo + value;
        // TODO envoyer à la BDD la mise à jour
    }

    public override string ToString()
    {
        return this.Username + " " + this.Elo;
    }
}
