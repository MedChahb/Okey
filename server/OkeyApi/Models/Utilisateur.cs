using Microsoft.AspNetCore.Identity;

namespace OkeyApi.Models
{
    /// <summary>
    /// Classe modèle utilisateur
    /// </summary>
    public class Utilisateur : IdentityUser
    {
        /// <summary>
        /// Permet de représenter l'Elo Score du joueur
        /// </summary>
        public int Elo { get; set; } = 400;
    }
}
