using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Chevalet : MonoBehaviour
{
    public static GameObject[] placeholders = new GameObject[28]; // Tableau des 28 Placeholders (la grille)

    private TuileData[,] tuiles2D = new TuileData[2, 14];
    private Stack<Tuile> pileGauche = new Stack<Tuile>();
    public static GameObject pileGauchePlaceHolder;
    private Stack<Tuile> pileDroite = new Stack<Tuile>();
    public static GameObject pileDroitePlaceHolder;
    public static GameObject jokerPlaceHolder;
    public static GameObject pilePiochePlaceHolder;

    void Start()
    {
        InitPlaceholders();
        this.InitializeBoardFromPlaceholders();
        //PrintTuilesArray();
    }

    void InitPlaceholders()
    {
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
        pileGauchePlaceHolder = GameObject.Find("PileGauchePlaceHolder");
        pileDroitePlaceHolder = GameObject.Find("PileDroitePlaceHolder");
        pilePiochePlaceHolder = GameObject.Find("PiochePlaceHolder");
        jokerPlaceHolder = GameObject.Find("Okey");
        pileGauchePlaceHolder
            .transform.GetChild(0)
            .gameObject.GetComponent<Tuile>()
            .SetIsDeplacable(false);
        pileGauchePlaceHolder
            .transform.GetChild(0)
            .gameObject.GetComponent<Tuile>()
            .SetIsInStack(true);
        pileDroitePlaceHolder
            .transform.GetChild(0)
            .gameObject.GetComponent<Tuile>()
            .SetIsDeplacable(false);
        jokerPlaceHolder
            .transform.GetChild(0)
            .gameObject.GetComponent<Tuile>()
            .SetIsDeplacable(false);
        pilePiochePlaceHolder
            .transform.GetChild(0)
            .gameObject.GetComponent<Tuile>()
            .SetIsDeplacable(false);
        pilePiochePlaceHolder
            .transform.GetChild(0)
            .gameObject.GetComponent<Tuile>()
            .SetIsInPioche(true);
    }

    public GameObject ClosestPlaceholder(Vector3 position)
    {
        GameObject closestPlaceholder = null;
        float closestDistance = Mathf.Infinity;

        // Loop through placeholders using absolute positions
        foreach (GameObject placeholder in placeholders)
        {
            if (placeholder != null)
            {
                float distance = Vector3.Distance(position, placeholder.transform.position); // Absolute position
                if (distance < closestDistance)
                {
                    closestPlaceholder = placeholder;
                    closestDistance = distance;
                }
            }
        }

        // Check specific piles using absolute positions
        float distancePileDroite = Vector3.Distance(
            position,
            pileDroitePlaceHolder.transform.position
        ); // Absolute position

        // Determine closest placeholder based on distance and tile number
        if (distancePileDroite < closestDistance && getTilesNumber() == 15)
        {
            closestDistance = distancePileDroite;
            closestPlaceholder = pileDroitePlaceHolder;
        }
        else if (
            position.y > 0
            && getTilesNumber() == 15 /* et peut gagner*/
        )
        {
            closestPlaceholder = jokerPlaceHolder;
        }

        Debug.Log("Position de la souris:" + position + " - Closest distance:" + closestDistance);
        return closestPlaceholder;
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

    public void draw(bool pioche)
    {
        if (
            getTilesNumber() == 14 /* et c'est mon tour*/
        )
        {
            if (pioche)
            {
                pileGauchePlaceHolder
                    .transform.GetChild(0)
                    .gameObject.GetComponent<Tuile>()
                    .SetIsDeplacable(true);
                pileGauchePlaceHolder
                    .transform.GetChild(0)
                    .gameObject.GetComponent<Tuile>()
                    .SetIsInStack(false);
                //Insérer la tuile suivante de la stack comme tuile fille du pileGauchePlaceHolder et lui passer
            }
            else
            {
                pilePiochePlaceHolder
                    .transform.GetChild(0)
                    .gameObject.GetComponent<Tuile>()
                    .SetIsDeplacable(true);
                pilePiochePlaceHolder
                    .transform.GetChild(0)
                    .gameObject.GetComponent<Tuile>()
                    .SetIsInPioche(false);
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

    private void InitializeBoardFromPlaceholders()
    {
        for (var i = 0; i < placeholders.Length; i++)
        {
            var x = i / 14; // Calculate the row based on index.
            var y = i % 14; // Calculate the column based on index.

            GameObject placeholder = placeholders[i];

            //Debug.Log($"Checking placeholder {i}, Child count: {placeholder.transform.childCount}");

            if (placeholder.transform.childCount > 0)
            {
                // Get the first child of the placeholder
                var child = placeholder.transform.GetChild(0).gameObject;
                var childSpriteRenderer = child.GetComponent<SpriteRenderer>();

                if (childSpriteRenderer != null)
                {
                    // note: "color_value" should be used for naming the sprites
                    var properties = childSpriteRenderer.sprite.name.Split('_');
                    //Debug.Log(properties[0] + properties[1]);

                    if (properties.Length == 2)
                    {
                        var couleur = this.ConvertToFrontendColorToBackendEnumName(properties[0]);
                        Debug.Log(couleur);
                        var num = int.Parse(properties[1]);
                        var isJoker = properties[0] == "FakeJoker";
                        this.tuiles2D[x, y] = new TuileData(couleur, num, isJoker);
                    }
                    else
                    {
                        // If the name does not contain both color and value, log an error or set as null
                        Debug.LogError(
                            $"Child sprite of placeholder at index {i} does not have a properly formatted name."
                        );
                        this.tuiles2D[x, y] = null;
                    }
                }
                else
                {
                    // If there is no SpriteRenderer component, set the corresponding array position to null
                    this.tuiles2D[x, y] = null;
                }
            }
            else //empty placeholder
            {
                // If the placeholder is empty, set the corresponding array position to null
                this.tuiles2D[x, y] = null;
            }
        }
    }

    private CouleurTuile ConvertToFrontendColorToBackendEnumName(string FrontendColor)
    {
        //Debug.Log(FrontendColor);
        return FrontendColor.ToLower() switch
        {
            "yellow" => CouleurTuile.J,
            "black" => CouleurTuile.N,
            "red" => CouleurTuile.R,
            "blue" => CouleurTuile.B,
            "green" => CouleurTuile.V,
            // other cases still needed
            "fakejoker" => CouleurTuile.X,
            _ => CouleurTuile.M, // the okey
        };
    }

    //public void SetTuile(int x, int y, TuileData tuile)
    //{
    //    tuiles2D[x, y] = tuile;
    //}

    //// get Tuile de l'index donnee
    //public TuileData GetTuile(int x, int y)
    //{
    //    if (x >= 0 && x < 2 && y >= 0 && y < 14)
    //    {
    //        return tuiles2D[x, y];
    //    }
    //    else
    //    {
    //        Debug.LogError("GetTuile: Index out of bounds");
    //        return null;
    //    }
    //}

    public TuileData[,] GetTuilesArray()
    {
        return this.tuiles2D;
    }

    private void PrintTuilesArray()
    {
        var sb = new StringBuilder();
        sb.AppendLine("Board State:");

        for (int x = 0; x < this.tuiles2D.GetLength(0); x++)
        {
            for (int y = 0; y < this.tuiles2D.GetLength(1); y++)
            {
                TuileData tile = this.tuiles2D[x, y];
                if (tile != null)
                {
                    sb.AppendLine(
                        $"Tile at [{x},{y}]: Color = {tile.couleur}, Number = {tile.num}, IsJoker = {tile.isJoker}"
                    );
                }
                else
                {
                    sb.AppendLine($"Tile at [{x},{y}]: None");
                }
            }
        }

        Debug.Log(sb.ToString());
    }

    public int getTilesNumber()
    {
        int num = 0;
        foreach (GameObject placeholder in placeholders)
        {
            if (placeholder.transform.childCount == 1)
            {
                num++;
            }
        }
        return num;
    }
}








/*
    to keep just in case :
    public static Tuile[] GetTilesPlacementInChevaletTab()
    {
        // Creation d'un tableau pour stocker le placement courant des tuiles sur le chevalet
        Tuile[] TilesArray = new Tuile[placeholders.Length];

        //loop sur les placeholders
        foreach (GameObject placeholder in placeholders)
        {
            int index = Array.IndexOf(placeholders, placeholder);

            if (placeholder.transform.childCount > 0)
            { // le placeholder a un child -> il contient une tuile -> on la met dans le tableau avec l'index du placeholder parent
                GameObject child = placeholder.transform.GetChild(0).gameObject;
                Tuile tuile = child.GetComponent<Tuile>();

                if (tuile != null)
                {
                    // Place the Tuile in the 1D array
                    TilesArray[index] = tuile;
                }
                else
                {
                    Debug.LogError("Error : placeholder has a child that isn't a Tile");
                }
            }
            else // si le placeholder n'a aucun child -> il est vide -> on met null a l'index correspondant au placeholder dans le tableau
            {
                TilesArray[index] = null;
            }
        }

        return TilesArray;
    }

    public static void PrintTilesArrayForTest()
    {
        // Get the tiles placement array using the method
        Tuile[] TilesArray = GetTilesPlacementInChevaletTab();

        // Check if TilesArray is not null
        if (TilesArray != null)
        {
            // Loop through the TilesArray to print tile number and color
            for (int i = 0; i < TilesArray.Length; i++)
            {
                if (TilesArray[i] != null)
                {
                    // Check if GetValeur() and GetCouleur() are not null
                    if (
                        TilesArray[i].GetValeur().ToString() != null
                        && TilesArray[i].GetCouleur().ToString() != null
                    ) // supposons que les valeurs sont init a 0
                    {
                        Debug.Log(
                            "Tile Number "
                                + (i + 1)
                                + " :"
                                + TilesArray[i].GetValeur().ToString()
                                + ", Tile Color: "
                                + TilesArray[i].GetCouleur().ToString()
                        );
                    }
                    else
                    {
                        Debug.Log("Tile at index " + i + " has null values.");
                    }
                }
                else
                {
                    Debug.Log("Tile at index " + i + " is null.");
                }
            }
        }
        else
        {
            Debug.Log("TilesArray is null.");
        }
    }
    */
