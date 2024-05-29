using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ParametreScreen : MonoBehaviour
{
    public Image parametresImage; // Reference to the image object

    [SerializeField]
    private Button BackBtn;

    [SerializeField]
    private TextMeshProUGUI soundEffects;

    [SerializeField]
    private TextMeshProUGUI music;

    [SerializeField]
    private TextMeshProUGUI language;

    public Slider volumeSliderAmbience;
    public Slider volumeSliderEffects;
    public Slider volumeSliderEffects2;

    public AudioSource audioSourceAmbience;

    public void Start()
    {
        volumeSliderAmbience.value = UIManager.singleton.backgroundMusic;
        volumeSliderEffects.value = UIManager.singleton.soundEffects;
        volumeSliderEffects2.value = UIManager.singleton.soundEffects;

        BackBtn.onClick.AddListener(onBackBtnClicked);
        volumeSliderAmbience.onValueChanged.AddListener(OnVolumeSliderValueChangedAmbience);
        volumeSliderEffects2.onValueChanged.AddListener(OnVolumeSliderValueChangedEffects);
    }

    private void onFrBtnClicked()
    {
        UIManager.singleton.language = false;
    }

    private void onEnBtnClicked()
    {
        UIManager.singleton.language = true;
    }

    private void onBackBtnClicked()
    {
        parametresImage.gameObject.SetActive(false);
    }

    public void Update()
    {
        if (UIManager.singleton.language == true) // EN
        {
            soundEffects.text = "Sound Effects";
            music.text = "Music";
            language.text = "Language";
        }
        else
        {
            soundEffects.text = "Effet Sonores";
            music.text = "Ambiance Musique";
            language.text = "Langue";
        }
    }

    private void OnVolumeSliderValueChangedAmbience(float value)
    {
        audioSourceAmbience.volume = value;
        UIManager.singleton.backgroundMusic = value;
    }

    private void OnVolumeSliderValueChangedEffects(float value)
    {
        UIManager.singleton.soundEffects = value;
    }
}
