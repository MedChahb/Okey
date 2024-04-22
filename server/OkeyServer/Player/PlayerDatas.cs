namespace OkeyServer.Player;

using System.Collections.Concurrent;
using Data;
using Exceptions;

/// <summary>
/// Classe contenant toutes les données d'un utilisateur connecté
/// </summary>
public class PlayerDatas
{
    private string _username;
    private int _elo;
    private int _photo;
    private DateTime _dateInscription;
    private int _experience;
    private int _nombreParties;
    private int _nombrePartiesGagnees;
    private ConcurrentDictionary<string, bool> _achievements;
    private readonly ServerDbContext _dbContext;

    /// <summary>
    /// Initialisation de la classe PlayerDatas
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="username"></param>
    public PlayerDatas(ServerDbContext dbContext, string username)
    {
        this._dbContext = dbContext;
        this._username = username;

        var player = this._dbContext.Users.FirstOrDefault(user => user.UserName == username);

        if (player != null)
        {
            this._elo = player.Elo;
            this._photo = player.Photo;
            this._experience = player.Experience;
            this._nombreParties = player.NombreParties;
            this._nombrePartiesGagnees = player.NombrePartiesGagnees;
            this._dateInscription = player.DateInscription;

            /// partie Achievements :
            this._achievements = new ConcurrentDictionary<string, bool>();
            var achievDbContext = this._dbContext.Achievements;
            if (achievDbContext != null)
            {
                var achievements = achievDbContext.FirstOrDefault(ach =>
                    ach.Utilisateur != null
                    && ach.Utilisateur.UserName != null
                    && ach.Utilisateur.UserName.Equals(username, StringComparison.Ordinal)
                );
                if (achievements != null)
                {
                    /// ajouter tous les achievements au dictionnaire, à la main ;'(
                    bool added = true;
                    added =
                        added
                        && this._achievements.TryAdd("Jouer5Parties", achievements.Jouer5Parties);
                    added =
                        added
                        && this._achievements.TryAdd("GagnerUneFois", achievements.GagnerUneFois);
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
                        "Couldn't find " + this._username + " achievements"
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
            throw new UserAchievementsNotFoundException("Couldn't find user : " + this._username);
        }
    }

    /// <summary>
    /// renvoi un booléen indiquant si le succès
    /// </summary>
    /// <param name="achievement"></param>
    /// <returns></returns>
    public bool CheckAchievement(string achievement)
    {
        return this._achievements[achievement];
    }

    /// <summary>
    ///  Fonction asynchrone qui met à jour les statistiques d'un utilisateur en les incrémentant des valeurs fournies en paramètres
    /// </summary>
    /// <param name="elo"> le elo à rajouter au elo du joueur</param>
    /// <param name="experience"> l'experience à rajouter à l'experience du joueur </param>
    /// <param name="nbPartiesGagnees"> booléen indiquant si le nombre de partie gagnées doit être incrémenté, faux par défaut</param>
    /// <param name="nbParties"> booléen indiquant si le nombre de parties doit être incrementé, vrai par défaut</param>
    public async Task UpdateStats(
        int elo,
        int experience,
        bool nbPartiesGagnees = false,
        bool nbParties = true
    )
    {
        this._elo += elo;
        this._experience += experience;
        if (nbParties)
        {
            this._nombreParties++;
        }
        if (nbPartiesGagnees)
        {
            this._nombrePartiesGagnees++;
        }
        var userUpdate = this._dbContext.Users.SingleOrDefault(u => u.UserName == this._username);
        if (userUpdate != null)
        {
            userUpdate.Elo = this._elo;
            userUpdate.Experience = this._experience;
            userUpdate.NombreParties = this._nombreParties;
            userUpdate.NombrePartiesGagnees = this._nombrePartiesGagnees;
            await this._dbContext.SaveChangesAsync();
        }
        else
        {
            this._elo -= elo;
            this._experience -= experience;
            if (nbParties)
            {
                this._nombreParties--;
            }
            if (nbPartiesGagnees)
            {
                this._nombrePartiesGagnees--;
            }
            /// exception erreur lors de la recuperation du profil
            throw new UserUpdateException(
                "Couldn't fetch user :" + this._username + " from the database"
            );
        }
    }

    /// <summary>
    /// met à jour un succès, par défaut le dévérouille et si value est renseigné le met à la valeur de value
    /// </summary>
    /// <param name="achievement"> titre du succès </param>
    /// <param name="value"> nouvelle valeur du succès</param>
    /// <exception cref="UserAchievementsNotFoundException"> exception indiquant l'échec de récuperations des succès dans la base de donnée</exception>
    public async Task UpdateAchievement(string achievement, bool value = true)
    {
        this._achievements[achievement] = value;

        var achievDbContext = this._dbContext.Achievements;
        if (achievDbContext != null)
        {
            var achievements = achievDbContext.FirstOrDefault(ach =>
                ach.Utilisateur != null
                && ach.Utilisateur.UserName != null
                && ach.Utilisateur.UserName.Equals(this._username, StringComparison.Ordinal)
            );
            if (achievements != null)
            {
                if (achievement.Equals("Jouer5Parties", StringComparison.Ordinal))
                {
                    achievements.Jouer5Parties = value;
                }
                else if (achievement.Equals("GagnerUneFois", StringComparison.Ordinal))
                {
                    achievements.Jouer5Parties = value;
                }
                else
                {
                    throw new UserAchievementsNotFoundException(
                        "Couldn't find the achievement : "
                            + achievement
                            + " in the user : "
                            + this._username
                            + " achievements collection"
                    );
                }

                await this._dbContext.SaveChangesAsync();
            }
            else
            {
                /// exception aaucune ligne achievements correspondat à l'utilisateur n'à été trouvée
                throw new UserAchievementsNotFoundException(
                    "Couldn't find " + this._username + " achievements"
                );
            }
        }
        else
        {
            /// exception aucune entrée de la table achievements n'a été trouvé
            throw new UserAchievementsNotFoundException("Couldn't find users achievements");
        }
    }

    public override string ToString()
    {
        return "username : "
            + this._username
            + " elo : "
            + this._elo
            + " date d'inscription : "
            + this._dateInscription
            + " photo : "
            + this._photo
            + " exp : "
            + this._experience
            + " nb parties jouées : "
            + this._nombreParties
            + " nb parties gagnées : "
            + this._nombrePartiesGagnees;
    }
}
