namespace OkeyServer.Misc;

using System.Collections.Concurrent;
using Lobby;

public class RoomManager : IRoomManager
{
    private readonly Dictionary<string, Room> _rooms;

    private ConcurrentBag<Room> roomsAvailable;
    private ConcurrentBag<Room> RoomsBusy;

    public event Action<string>? GameStarted;

    public RoomManager()
    {
        this._rooms = new Dictionary<string, Room>();
        this.roomsAvailable = new ConcurrentBag<Room>();
        this.RoomsBusy = new ConcurrentBag<Room>();

        this._rooms.Add("room1", new Room("room1"));

        foreach (var room in this._rooms)
        {
            this.roomsAvailable.Add(room.Value);
        }
    }

    public ConcurrentBag<Room> GetRooms() => this.roomsAvailable;

    public Room GetRoomById(string id) => this._rooms[id];

    public bool IsRoomFull(string roomName) =>
        this._rooms.TryGetValue(roomName, out var room) && room.IsFull();

    public void ResetRoom(string roomName)
    {
        var room = this.GetRoomById(roomName);
        room.Players = new List<string>();
        this.RoomsBusy = new ConcurrentBag<Room>(this.RoomsBusy.Except(new[] { room }));
        this.roomsAvailable.Add(room);
    }

    public void StartGameForRoom(string roomName)
    {
        if (this._rooms.TryGetValue(roomName, out var room) && room.IsFull())
        {
            this.GameStarted?.Invoke(roomName);
        }
    }

    public bool IsRoomBusy(string roomName)
    {
        foreach (var roomB in this.RoomsBusy)
        {
            if (roomB.Name.Equals(roomName, StringComparison.Ordinal))
            {
                return true;
            }
        }
        return false;
    }

    public bool TryJoinRoom(string roomName, string playerId)
    {
        if (this._rooms.TryGetValue(roomName, out var room) && !room.IsFull())
        {
            room.AddPlayer(playerId);

            if (room.IsFull())
            {
                this.RoomsBusy.Add(room);
                this.roomsAvailable = new ConcurrentBag<Room>(
                    this.roomsAvailable.Except(new[] { room })
                );
            }

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
        }
    }

    public string GetRoomIdByConnectionId(string connectionId) =>
        this._rooms.FirstOrDefault(r => r.Value.HasPlayer(connectionId)).Key;

    public void PlayerDisconnected(string connectionId)
    {
        foreach (var room in this._rooms.Values)
        {
            room.RemovePlayer(connectionId);
        }
    }

    public string GetFirstRoomAvailable()
    {
        Room? room;
        if (this.roomsAvailable.TryPeek(out room))
        {
            return room.Name;
        }
        return "";
    }

    public string GetRoomsInfo() =>
        string.Join(
            "\n",
            this._rooms.Values.Select(r => $"{r.Name}: {r.Players.Count}/{r.Capacity} players")
        );
}
