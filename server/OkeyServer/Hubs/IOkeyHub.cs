namespace OkeyServer.Hubs;

/// <summary>
/// permet d'avoir un Hub avec typage fort en forçant l'utilisation des méthodes définies ici
/// (qui doivent être définies côté client)
/// et empêchant donc d'envoyer des messages de n'importe quel type avec SendAsync("type","contenu")
/// pour l'instant seul le protocole de trasnmission de message simple est utilisé
/// </summary>
public interface IOkeyHub
{
    Task ReceiveMessage(string message);
}
