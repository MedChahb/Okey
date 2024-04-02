namespace OkeyServer.Misc;

public class Room
{
    /// <summary>
    /// Nom de la room
    /// </summary>
    private string _roomName;

    /// <summary>
    /// Capacite maximale de la room
    /// </summary>
    private int _capacite;

    /// <summary>
    /// Nombre de joueurs actuels dans la room
    /// </summary>
    private int _nbJoueurs;

    /// <summary>
    /// Liste des identifiants joueurs
    /// </summary>
    private List<string> _joueursId;

    public Room(string roomName)
    {
        this._roomName = roomName;
        this._capacite = 4;
        this._nbJoueurs = 0;
        this._joueursId = new List<string>();
    }

    /// <summary>
    /// Permet d'ajouter un joueur dans la room
    /// </summary>
    /// <param name="joueurId">Identifiant SignalR du joueur</param>
    /// <returns>Un booleen true: Que le joueur a bien ete insere; false: Signal que la room est complete maintenant; null: La room est pleine</returns>
    public bool? JoueurJoinRoom(string joueurId)
    {
        if (this._joueursId.Count >= this._capacite)
        {
            return null;
        }
        this._nbJoueurs++;
        this._joueursId.Add(joueurId);
        if (this._nbJoueurs == this._capacite)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Permet de retirer un joueur
    /// </summary>
    /// <param name="joueurId">Identifiant SignalR du joueur</param>
    /// <returns>Retourne un booleen: true:Signale si on rend une room incomplete ; false: L'insertion s'est bien faite</returns>
    public bool JoueurLeaveRoom(string joueurId)
    {
        if (this._nbJoueurs == this._capacite)
        {
            /* Envoyer signal qu'une room est disponnible */
            this._nbJoueurs--;
            this._joueursId.Remove(joueurId);
            return true;
        }
        else
        {
            this._nbJoueurs--;
            this._joueursId.Remove(joueurId);
            return false;
        }
    }

    /// <summary>
    /// Getter de l'attribut _roomName
    /// </summary>
    /// <returns>L'attribut roomName</returns>
    public string GetRoomName() => this._roomName;

    /// <summary>
    /// Getter de l'attribut nombre de joueurs
    /// </summary>
    /// <returns>L'attribut _nbJoueurs</returns>
    public int GetNbCurrent() => this._nbJoueurs;

    /// <summary>
    /// Getter de l'attribut Capacite
    /// </summary>
    /// <returns>L'attribut _capacite</returns>
    public int GetCapacity() => this._capacite;

    /// <summary>
    /// Getter de la liste des identifiants joueurs
    /// </summary>
    /// <returns>Liste des jouersIds</returns>
    public List<string> GetUserIds() => this._joueursId;
}
