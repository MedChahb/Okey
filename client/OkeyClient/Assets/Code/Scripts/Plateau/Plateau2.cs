using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Plateau2 : MonoBehaviour
{
    public GameObject plateauPanel;
    public GameObject confirmationPanel;
    public GameObject ParamPanel;

    // Fonction pour activer le Confirmation_Panel et désactiver le Plateau_Panel
    public void ShowConfirmationPanel()
    {
        confirmationPanel.SetActive(true);
        plateauPanel.SetActive(false);
    }

    // Fonction pour activer le Plateau_Panel et désactiver le Confirmation_Panel
    public void ShowPlateauPanel()
    {
        plateauPanel.SetActive(true);
        confirmationPanel.SetActive(false);
        ParamPanel.SetActive(false);
    }

    public void ShowParamPanel()
    {
        ParamPanel.SetActive(true);
        confirmationPanel.SetActive(false);
        plateauPanel.SetActive(false);
    }

    //Fonction Pour retourner a la page D'accuile
    public void ChangeScene()
    {
        SceneManager.LoadScene(0);
    }
}
