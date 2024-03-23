using System;
using UnityEngine;

public class Chevalet : MonoBehaviour
{
    public GameObject[] placeholders = new GameObject[28]; // Tableau des 28 Placeholders (la grille)

    void Start()
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
    }

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
            int indexTab = currentPlaceholderNumber-1;

            // Vérifier s'il y a un placeholder à droite et s'il n'est pas plein
            if (currentPlaceholderNumber < 29 && placeholders[(indexTab+1)%28].transform.childCount <= 1)
            {
                // Déplacer la première tuile du placeholder actuel vers le placeholder à droite
                GameObject tile = placeholder.transform.GetChild(0).gameObject;
                tile.GetComponent<Tuile>().AttachToPlaceholder(placeholders[(indexTab+1)%28]);

                // Appeler récursivement la fonction UpdateTiles sur le placeholder à droite
                UpdateTiles(placeholders[(indexTab+1)%28]);
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


}
