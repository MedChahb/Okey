namespace OkeyServer.Player;

using System.Collections.Concurrent;
using Data;
using Exceptions;

/// <summary>
/// Classe contenant toutes les données d'un utilisateur connecté ou d'un guest actuellement sur le serveur !!!! WIP !!!!
/// </summary>
public class PlayerDatas
{
    private string Username { get; set; }
    private int Elo { get; set; }
    private int Photo { get; set; }
    private DateTime DateInscription { get; set; }
    private int Experience { get; set; }
    private int NombreParties { get; set; }
    private int NombrePartiesGagnees { get; set; }
    private ConcurrentDictionary<string, bool>? _achievements;

    /// <summary>
    /// Initialisation de la classe PlayerDatas
    /// </summary>
    /// <param name="DbContext"></param>
    /// <param name="Username"></param>
    public PlayerDatas(ServerDbContext DbContext, string Username)
    {
        this.Username = Username;
        if (Username == "Guest")
        {
            this.Elo = 400;
            var random = new Random();
            var randomNumber = random.Next(1, 5);
            this.Photo = randomNumber;
            this.Experience = 0;
            this.NombreParties = 0;
            this.NombrePartiesGagnees = 0;
            this.DateInscription = DateTime.Now;
        }
        else
        {
            // on cherche dans la base de donnée la ligne de la tablke correspondate au username fourni
            var player = DbContext.Users.FirstOrDefault(User => User.UserName == Username);

            if (player != null)
            {
                this.Elo = player.Elo;
                this.Photo = player.Photo;
                this.Experience = player.Experience;
                this.NombreParties = player.NombreParties;
                this.NombrePartiesGagnees = player.NombrePartiesGagnees;
                this.DateInscription = player.DateInscription;

                /// partie Achievements :
                this._achievements = new ConcurrentDictionary<string, bool>();
                var achievDbContext = DbContext.Achievements;
                if (achievDbContext != null)
                {
                    var achievements = achievDbContext.FirstOrDefault(Ach =>
                        Ach.Utilisateur != null
                        && Ach.Utilisateur.UserName != null
                        && Ach.Utilisateur.UserName.Equals(Username, StringComparison.Ordinal)
                    );
                    if (achievements != null)
                    {
                        /// ajouter tous les achievements au dictionnaire, à la main ;'(

                        var added = this._achievements.TryAdd(
                            "Jouer5Parties",
                            achievements.Jouer5Parties
                        );
                        added =
                            added
                            && this._achievements.TryAdd(
                                "GagnerUneFois",
                                achievements.GagnerUneFois
                            );
                        if (added == false)
                        {
                            /// exception un ajout au dictionnaire a échoué
                            throw new DictionnaryAddException(
                                "Couldn't add one or more achievements to the user data structure"
                            );
                        }
                    }
                    else
                    {
                        /// exception aaucune ligne achievements correspondat à l'utilisateur n'à été trouvée
                        throw new UserAchievementsNotFoundException(
                            "Couldn't find " + this.Username + " achievements"
                        );
                    }
                }
                else
                {
                    /// exception aucune entrée de la table achievements n'a été trouvé
                    throw new UserAchievementsNotFoundException("Couldn't find users achievements");
                }
            }
            else
            {
                /// le joueur avec cet username n'a pas été trouvé dans la base de donnée
                throw new UserAchievementsNotFoundException(
                    "Couldn't find user : " + this.Username
                );
            }
        }
    }

    /// <summary>
    /// renvoi un booléen indiquant si le succès
    /// </summary>
    /// <param name="Achievement"></param>
    /// <returns> booléen vrai si l'achievement est débloqué </returns>
    public bool CheckAchievement(string Achievement)
    {
        if (this.Username == "Guest")
        {
            return false;
        }
        if (this._achievements != null)
        {
            return this._achievements[Achievement];
        }
        return false;
    }

