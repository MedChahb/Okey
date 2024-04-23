using System.Collections.Generic;
using LogiqueJeu.Joueur;
using UnityEngine;

public class ClassementManager : MonoBehaviour
{
    void Start()
    {
        //Test .....;
        Debug.Log("Fetching classements...");
        // Appel pour récupérer les 5 meilleurs joueurs
        JoueurManager.Instance.StartFetchClassements(null, 5, OnClassementsReceived);
    }

    // Callback appelé avec les résultats du classement
    private void OnClassementsReceived(List<Joueur> joueurs)
    {
        if (joueurs != null)
        {
            Debug.Log("Classement reçu:");
            foreach (var joueur in joueurs)
            {
                Debug.Log(
                    $"Nom d'utilisateur: {joueur.NomUtilisateur}, Classement: {joueur.Classement}, Elo: {joueur.Elo}"
                );
            }
        }
        else
        {
            Debug.Log("Erreur lors de la récupération des classements");
        }
    }
}
