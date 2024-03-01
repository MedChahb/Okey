using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class ParametreScreen : MonoBehaviour
{
    [SerializeField]private Image parametresImage; // Reference to the image object
    [SerializeField] private Button enBtn;
    [SerializeField] private Button frBtn;
    [SerializeField] private Button parametreBtn;
    [SerializeField] private Button BackBtn;
    [SerializeField] private TextMeshProUGUI playBtnTxt;

    public void Start()
    {
        parametreBtn.onClick.AddListener(onParametreBtnClicked);
        enBtn.onClick.AddListener(onEnBtnClicked);
        frBtn.onClick.AddListener(onFrBtnClicked);
        BackBtn.onClick.AddListener(onBackBtnClicked);
    }

    // This method is called when the button is pressed
    public void onParametreBtnClicked()
    {
        // Check if the image reference is not null
        // debug.log("parametresImage : " + parametresImage);
        if (parametresImage != null)
        {
            // Toggle the active state of the image
            parametresImage.gameObject.SetActive(!parametresImage.gameObject.activeSelf);
        }
        else
        {
            Debug.LogWarning("Parametres Image reference is not set!");
        }
    }



    private void onFrBtnClicked(){
        GameManager.singleton.language = false;
    }

    private void onEnBtnClicked(){
        GameManager.singleton.language = true;
    }


    private void onBackBtnClicked()
    {
        parametresImage.gameObject.SetActive(false);
    }

    public void Update()
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





}
