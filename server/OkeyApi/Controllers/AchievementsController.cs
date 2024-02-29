namespace OkeyApi.Controllers;

using Dtos.Achievements;
using Interfaces;
using Mappers;
using Microsoft.AspNetCore.Mvc;
using Models;
using OkeyApi.Data;
using Repository;

[Route("okeyapi/achivements")]
[ApiController]
public class AchievementsController : ControllerBase
{
    private readonly IAchievementsRepository _achievementsRepo;
    private readonly ApplicationDBContext _context;

    public AchievementsController(
        ApplicationDBContext context,
        IAchievementsRepository achievementsRepo
    )
    {
        this._context = context;
        this._achievementsRepo = achievementsRepo;
    }
}
