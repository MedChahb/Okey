using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chevalet : MonoBehaviour
{
    public GameObject[] placeholders = new GameObject[28]; // Tableau des 28 Placeholders (la grille)

    private Tuile[,] tuiles2D = new Tuile[2, 14];
    private Stack<Tuile> pileGauche = new Stack<Tuile>();
    public GameObject pileGauchePlaceHolder;
    private Stack<Tuile> pileDroite = new Stack<Tuile>();
    public GameObject pileDroitePlaceHolder;

    void Start()
    {
        //InitializePlaceholders();

        // Remplir le tableau avec les placeholders dans la scene
        for (int i = 1; i <= 28; i++) //i commence a 1 car le premier placeholder est "PlaceHolder1"
        {
            GameObject placeholder = GameObject.Find("PlaceHolder" + i);
            if (placeholder != null)
            {
                placeholders[i - 1] = placeholder; // Mettre le placeholder dans le tableau
            }
            else
            {
                Debug.LogError("PlaceHolder" + i + " not found!");
            }
        }

        //StartCoroutine(WaitAndCall(2f));

        InitializeBoardFromPlaceholders();
        //Ajouter event clique sur pileDroitePlaceHolder et déclencher draw()
        PrintTuilesArray();
    }

    //void InitializePlaceholders()
    //{
    //    for (int i = 1; i <= 28; i++)
    //    {
    //        GameObject placeholder = GameObject.Find("PlaceHolder" + i);
    //        if (placeholder != null)
    //        {
    //            placeholders[i - 1] = placeholder;
    //        }
    //        else
    //        {
    //            Debug.LogError("PlaceHolder" + i + " not found!");
    //        }
    //    }
    //}

    public GameObject ClosestPlaceholder(Vector3 position)
    {
        GameObject closestPlaceholder = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject placeholder in placeholders)
        {
            if (placeholder != null)
            {
                float distance = Vector3.Distance(position, placeholder.transform.position);
                if (distance < closestDistance)
                {
                    closestPlaceholder = placeholder;
                    closestDistance = distance;
                }
            }
        }
        if (Vector3.Distance(position, pileDroitePlaceHolder) < closestDistance)
        {
            //Vérifier la distance à la pile
            if (1/*Nombre de tuiles == 15 et */) 
            {
                closestDistance = distance;
                closestPlaceholder = pileDroitePlaceHolder;
            }
            else 
            {
                //Affichage vous ne pouvez pas jetter
            }
        }

        return closestPlaceholder;
    }

    public void draw() {
        if (1/*Nombre de tuiles == 14 et c'est mon tour*/) 
        {
            pileGauchePlaceHolder.GetChild(0).gameObject.GetComponent<Tuile>().SetIsDeplacable(true);
        }
        else 
        {
            pileGauchePlaceHolder.GetChild(0).gameObject.GetComponent<Tuile>().SetIsDeplacable(false);
        }
    }

    public void UpdateTiles(GameObject placeholder)
    {
        // Vérifier si le placeholder a des enfants (des tuiles)
        if (placeholder.transform.childCount > 1)
        {
            // Déterminer le numéro du placeholder actuel
            int currentPlaceholderNumber = GetPlaceholderNumber(placeholder.name);
            int indexTab = currentPlaceholderNumber - 1;

            // Vérifier s'il y a un placeholder à droite et s'il n'est pas plein
            if (
                currentPlaceholderNumber < 29
                && placeholders[(indexTab + 1) % 28].transform.childCount <= 1
            )
            {
                // Déplacer la première tuile du placeholder actuel vers le placeholder à droite
                GameObject tile = placeholder.transform.GetChild(0).gameObject;
                tile.GetComponent<Tuile>().AttachToPlaceholder(placeholders[(indexTab + 1) % 28]);

                // Appeler récursivement la fonction UpdateTiles sur le placeholder à droite
                UpdateTiles(placeholders[(indexTab + 1) % 28]);
            }
        }
    }

    // Fonction auxiliaire pour extraire le numéro du placeholder à partir de son nom
    public static int GetPlaceholderNumber(string name)
    {
        string numberString = name.Substring(11);
        int number;
        int.TryParse(numberString, out number);
        return number;
    }

    //void InitializeBoardFromPlaceholders()
    //{
    //    for (int i = 0; i < placeholders.Length; i++)
    //    {
    //        int x = i / 14;
    //        int y = i % 14;

    //        GameObject placeholder = placeholders[i];
    //        if (placeholder != null)
    //        {
    //            Tuile tuile = placeholder.GetComponent<Tuile>();
    //            if (tuile == null)
    //            {
    //                tuile = placeholder.AddComponent<Tuile>();
    //            }

    //            SetTuile(x, y, tuile); // fill the matrice with the values retrieved
    //        }
    //    }
    //}



    IEnumerator WaitAndCall(float waitTime)
    {
        // Wait for the specified amount of time
        yield return new WaitForSeconds(waitTime);

        // Call your function here
        InitializeBoardFromPlaceholders();
    }

    void InitializeBoardFromPlaceholders()
    {
        Debug.Log("outside loop");

        for (int i = 0; i < placeholders.Length; i++)
        {
            int x = i / 14; // Calculate the row based on index.
            int y = i % 14; // Calculate the column based on index.

            GameObject placeholder = placeholders[i];

            Debug.Log($"Checking placeholder {i}, Child count: {placeholder.transform.childCount}");

            if (placeholder.transform.childCount > 0)
            {
                Debug.Log("inside first if");
                // Get the first child of the placeholder
                GameObject child = placeholder.transform.GetChild(0).gameObject;
                SpriteRenderer childSpriteRenderer = child.GetComponent<SpriteRenderer>();

                if (childSpriteRenderer != null)
                {
                    Debug.Log("inside sec if");

                    // note: "color_value" should be used for naming the sprites
                    string[] properties = childSpriteRenderer.sprite.name.Split('_');
                    Debug.Log(properties[0] + properties[1]);

                    if (properties.Length == 2)
                    {
                        Debug.Log("inside third if");

                        // Create a new Tuile, or use an existing one
                        Tuile tuile = placeholder.AddComponent<Tuile>();

                        // Extract and assign color and value from the sprite's name
                        tuile.SetCouleur(properties[0]);
                        tuile.SetValeur(int.Parse(properties[1]));
                        Debug.Log("here");
                        // Place the Tuile in the 2D array
                        SetTuile(x, y, tuile);
                    }
                    else
                    {
                        // If the name does not contain both color and value, log an error or set as null
                        Debug.LogError(
                            $"Child sprite of placeholder at index {i} does not have a properly formatted name."
                        );
                        tuiles2D[x, y] = null;
                    }
                }
                else
                {
                    // If there is no SpriteRenderer component, set the corresponding array position to null
                    tuiles2D[x, y] = null;
                }
            }
            else
            {
                // If the placeholder is empty, set the corresponding array position to null
                tuiles2D[x, y] = null;
            }
        }
    }

    public void SetTuile(int x, int y, Tuile tuile)
    {
        tuiles2D[x, y] = tuile;
    }

    // get Tuile de l'index donnee
    public Tuile GetTuile(int x, int y)
    {
        if (x >= 0 && x < 2 && y >= 0 && y < 14)
        {
            return tuiles2D[x, y];
        }
        else
        {
            Debug.LogError("GetTuile: Index out of bounds");
            return null;
        }
    }

    public Tuile[,] GetTuilesArray()
    {
        return tuiles2D;
    }

    public void PrintTuilesArray()
    {
        for (int x = 0; x < tuiles2D.GetLength(0); x++)
        {
            for (int y = 0; y < tuiles2D.GetLength(1); y++)
            {
                // Check if there is a Tuile at the current position
                if (tuiles2D[x, y] != null)
                {
                    // If there is a Tuile, print its details
                    Debug.Log(
                        $"Tuile at [{x},{y}]: Color = {tuiles2D[x, y].GetCouleur()}, Value = {tuiles2D[x, y].GetValeur()}"
                    );
                }
                else
                {
                    Debug.Log($"Tuile at [{x},{y}]: None");
                }
            }
        }
    }
}
