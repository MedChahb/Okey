using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManagerPFormulaire : MonoBehaviour
{
    [SerializeField]
    private Button backBtn;

    [SerializeField]
    private TextMeshProUGUI partieSimpleTxt;

    [SerializeField]
    private int SceneId;

    // Start is called before the first frame update
    void Start()
    {
        backBtn.onClick.AddListener(onBackBtnClicked);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.singleton.language) // En
        {
            partieSimpleTxt.text = "Texte en anglais";
        }
        else // Fr
        {
            partieSimpleTxt.text = "Partie Simple";
        }
    }

    void onBackBtnClicked()
    {
        SceneManager.LoadScene(SceneId);
    }
}
