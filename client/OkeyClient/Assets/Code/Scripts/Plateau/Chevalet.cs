using System.Collections.Generic;
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
        if (placeholder.transform.childCount > 0)
        {
            // Récupérer la première tuile enfant
            GameObject tile = placeholder.transform.GetChild(0).gameObject;

            // Récupérer le numéro du placeholder actuel
            int currentPlaceholderNumber = int.Parse(placeholder.name.Substring(9));

            // Vérifier s'il y a un placeholder à droite
            if (currentPlaceholderNumber < 28)
            {
                // Récupérer le placeholder suivant
                GameObject nextPlaceholder = GameObject.Find("PlaceHolder" + (currentPlaceholderNumber + 1));

                // Déplacer la tuile vers le placeholder suivant
                tile.GetComponent<Tuile>().AttachToPlaceholder(placeholder);

                // Déplacer récursivement les tuiles suivantes
                UpdateTiles(nextPlaceholder);
            }
        }
    }

}
