using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager_PInformation : MonoBehaviour
{

    [SerializeField] private Button backBtn;
    [SerializeField] private int SceneId;

    // Start is called before the first frame update
    void Start()
    {
        backBtn.onClick.AddListener(onBackBtnClicked);
        
    }

 
    // Update is called once per frame
    void Update()
    {
        
    }


    void onBackBtnClicked()
    {

        SceneManager.LoadScene(SceneId);
    }
}
