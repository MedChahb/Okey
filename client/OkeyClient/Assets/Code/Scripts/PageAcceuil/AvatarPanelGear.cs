using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarPanelGear : MonoBehaviour
{
    public GameObject Avatar;

    public void showConnexionPage()
    {
        Avatar.SetActive(false);
    }
}
