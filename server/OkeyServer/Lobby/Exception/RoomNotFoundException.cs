namespace OkeyServer.Lobby.Exception;

using Exception = System.Exception;

public class RoomNotFoundException : Exception
{
    public RoomNotFoundException(string message)
        : base(message) { }
}
