using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Retour : MonoBehaviour
{
    // Référence au panel que vous souhaitez activer/désactiver
    public GameObject panel;

    // Cette méthode est appelée pour retourner à la page d'accueil
    public void RetourPageAccueil()
    {
        SceneManager.LoadSceneAsync(0);
    }

    // Méthode pour activer/désactiver le panel
    public void TogglePanel()
    {
        // Change l'état d'activation du panel (true devient false et vice versa)
        panel.SetActive(!panel.activeSelf);
    }

    public void DesactiverPanel()
    {
        panel.SetActive(false);
    }
}
