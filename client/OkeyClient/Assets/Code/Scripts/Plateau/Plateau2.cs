using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Plateau2 : MonoBehaviour
{
    public GameObject PlateauPanel;
    public GameObject ConfirmationPanel;
    public GameObject ParamPanel;

    // Fonction pour activer le Confirmation_Panel et désactiver le Plateau_Panel
    public void ShowConfirmationPanel()
    {
        ConfirmationPanel.SetActive(true);
        PlateauPanel.SetActive(false);
    }

    // Fonction pour activer le Plateau_Panel et désactiver le Confirmation_Panel
    public void ShowPlateauPanel()
    {
        PlateauPanel.SetActive(true);
        ConfirmationPanel.SetActive(false);
        ParamPanel.SetActive(false);
    }

    public void ShowParamPanel()
    {
        ParamPanel.SetActive(true);
        ConfirmationPanel.SetActive(false);
        PlateauPanel.SetActive(false);
    }

    //Fonction Pour retourner a la page D'accuile
    public void ChangeScene()
    {
        SceneManager.LoadScene(0);
    }
}
