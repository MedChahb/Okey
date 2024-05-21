using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlateauUIManager : MonoBehaviour
{
    public static PlateauUIManager Instance { get; private set; }

    public GameObject playerAvatars;

    public GameObject FindPartiePanel;

    public TextMeshProUGUI quitPopupTitle;
    public TextMeshProUGUI quiteLabel;
    public TextMeshProUGUI cancelLabel;

    public TextMeshProUGUI gameWinningLabel;

    public TextMeshProUGUI ambienceLabel;
    public TextMeshProUGUI effetSonoreLabel;

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

    void Start()
    {
        if (UIManager.singleton.language)
        {
            quiteLabel.text = "Quit the game";
            cancelLabel.text = "Cancel";
            gameWinningLabel.text = "Game winner";
            ambienceLabel.text = "Ambient Music";
            effetSonoreLabel.text = "Sound effect";
            quitPopupTitle.text = "Quit game?";
        }
        else
        {
            quiteLabel.text = "Quitter la partie";
            cancelLabel.text = "Annuler";
            gameWinningLabel.text = "Gagnant de la partie";
            ambienceLabel.text = "Ambiance Musique";
            effetSonoreLabel.text = "Effet sonore";
            quitPopupTitle.text = "Quitter la partie?";
        }
    }

    public void QuitterPartie()
    {
        SignalRConnector._hubConnection.StopAsync();
        Chevalet.neverReceivedChevalet = true;
        Chevalet.PiocheIsVide = false;
        SceneManager.LoadScene("Acceuil");
    }
}