    /// <summary>
    ///  Fonction asynchrone qui met à jour les statistiques d'un utilisateur en les incrémentant des valeurs fournies en paramètres
    /// </summary>
    /// <param name="dbContext"> le contexte permettant d'accéder à la base de donnée</param>
    /// <param name="elo"> le elo à rajouter au elo du joueur</param>
    /// <param name="experience"> l'experience à rajouter à l'experience du joueur </param>
    /// <param name="PartieGagnee"> booléen indiquant si le nombre de partie gagnées doit être incrémenté, faux par défaut</param>
    /// <param name="partieJouee"> booléen indiquant si le nombre de parties doit être incrementé, vrai par défaut</param>
    public async Task UpdateStats(
        ServerDbContext dbContext,
        int elo,
        int experience,
        bool partieJouee = true,
        bool PartieGagnee = false
    )
    {
        if (this.Username != "Guest")
        {
            var userUpdate = dbContext.Users.SingleOrDefault(u => u.UserName == this.Username);
            if (userUpdate != null)
            {
                this.Elo += elo;
                this.Experience += experience;

                if (this.NombreParties == 4 && partieJouee)
                {
                    await this.UpdateAchievement(dbContext, "Jouer5Parties");
                }

                if (partieJouee)
                {
                    this.NombreParties++;
                }
                if (this.NombrePartiesGagnees == 0 && PartieGagnee)
                {
                    await this.UpdateAchievement(dbContext, "GagnerUneFois");
                }
                if (PartieGagnee)
                {
                    this.NombrePartiesGagnees++;
                }
                userUpdate.Elo = this.Elo += elo;
                userUpdate.Experience = this.Experience += experience;
                userUpdate.NombreParties = this.NombreParties;
                userUpdate.NombrePartiesGagnees = this.NombrePartiesGagnees;
                await dbContext.SaveChangesAsync();
            }
            else
            {
                /// exception erreur lors de la recuperation du profil
                throw new UserUpdateException(
                    "Couldn't fetch user :" + this.Username + " from the database"
                );
            }
        }
    }

    /// <summary>
    /// met à jour un succès, par défaut le dévérouille et si value est renseigné le met à la valeur de value
    /// </summary>
    /// <param name="achievement"> titre du succès </param>
    /// <param name="value"> nouvelle valeur du succès</param>
    /// <exception cref="UserAchievementsNotFoundException"> exception indiquant l'échec de récuperations des succès dans la base de donnée</exception>
    public async Task UpdateAchievement(
        ServerDbContext dbContext,
        string achievement,
        bool value = true
    )
    {
        if (this.Username != "Guest")
        {
            if (this._achievements != null)
            {
                this._achievements[achievement] = value;
            }
            else
            {
                throw new UserAchievementsNotFoundException("achievements attribute is null");
            }

            var achievDbContext = dbContext.Achievements;
            if (achievDbContext != null)
            {
                var achievements = achievDbContext.FirstOrDefault(ach =>
                    ach.Utilisateur != null
                    && ach.Utilisateur.UserName != null
                    && ach.Utilisateur.UserName.Equals(this.Username, StringComparison.Ordinal)
                );
                if (achievements != null)
                {
                    if (achievement.Equals("Jouer5Parties", StringComparison.Ordinal))
                    {
                        achievements.Jouer5Parties = value;
                    }
                    else if (achievement.Equals("GagnerUneFois", StringComparison.Ordinal))
                    {
                        achievements.GagnerUneFois = value;
                    }
                    else
                    {
                        throw new UserAchievementsNotFoundException(
                            "Couldn't find the achievement : "
                                + achievement
                                + " in the user : "
                                + this.Username
                                + " achievements collection"
                        );
                    }

                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    /// exception aaucune ligne achievements correspondat à l'utilisateur n'à été trouvée
                    throw new UserAchievementsNotFoundException(
                        "Couldn't find " + this.Username + " achievements"
                    );
                }
            }
            else
            {
                /// exception aucune entrée de la table achievements n'a été trouvé
                throw new UserAchievementsNotFoundException("Couldn't find users achievements");
            }
        }
    }

    public override string ToString()
    {
        return "username : "
            + this.Username
            + " elo : "
            + this.Elo
            + " date d'inscription : "
            + this.DateInscription
            + " photo : "
            + this.Photo
            + " exp : "
            + this.Experience
            + " nb parties jouées : "
            + this.NombreParties
            + " nb parties gagnées : "
            + this.NombrePartiesGagnees;
    }

    // test purposes may be reworked later to fit front end needs
    public PlayerPublicData PlayerInfos()
    {
        return new PlayerPublicData(
            this.Username,
            this.Elo,
            this.Photo,
            this.DateInscription,
            this.Experience,
            this.NombreParties,
            this.NombrePartiesGagnees
        );
    }

    public async Task UpdateAvatar(ServerDbContext dbContext, int photo)
    {
        if (this.Username == "Guest")
        {
            this.Photo = photo;
        }
        else
        {
            var userUpdate = dbContext.Users.SingleOrDefault(u => u.UserName == this.Username);
            if (userUpdate != null)
            {
                userUpdate.Photo = photo;
                await dbContext.SaveChangesAsync();
            }
            else
            {
                throw new UserUpdateException("couldn't update user's avatar");
            }
        }
    }

    public int GetElo()
    {
        return this.Elo;
    }

    public string AchievementsToString()
    {
        var achievements = "";
        if (this._achievements == null)
        {
            throw new UserAchievementsNotFoundException("_achievements attribute is null");
        }
        if (this.Username != "Guest")
        {
            foreach (var elmt in this._achievements)
            {
                achievements += elmt.Key + " : " + elmt.Value + "\n";
            }
        }
        else
        {
            achievements += "this user is a guest, no achievements available";
        }

        return achievements;
    }
}
