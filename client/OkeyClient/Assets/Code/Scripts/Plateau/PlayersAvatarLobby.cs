using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // This import might not be needed if you're only using SpriteRenderer

public class PlayerAvatarsLobby : MonoBehaviour
{
    public SpriteRenderer mainPlayerRenderer;
    public SpriteRenderer player2Renderer;
    public SpriteRenderer player3Renderer;
    public SpriteRenderer player4Renderer;

    public void LoadAndDisplayAvatars(List<string> playerUsernames)
    {
        if (playerUsernames.Count > 0)
        {
            mainPlayerRenderer.sprite = LoadAvatarForUser(playerUsernames[0]);

            if (playerUsernames.Count > 1)
                player2Renderer.sprite = LoadAvatarForUser(playerUsernames[1]);

            if (playerUsernames.Count > 2)
                player3Renderer.sprite = LoadAvatarForUser(playerUsernames[2]);

            if (playerUsernames.Count > 3)
                player4Renderer.sprite = LoadAvatarForUser(playerUsernames[3]);
        }
    }

    Sprite LoadAvatarForUser(string username)
    {
        string path = "Avatars/" + username.ToLower();
        Sprite avatar = Resources.Load<Sprite>(path);
        if (avatar == null)
        {
            Debug.LogError("Failed to load avatar for user: " + username);
        }
        return avatar;
    }
}
