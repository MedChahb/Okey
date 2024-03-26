using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarPanelGear : MonoBehaviour
{
    public GameObject Avatar;
    public GameObject InfoPanel;

    public void showConnexionPage()
    {
        Avatar.SetActive(false);
    }

    public void ShowPageAcceuil()
    {
        InfoPanel.SetActive(false);
        Avatar.SetActive(true);
    }

    public void ShowInfoPanel()
    {
        InfoPanel.SetActive(true);
    }

    public void ShowDecoPanel()
    {
        InfoPanel.SetActive(false);
        Avatar.SetActive(false);
    }
}
