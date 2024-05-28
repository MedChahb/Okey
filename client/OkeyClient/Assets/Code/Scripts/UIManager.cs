using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager singleton; // this is the manager of the game that can be accessed globally thanks to the static keyword
    public bool language = true; // true = English, false = French - logic may change later on

    [SerializeField]
    private JoueurManager JoueurManager;

    public float backgroundMusic = 1f;
    public float soundEffects = 1f;

    private void Awake()
    {
        Debug.Log("[UIManager] Awake called.");

        if (singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (singleton != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start() { }
}
