namespace OkeyApi.Controllers;

using Dtos.Classement;
using Interfaces;
using Microsoft.AspNetCore.Mvc;

///<summary>
/// Classe Controller de la gestion du classement utilisateur
/// Contient toute la logique des end-points de la forme: /classement/*
/// </summary>

[Route("okeyapi/classement")]
[ApiController]
public class ClassementController : ControllerBase
{
    private readonly IUtilisateurRepository _utilisateurRepo;

    /// <summary>
    /// Constructeur de la classe ClassementController
    /// </summary>
    /// <param name="utilisateurRepository">Issu de notre API fournissant des fonctions de recherche d'utilisateurs efficaces.</param>
    public ClassementController(IUtilisateurRepository utilisateurRepository)
    {
        this._utilisateurRepo = utilisateurRepository;
    }

    /// <summary>
    /// Route GET de l'API permettant d'obtenir le classement général
    /// </summary>
    /// <returns>Retourne le contrat représentant l'action de la méthode, géré par le framework.</returns>
    /// <remarks>Le JSON obtenu contient: le nom d'utilisateur, son elo ainsi que son classement. Le classement est deéjà trié.</remarks>>
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

    /// <summary>
    /// Route GET de l'API permettant d'obtenir le classement d'un utilisateur identifié par son nom d'utilisateur
    /// </summary>
    /// <param name="username">Nom d'utilisateur du joueur recherché</param>
    /// <returns>Retourne le contrat représentant l'action de la méthode, géré par le framework.</returns>
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

    /// <summary>
    /// Route GET de l'API permettant d'obtenir le top n (n un entier naturel supérieur à 0) général.
    /// </summary>
    /// <param name="pagination">Entier supérieur à 0</param>
    /// <returns>Retourne le contrat représentant l'action de la méthode, géré par le framework.</returns>
    [HttpGet("{pagination:int}")]
    public async Task<IActionResult> GetPaginate([FromRoute] int pagination)
    {
        if (pagination > 0)
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

        return this.StatusCode(500, "La pagination doit être supérieur à 0.");
    }
}
