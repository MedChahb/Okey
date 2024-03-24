using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManagerPAcceuil : MonoBehaviour
{
    [SerializeField]
    private Button playBtn;

    [SerializeField]
    private Button paramBtn;

    [SerializeField]
    private Button connexionBtn;

    [SerializeField]
    private TextMeshProUGUI playBtnTxt;

    [SerializeField]
    private int sceneId;

    [SerializeField]
    private ParametreScreen parametres;

    [SerializeField]
    private LogInScreen login;

    // Start is called before the first frame update
    void Start()
    {
        playBtn.onClick.AddListener(onPlayBtnClicked);
        paramBtn.onClick.AddListener(onSettingsClicked);
        connexionBtn.onClick.AddListener(onLoginClicked);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.singleton.language)
        {
            playBtnTxt.text = "Play";
        }
        else
        {
            playBtnTxt.text = "Jouer";
        }
    }

    void onPlayBtnClicked()
    {
        SceneManager.LoadScene(sceneId);
        Debug.Log("Scene changed to " + sceneId);
    }

    void onSettingsClicked()
    {
        if (parametres.parametresImage != null)
        {
            // Toggle the active state of the image
            parametres.parametresImage.gameObject.SetActive(
                !parametres.parametresImage.gameObject.activeSelf
            );
        }
        else
        {
            Debug.LogWarning("Parametres Image reference is not set!");
        }
    }

    void onLoginClicked()
    {
        if (login.connexionImage != null)
        {
            // Toggle the active state of the image
            login.connexionImage.gameObject.SetActive(
                !login.connexionImage.gameObject.activeSelf
            );
        }
        else
        {
            Debug.LogWarning("Login Image reference is not set!");
        }
    }
}
