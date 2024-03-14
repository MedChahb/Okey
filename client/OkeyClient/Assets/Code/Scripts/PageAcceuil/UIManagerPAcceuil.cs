using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEditor.SearchService;

public class UIManagerPAcceuil : MonoBehaviour
{
    [SerializeField] private Button playBtn;
    [SerializeField] private Button paramBtn;

    [SerializeField] private TextMeshProUGUI playBtnTxt;

    [SerializeField] private int sceneId;

    [SerializeField] private ParametreScreen parametres;

    // Start is called before the first frame update
    void Start()
    {
        playBtn.onClick.AddListener(onPlayBtnClicked);
        paramBtn.onClick.AddListener(onSettingsClicked);

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
            parametres.parametresImage.gameObject.SetActive(!parametres.parametresImage.gameObject.activeSelf);
        }
        else
        {
            Debug.LogWarning("Parametres Image reference is not set!");
        }
    }
}
