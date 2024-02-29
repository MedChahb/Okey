namespace OkeyApi.Repository;

using Data;
using Dtos.Achievements;
using Interfaces;
using Models;

public class AchievementsRepository : IAchievementsRepository
{
    public readonly ApplicationDBContext _context;

    public AchievementsRepository(ApplicationDBContext context)
    {
        this._context = context;
    }

    public async Task<Achievements?> CreateAsync(Achievements achievementsModel)
    {
        await this._context.Achievements.AddAsync(achievementsModel);
        await this._context.SaveChangesAsync();
        return achievementsModel;
    }
}
