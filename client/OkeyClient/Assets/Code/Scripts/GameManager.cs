using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton; // this is the manager of the game that can be accessed globally thanks to the static keyword
    public bool language = true; // true = English, false = French - logic may change later on

    // Start is called before the first frame update
    void Start()
    {
        // Check if an instance already exists
        if (singleton == null)
        {
            // If not, set the singleton to this instance and make sure it persists across scene loads
            singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (singleton != this)
        {
            // If a different instance exists, destroy this one to avoid duplicates
            Destroy(gameObject);
        }
    }


    void Update(){ //Principal gameloop
        /* mise en place Etc
        ..
        ..
        lancement du tour d'un joueur
        */
        /*
        just in case :
        Tuile[] TilesArray = Chevalet.GetTilesPlacementInChevaletTab();
        Chevalet.PrintTilesArrayForTest();
        */

    }
}
