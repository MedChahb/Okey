namespace OkeyApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using OkeyApi.Data;

[Route("okeyapi/achivements")]
[ApiController]
public class AchievementsController(ApplicationDBContext context) : ControllerBase { }
