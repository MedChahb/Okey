using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAvatarsLobby : MonoBehaviour
{
    public static PlayerAvatarsLobby Instance { get; private set; }

    public SpriteRenderer mainPlayerRenderer;
    public SpriteRenderer player2Renderer;
    public SpriteRenderer player3Renderer;
    public SpriteRenderer player4Renderer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void LoadAndDisplayAvatars(List<Sprite> avatars)
    {
        if (avatars.Count > 0)
        {
            SetSpriteAndScale(mainPlayerRenderer, avatars[0]);

            if (avatars.Count > 1)
                SetSpriteAndScale(player2Renderer, avatars[1]);

            if (avatars.Count > 2)
                SetSpriteAndScale(player3Renderer, avatars[2]);

            if (avatars.Count > 3)
                SetSpriteAndScale(player4Renderer, avatars[3]);
        }
    }

    private void SetSpriteAndScale(SpriteRenderer renderer, Sprite sprite)
    {
        renderer.sprite = sprite;
        if (sprite != null)
        {
            float widthScale = 150f / sprite.bounds.size.x;
            float heightScale = 150f / sprite.bounds.size.y;
            float scale = Mathf.Min(widthScale, heightScale);
            renderer.transform.localScale = new Vector3(scale, scale, 1);
        }
    }
}
