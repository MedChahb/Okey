namespace OkeyServer.Misc;

using Lobby;

public class RoomManager : IRoomManager
{
    private readonly Dictionary<string, Room> _rooms;

    public event Action<string>? GameStarted;

    public RoomManager()
    {
        this._rooms = new Dictionary<string, Room>();
        this._rooms.Add("room1", new Room("room1"));
    }

    public Dictionary<string, Room> GetRooms() => this._rooms;

    public bool IsRoomFull(string roomName) =>
        this._rooms.TryGetValue(roomName, out var room) && room.IsFull();

    public void StartGameForRoom(string roomName)
    {
        if (this._rooms.TryGetValue(roomName, out var room) && room.IsFull())
        {
            this.GameStarted?.Invoke(roomName);
        }
    }

    public bool TryJoinRoom(string roomName, string playerId)
    {
        if (this._rooms.TryGetValue(roomName, out var room) && !room.IsFull())
        {
            room.AddPlayer(playerId);
            return true;
        }
        return false;
    }

    private void OnGameStarted(string roomName) => this.GameStarted?.Invoke(roomName);

    public void LeaveRoom(string roomName, string playerId)
    {
        if (this._rooms.TryGetValue(roomName, out var room))
        {
            room.RemovePlayer(playerId);
            //if (room.IsEmpty())
            //{
            //    this._rooms.Remove(roomName);
            //}
        }
    }

    public string GetRoomIdByConnectionId(string connectionId) =>
        this._rooms.FirstOrDefault(r => r.Value.HasPlayer(connectionId)).Key;

    public void PlayerDisconnected(string connectionId)
    {
        foreach (var room in this._rooms.Values)
        {
            room.RemovePlayer(connectionId);
            //if (room.IsEmpty())
            //{
            //    this._rooms.Remove(room.Name);
            //}
        }
    }

    public string GetRoomsInfo() =>
        string.Join(
            "\n",
            this._rooms.Values.Select(r => $"{r.Name}: {r.Players.Count}/{r.Capacity} players")
        );
}
