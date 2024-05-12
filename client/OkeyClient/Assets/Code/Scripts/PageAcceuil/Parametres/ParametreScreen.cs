using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ParametreScreen : MonoBehaviour
{
    public Image parametresImage; // Reference to the image object

    [SerializeField]
    private TextMeshProUGUI titleCard;

    [SerializeField]
    private Button enBtn;

    [SerializeField]
    private Button frBtn;

    [SerializeField]
    private Button BackBtn;

    [SerializeField]
    private TextMeshProUGUI soundEffects;

    [SerializeField]
    private TextMeshProUGUI music;

    [SerializeField]
    private TextMeshProUGUI language;

    [SerializeField]
    private Image enBackground;

    [SerializeField]
    private Image frBackground;

    public void Start()
    {
        enBtn.onClick.AddListener(onEnBtnClicked);
        frBtn.onClick.AddListener(onFrBtnClicked);
        BackBtn.onClick.AddListener(onBackBtnClicked);
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
            //titleCard.text = "Settings";
            soundEffects.text = "Sound Effects";
            music.text = "Music";
            language.text = "Language";
        }
        else
        {
            //titleCard.text = "Parametres";
            soundEffects.text = "Effet Sonores";
            music.text = "Ambiance Musique";
            language.text = "Language";
        }

        Color color;
        if (ColorUtility.TryParseHtmlString("#39A24A", out color))
        {
            enBackground.color = UIManager.singleton.language ? color : Color.clear;
            frBackground.color = UIManager.singleton.language ? Color.clear : color;
        }
        else
        {
            Debug.LogError("Invalid color format!");
        }
    }
}
