namespace Okey.AppelsAPI.Factory;

using Deserializer;
using Dtos;

public class WatchDtoFactory(APICalls apiClient)
{
    public Task<PublicWatchDto?> CreatePublicWatchDtoTask(string response)
    {
        var jsonData = new PublicWatchJsonDeserializer(response).Deserialize();
        return Task.FromResult(jsonData);
    }

    public Task<PrivateWatchDto?> CreatePrivateWatchDtoTask(string response)
    {
        var jsonData = new PrivateWatchJsonDeserializer(response).Deserialize();
        return Task.FromResult(jsonData);
    }
}
