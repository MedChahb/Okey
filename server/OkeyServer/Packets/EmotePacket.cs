namespace OkeyServer.Packets;

/// <summary>
/// Représente un paquet de données pour une émote.
/// </summary>
public class EmotePacket
{
    /// <summary>
    /// Obtient ou définit le nom de l'émote.
    /// </summary>
    public string? EmoteName { get; set; }
}
