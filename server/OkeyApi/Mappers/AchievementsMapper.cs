namespace OkeyApi.Mappers;

using Dtos.Achievements;
using Dtos.Compte;
using Models;

public static class AchievementsMapper
{
    public static AchievementDto ToAchievementDto(this Achievements achievementsModel)
    {
        return new AchievementDto
        {
            Id = achievementsModel.Id,
            Jouer5Parties = achievementsModel.Jouer5Parties,
            GagnerUneFois = achievementsModel.GagnerUneFois,
            UserId = achievementsModel.UserId,
            Utilisateur = achievementsModel.Utilisateur
        };
    }

    public static Achievements ToAchievementFromCreate(
        this CreateAchievementsDto createAchievementsDto,
        Utilisateur utilisateur
    )
    {
        return new Achievements
        {
            Id = createAchievementsDto.Id,
            UserId = createAchievementsDto.UserId,
            Utilisateur = utilisateur
        };
    }
}
