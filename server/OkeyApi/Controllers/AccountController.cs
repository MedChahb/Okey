namespace OkeyApi.Controllers;

using System.Security.Claims;
using Data;
using Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OkeyApi.Dtos.Compte;
using OkeyApi.Interfaces;
using OkeyApi.Models;

[Route("okeyapi/compte")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<Utilisateur> _utilisateurManager;
    private readonly ITokenService _tokenService;
    private readonly SignInManager<Utilisateur> _signInManager;
    private readonly IUtilisateurRepository _utilisateurRepository;
    private readonly ApplicationDBContext _dbContext;

    public AccountController(
        UserManager<Utilisateur> utilisateurManager,
        ITokenService tokenService,
        SignInManager<Utilisateur> signInManager,
        IUtilisateurRepository utilisateurRepository,
        ApplicationDBContext dbContext
    )
    {
        this._utilisateurManager = utilisateurManager;
        this._tokenService = tokenService;
        this._signInManager = signInManager;
        this._utilisateurRepository = utilisateurRepository;
        this._dbContext = dbContext;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var user = await this._utilisateurManager.Users.FirstOrDefaultAsync(x =>
            x.UserName == loginDto.UserName.ToLower()
        );
        if (user == null)
            return this.Unauthorized("Nom d'utilisateur invalide");
        var result = await this._signInManager.CheckPasswordSignInAsync(
            user,
            loginDto.Password,
            false
        );
        if (!result.Succeeded)
            return this.Unauthorized(
                "Utilisateur non trouvé! (nom d'utilisateur ou mot de passe incorrect)"
            );

        return this.Ok(
            new NewUtilisateurDto
            {
                UserName = user.UserName,
                Token = this._tokenService.CreateToken(user)
            }
        );
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var utilisateur = new Utilisateur { UserName = registerDto.Username };
            var createUtilisateur = await this._utilisateurManager.CreateAsync(
                utilisateur,
                registerDto.Password
            );
            if (createUtilisateur.Succeeded)
            {
                var roleResult = await this._utilisateurManager.AddToRoleAsync(utilisateur, "USER");
                if (roleResult.Succeeded)
                {
                    try
                    {
                        var achievement = new Achievements
                        {
                            UserId = utilisateur.Id,
                            Utilisateur = utilisateur
                        };
                        this._dbContext.Achievements.Add(achievement);
                        await this._dbContext.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                    return this.Ok(
                        new NewUtilisateurDto
                        {
                            UserName = utilisateur.UserName,
                            Token = this._tokenService.CreateToken(utilisateur)
                        }
                    );
                }
                return this.StatusCode(500, roleResult.Errors);
            }
            return this.StatusCode(500, createUtilisateur.Errors);
        }
        catch (Exception e)
        {
            return this.StatusCode(500, e);
        }
    }

    [HttpGet("watch")]
    public async Task<IActionResult> GetAll()
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest();
        }
        var users = await this._utilisateurRepository.GetAllAsync();
        var usersDto = users.Select(s => s.ToPublicUtilisateurDto());
        return this.Ok(usersDto);
    }

    [HttpGet("watch/{username}")]
    public async Task<IActionResult> GetByUsername([FromRoute] string username)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest();
        }

        var user = await this._utilisateurRepository.GetByUsername(username);
        if (user == null)
        {
            return this.NotFound();
        }

        var userIdentity = this.User.Identity;
        if (userIdentity != null && userIdentity.IsAuthenticated)
        {
            var utilisateur = this.GetCurrentUser();
            if (utilisateur.Result.Username == username)
            {
                var achievements = await this._dbContext.Achievements.FirstOrDefaultAsync(e =>
                    e.Utilisateur.UserName.Equals(utilisateur.Result.Username)
                );
                var list = new List<bool>();
                list.Add(achievements.Jouer5Parties);
                list.Add(achievements.GagnerUneFois);
                return this.Ok(
                    new PrivateUtilisateurDto
                    {
                        Username = utilisateur.Result.Username,
                        Elo = utilisateur.Result.Elo,
                        Achievements = list
                    }
                );
            }
        }
        return this.Ok(user.ToPublicUtilisateurDto());
    }

    private async Task<PrivateUtilisateurDto> GetCurrentUser()
    {
        var claims = this.HttpContext?.User?.Claims;
        if (claims != null)
        {
            var username = claims.First().Value;
            var user = await this._utilisateurRepository.GetByUsername(username);
            if (user == null)
            {
                return null!;
            }
            return user.ToPrivateUtilisateurDto();
        }
        return null!;
    }
}
