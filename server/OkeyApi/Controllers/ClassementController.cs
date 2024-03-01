namespace OkeyApi.Controllers;

using Dtos.Classement;
using Interfaces;
using Microsoft.AspNetCore.Mvc;

[Route("okeyapi/classement")]
[ApiController]
public class ClassementController : ControllerBase
{
    private readonly IUtilisateurRepository _utilisateurRepo;

    public ClassementController(IUtilisateurRepository utilisateurRepository)
    {
        this._utilisateurRepo = utilisateurRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var utilisateurs = await this._utilisateurRepo.GetAllAsync(); // Obtenez tous les utilisateurs
            var classement = new List<ClassementDto>();
            foreach (var utilisateur in utilisateurs)
            {
                classement.Add(
                    new ClassementDto { Username = utilisateur.UserName, Elo = utilisateur.Elo }
                );
            }

            classement = classement.OrderByDescending(s => s.Elo).ToList();

            for (var i = 0; i < classement.Count; i++)
            {
                classement[i].Classement = i + 1;
            }

            return this.Ok(classement);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return this.StatusCode(
                500,
                "Une erreur s'est produite lors de la récupération du classement."
            );
        }
    }

    [HttpGet("{username}")]
    public async Task<IActionResult> GetById([FromRoute] string username)
    {
        try
        {
            var utilisateurs = await this._utilisateurRepo.GetAllAsync(); // Obtenez tous les utilisateurs
            var classement = new List<ClassementDto>();
            foreach (var utilisateur in utilisateurs)
            {
                classement.Add(
                    new ClassementDto { Username = utilisateur.UserName, Elo = utilisateur.Elo }
                );
            }

            classement = classement.OrderByDescending(s => s.Elo).ToList();

            for (var i = 0; i < classement.Count; i++)
            {
                classement[i].Classement = i + 1;
            }

            var user = classement.FirstOrDefault(u => u.Username.Equals(username));
            if (user == null)
            {
                return this.StatusCode(404, "Utilisateur non trouvé");
            }
            return this.Ok(user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return this.StatusCode(
                500,
                "Une erreur s'est produite lors de la récupération du classement."
            );
        }
    }

    [HttpGet("{pagination:int}")]
    public async Task<IActionResult> GetPaginate([FromRoute] int pagination)
    {
        try
        {
            var utilisateurs = await this._utilisateurRepo.GetAllAsync(); // Obtenez tous les utilisateurs
            var classement = new List<ClassementDto>();
            foreach (var utilisateur in utilisateurs)
            {
                classement.Add(
                    new ClassementDto { Username = utilisateur.UserName, Elo = utilisateur.Elo }
                );
            }

            classement = classement.OrderByDescending(s => s.Elo).ToList();

            for (var i = 0; i < classement.Count; i++)
            {
                classement[i].Classement = i + 1;
            }

            return this.Ok(classement.Take(pagination));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return this.StatusCode(
                500,
                "Une erreur s'est produite lors de la récupération du classement."
            );
        }
    }
}
