using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager singleton;  // this is basically the manager of the game (THAT CAN BE ACCESSED BY DOING GAMEMANAGER.SINGLETON THANKS TO THE STATIC KEYWORD)
    public bool language = true; // true = English, false = French - logic may change later on


    // Start is called before the first frame update
    void Start()
    {
        singleton = this; // singleton equals to this instance of the game manager 
        DontDestroyOnLoad(gameObject); // this will make sure that the game manager is not destroyed when we change scenes
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
