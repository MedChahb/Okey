using System;
using System.Collections.Generic;
using UnityEngine;

public class Chevalet : MonoBehaviour
{
    public GameObject[] placeholders = new GameObject[28]; // Tableau des 28 Placeholders (la grille)

    private Tuile[,] tuiles2D = new Tuile[2, 14];
    private Stack<Tuile> pileGauche = new Stack<Tuile>();
    private Stack<Tuile> pileDroite = new Stack<Tuile>();

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

        InitializeBoardFromPlaceholders();
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

    // Fonction auxiliaire pour extraire le numéro du placeholder à partir de son nom
    public static int GetPlaceholderNumber(string name)
    {
        string numberString = name.Substring(11);
        int number;
        int.TryParse(numberString, out number);
        return number;
    }

    void InitializeBoardFromPlaceholders()
    {
        for (int i = 0; i < placeholders.Length; i++)
        {
            int x = i / 14;
            int y = i % 14;

            GameObject placeholder = placeholders[i];
            if (placeholder != null)
            {
                Tuile tuile = placeholder.GetComponent<Tuile>();
                if (tuile == null)
                {
                    tuile = placeholder.AddComponent<Tuile>();
                }

                SetTuile(x, y, tuile); // fill the matrice with the values retrieved
            }
        }
    }

    // set le positionnement des tuiles dans la matrice
    public void SetTuile(int x, int y, Tuile tuile)
    {
        if (x >= 0 && x < 2 && y >= 0 && y < 14)
        {
            tuiles2D[x, y] = tuile;
        }
        else
        {
            Debug.LogError("SetTuile: Index out of bounds");
        }
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
}
