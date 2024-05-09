namespace OkeyServer.Packets;

public class StartingGamePacket
{
    public List<string>? playersList { get; set; }
    public string? playerId { get; set; }
}
