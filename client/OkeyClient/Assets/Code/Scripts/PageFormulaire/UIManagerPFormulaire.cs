using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManagerPFormulaire : MonoBehaviour
{
    [SerializeField]
    private Button backBtn;

    [SerializeField]
    private int SceneId;

    // Start is called before the first frame update
    void Start()
    {
        backBtn.onClick.AddListener(onBackBtnClicked);
    }

    // Update is called once per frame
    void Update() { }

    void onBackBtnClicked()
    {
        SceneManager.LoadScene(SceneId);
    }
}
