using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 


public class ParametreBtn : MonoBehaviour
{
    public Image parametreImage;
    public bool isParametreOpen = false;


    // Start is called before the first frame update
    void Start()
    {
        parametreImage = GameObject.Find("Parametres").GetComponent<Image>();
    }

    // // Update is called once per frame
    // void Update()
    // {

    // }


    public void ShowParametre()
    {
        parametreImage.enabled = true;
        Debug.Log("Parametre button clicked");
    }
}
