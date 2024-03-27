namespace Okey.AppelsAPI.Factory;

using AppelsApi.Dtos;
using Dtos;

public interface IWatchDtoFactory
{
    Task<PublicWatchDto?> CreatePublicWatchDtoTask(string content);
    Task<PrivateWatchDto?> CreatePrivateWatchDtoTask(string username);
}
