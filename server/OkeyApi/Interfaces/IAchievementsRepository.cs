namespace OkeyApi.Interfaces;

using Models;

public interface IAchievementsRepository
{
    Task<Achievements?> CreateAsync(Achievements achievements);
}
