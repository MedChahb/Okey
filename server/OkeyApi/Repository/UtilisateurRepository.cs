namespace OkeyApi.Repository;

using Data;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

/// <summary>
/// Classe Repository des Utilisateurs, permet l'implémentation d'écriture en Base de Donnée
/// </summary>
public class UtilisateurRepository : IUtilisateurRepository
{
    /// <summary>
    /// Contexte de la base de donnée
    /// </summary>
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Constructeur de la classe
    /// </summary>
    /// <param name="context">Contexte Base de Donnée</param>
    public UtilisateurRepository(ApplicationDbContext context) => this._context = context;

    /// <summary>
    /// Récupération asynchrone de tout les utilisateurs en Base de Donnée
    /// </summary>
    /// <returns>Contrat permettant la validation des actions à faire, propre au système</returns>
    public async Task<List<Utilisateur>> GetAllAsync() => await this._context.Users.ToListAsync();

    /// <summary>
    /// Récupération asynchrone d'utilisateur en Base de Donnée
    /// </summary>
    /// <param name="username">Identitfiant de l'utilisateur à chercher</param>
    /// <returns>Contrat permettant la validation des actions à faire, propre au système</returns>
    public async Task<Utilisateur?> GetByUsername(string username) =>
        await this._context.Users.FirstOrDefaultAsync(s =>
            s.UserName != null && s.UserName.Equals(username, StringComparison.Ordinal)
        );

    public async Task UpdatePhotoAsync(string username, int photo)
    {
        var user = await this._context.Users.FirstOrDefaultAsync(s =>
            s.UserName != null && s.UserName.Equals(username, StringComparison.Ordinal)
        );
        if (user != null)
        {
            user.Photo = photo;
        }
        await this._context.SaveChangesAsync();
    }

    public async Task UpdateUsernameAsync(string username, string new_username)
    {
        var user = await this._context.Users.FirstOrDefaultAsync(s =>
            s.UserName != null && s.UserName.Equals(username, StringComparison.Ordinal)
        );

        var existinguser = await this._context.Users.FirstOrDefaultAsync(s =>
            s.UserName != null && s.UserName.Equals(new_username, StringComparison.Ordinal)
        );
        if (user != null)
        {
            if (existinguser == null)
            {
                user.UserName = new_username;
                user.NormalizedUserName = new_username.ToUpper(
                    System.Globalization.CultureInfo.CurrentCulture
                );
                await this._context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Utilisateur existant");
            }
        }
    }
}
