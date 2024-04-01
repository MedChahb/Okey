namespace OkeyServer.Misc;

public class RoomNotFoundException : Exception
{
    public RoomNotFoundException(string message)
        : base(message) { }
}
