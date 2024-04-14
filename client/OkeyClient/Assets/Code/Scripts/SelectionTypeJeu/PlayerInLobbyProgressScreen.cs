using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInLobbyProgressScreen : MonoBehaviour
{
    public GameObject cancelPopUp;

    // Start is called before the first frame update
    void Start()
    {
        cancelPopUp.SetActive(false);
    }

    // Update is called once per frame
    void Update() { }
}
