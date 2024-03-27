namespace Okey.AppelsAPI.Deserializer;

using System.Text.Json;
using Dtos;

public class PrivateWatchJsonDeserializer(string jsonData)
{
    public PrivateWatchDto? Deserialize() => JsonSerializer.Deserialize<PrivateWatchDto>(jsonData);
}
