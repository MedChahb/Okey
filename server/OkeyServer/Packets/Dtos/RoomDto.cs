namespace OkeyServer.Packets.Dtos;

public class RoomDto
{
    public string? Name { get; set; }
    public int Capacity { get; set; }
    public List<string>? Players { get; set; }
}
