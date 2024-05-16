using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlateauUIManager : MonoBehaviour
{
    public static PlateauUIManager Instance { get; private set; }

    public GameObject playerAvatars;

    // awake
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
}
