namespace OkeyServer.Hubs;

using Microsoft.AspNetCore.SignalR;

/// <summary>
/// Hub de communication entre les clients et le serveur
/// </summary>
public sealed class OkeyHub : Hub<IOkeyHub>
{
    // necessité d'avoir accès à l'ensemble des rooms ici

    /// <summary>
    /// On ajoute automatiquement le client au groupe Hub,
    /// ce groupe contient tous les clients qui ne sont pas dans une partie
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        try
        {
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, "Hub");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        await base.OnConnectedAsync();
    }

    /// <summary>
    /// Envoi un message à tous lees membres d'un groupe
    /// </summary>
    /// <param name="lobbyName"> groupe qui reçoit le message de test</param>
    /// <param name="message"> message envoyé à tous les membres du groupe</param>
    public async Task SendToLobby(string lobbyName, string message)
    {
        try
        {
            await this.Clients.Group(lobbyName).ReceiveMessage($"{message}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    /// <summary>
    /// Effectue la connection au lobby correspondant au lobbyName si ce lobby est joignable
    /// </summary>
    /// <param name="lobbyName"> chaine de caractère correspondant au lobby souhaitant être rejoint </param>
    public async Task LobbyConnection(string lobbyName)
    {
        if ( /* vérifie que lobbyname correspond à un lobby existant et disponible*/
            true
        )
        {
            /* soit on assigne automatiquement le lobby soit le joueur à le choix  enlever lobbyname si auto*/
            try
            {
                await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, "Hub");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await this.Clients.Caller.ReceiveMessage(
                    $"An error Occured or you tried to connect to a lobby from another lobby \nException : {e}"
                );
                throw;
            }

            try
            {
                await this.Groups.AddToGroupAsync(this.Context.ConnectionId, lobbyName);
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    $"{e} \nAn error occured while user was added to the group {lobbyName}"
                );
                throw;
            }
            await this
                .Clients.Group(lobbyName)
                .ReceiveMessage($"Player {this.Context.ConnectionId} joined the lobby {lobbyName}");
        }
    }

    /// <summary>
    /// deconnecte le client du lobby en question
    /// </summary>
    /// <param name="lobbyName"> nom du lobby à quitter </param>
    public async Task LobbyDisconnection(string lobbyName)
    {
        try
        {
            await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, lobbyName);
            await this
                .Clients.Group(lobbyName)
                .ReceiveMessage($"Player {this.Context.ConnectionId} left the lobby!");
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, "Hub");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
