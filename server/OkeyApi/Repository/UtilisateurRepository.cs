namespace OkeyApi.Repository;

using Data;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

public class UtilisateurRepository : IUtilisateurRepository
{
    private readonly ApplicationDBContext _context;

    public UtilisateurRepository(ApplicationDBContext context)
    {
        this._context = context;
    }

    public async Task<List<Utilisateur>> GetAllAsync()
    {
        return await this._context.Users.ToListAsync();
    }
}
