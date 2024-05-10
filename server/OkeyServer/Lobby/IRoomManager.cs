namespace OkeyServer.Misc;

using System.Collections.Concurrent;
using Lobby;

public interface IRoomManager
{
    event Action<string> GameStarted;
    bool TryJoinRoom(string roomName, string playerId);
    void LeaveRoom(string roomName, string playerId);
    string GetRoomIdByConnectionId(string connectionId);
    void PlayerDisconnected(string connectionId);
    string GetRoomsInfo();
    public bool IsRoomFull(string roomName);
    public void StartGameForRoom(string roomName);
    public ConcurrentBag<Room> GetRooms();
    public Room GetRoomById(string id);
    public void ResetRoom(string roomName);
    public bool IsRoomBusy(string roomName);
    public string GetFirstRoomAvailable();
}
