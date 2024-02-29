using Microsoft.AspNetCore.Identity;

namespace OkeyApi.Models
{
    public class Utilisateur : IdentityUser
    {
        public int Elo { get; set; } = 400;
    }
}
