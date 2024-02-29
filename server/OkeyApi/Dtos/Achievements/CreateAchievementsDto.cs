namespace OkeyApi.Dtos.Achievements;

using Models;

public class CreateAchievementsDto
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public Utilisateur Utilisateur { get; set; }
}
