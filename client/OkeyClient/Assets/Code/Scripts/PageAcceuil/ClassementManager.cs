using System.Collections.Generic;
using LogiqueJeu.Joueur;
using UnityEngine;

public class ClassementManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
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
                    $"Nom d'utilisateur: {joueur.NomUtilisateur}, Classement: {joueur.Classement}"
                );
            }
        }
        else
        {
            Debug.Log("Erreur lors de la récupération des classements");
        }
    }
}
