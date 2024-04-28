namespace OkeyServer.Data;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;

public class ServerDbContext(DbContextOptions dbContextOptions)
    : IdentityDbContext<Utilisateur>(dbContextOptions)
{
    public DbSet<Achievements>? Achievements { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        var roles = new List<IdentityRole>
        {
            new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
            new IdentityRole { Name = "Utilisateur", NormalizedName = "USER" }
        };
        builder.Entity<IdentityRole>().HasData(roles);
    }
}
