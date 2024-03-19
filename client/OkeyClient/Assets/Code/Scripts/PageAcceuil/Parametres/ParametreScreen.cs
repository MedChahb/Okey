using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ParametreScreen : MonoBehaviour
{
    public Image parametresImage; // Reference to the image object

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
        GameManager.singleton.language = false;
    }

    private void onEnBtnClicked()
    {
        GameManager.singleton.language = true;
    }

    private void onBackBtnClicked()
    {
        parametresImage.gameObject.SetActive(false);
    }

    public void Update()
    {
        if (GameManager.singleton.language == true) // EN
        {
            soundEffects.text = "Sound Effects";
            music.text = "Music";
            language.text = "Language";
        }
        else
        {
            soundEffects.text = "Effet Sonores";
            music.text = "Ambiance Musique";
            language.text = "Language";
        }

        Color color;
        if (ColorUtility.TryParseHtmlString("#39A24A", out color))
        {
            enBackground.color = GameManager.singleton.language ? color : Color.white;
            frBackground.color = GameManager.singleton.language ? Color.white : color;
        }
        else
        {
            Debug.LogError("Invalid color format!");
        }
    }
}
