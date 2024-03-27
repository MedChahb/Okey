namespace Okey.AppelsAPI.Deserializer;

using System.Text.Json;
using Dtos;

public class PublicWatchJsonDeserializer(string jsonData)
{
    public PublicWatchDto? Deserialize() => JsonSerializer.Deserialize<PublicWatchDto>(jsonData);
}
