using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlateauUIManager : MonoBehaviour
{
    public static PlateauUIManager Instance { get; private set; }

    public GameObject playerAvatars;

    public GameObject FindPartiePanel;

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

    public void QuitterPartie()
    {
        SignalRConnector._hubConnection.StopAsync();
        Chevalet.neverReceivedChevalet = true;
        Chevalet.PiocheIsVide = false;
        SceneManager.LoadScene("Acceuil");
    }
}
