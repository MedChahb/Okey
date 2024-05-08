namespace OkeyServer.Lobby;

public class Room
{
    public string Name { get; }
    public int Capacity { get; }
    public List<string> Players { get; set; }

    public Room(string name, int capacity = 4)
    {
        this.Name = name;
        this.Capacity = capacity;
        this.Players = new List<string>();
    }

    public bool IsFull() => this.Players.Count >= this.Capacity;

    public bool IsEmpty() => this.Players.Count == 0;

    public void AddPlayer(string playerId)
    {
        if (!this.Players.Contains(playerId))
        {
            this.Players.Add(playerId);
        }
    }

    public void RemovePlayer(string playerId) => this.Players.Remove(playerId);

    public bool HasPlayer(string playerId) => this.Players.Contains(playerId);

    public List<string> GetPlayerIds() => this.Players;
}
