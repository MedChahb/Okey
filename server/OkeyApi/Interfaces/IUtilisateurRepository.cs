namespace OkeyApi.Interfaces;

using Models;

public interface IUtilisateurRepository
{
    Task<List<Utilisateur>> GetAllAsync();
    Task<Utilisateur?> GetByUsername(string username);
}
