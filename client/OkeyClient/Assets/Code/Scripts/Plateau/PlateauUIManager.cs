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

    public TextMeshProUGUI bonneChanceLabel;
    public TextMeshProUGUI bienJouerLabel;
    public TextMeshProUGUI superCombatLabel;
    public TextMeshProUGUI merciLabel;
    public TextMeshProUGUI aieLabel;

    public TextMeshProUGUI gameWinningLabel;

    public TextMeshProUGUI ambienceLabel;
    public TextMeshProUGUI effetSonoreLabel;

    public AudioSource audioSourceAmbience;
    public AudioSource audioSourceSoundEffects;

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
        audioSourceAmbience.volume = UIManager.singleton.backgroundMusic;
        audioSourceSoundEffects.volume = UIManager.singleton.soundEffects;
        if (UIManager.singleton.language)
        {
            quiteLabel.text = "Quit the game";
            cancelLabel.text = "Cancel";
            gameWinningLabel.text = "Game winner";
            ambienceLabel.text = "Ambient Music";
            effetSonoreLabel.text = "Sound effect";
            quitPopupTitle.text = "Quit game?";
            bonneChanceLabel.text = "Good luck";
            bienJouerLabel.text = "Well done";
            superCombatLabel.text = "Great fight";
            merciLabel.text = "Thank you";
            aieLabel.text = "Ouch";
        }
        else
        {
            quiteLabel.text = "Quitter la partie";
            cancelLabel.text = "Annuler";
            gameWinningLabel.text = "Gagnant de la partie";
            ambienceLabel.text = "Ambiance Musique";
            effetSonoreLabel.text = "Effet sonore";
            quitPopupTitle.text = "Quitter la partie?";
            bonneChanceLabel.text = "Bonne chance";
            bienJouerLabel.text = "Bien joué";
            superCombatLabel.text = "Super combat";
            merciLabel.text = "Merci";
            aieLabel.text = "Aïe";
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
