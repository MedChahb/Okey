using System.Collections.Generic;
using LogiqueJeu.Joueur;
using TMPro;
using UnityEngine;

public class ClassementManager : MonoBehaviour
{
    public TextMeshProUGUI[] rankTexts;
    public TextMeshProUGUI[] userTexts;
    public TextMeshProUGUI[] eloTexts;

    void Start()
    {
        Debug.Log("Fetching classements...");
        // L'appel ci-dessous est configuré pour récupérer uniquement les 5 meilleurs joueurs
        JoueurManager.Instance.StartFetchClassements(null, 5, OnClassementsReceived);
    }

    private void OnClassementsReceived(List<Joueur> joueurs)
    {
        if (joueurs != null && joueurs.Count > 0)
        {
            Debug.Log("Classement reçu:");
            int displayCount = Mathf.Min(joueurs.Count, 5); // S'assurer de ne traiter que les 5 premiers
            for (int i = 0; i < displayCount; i++)
            {
                rankTexts[i].text = (i + 1).ToString();
                userTexts[i].text = joueurs[i].NomUtilisateur;
                eloTexts[i].text = joueurs[i].Elo.ToString();
            }
            // Effacer les champs restants si moins de 5 joueurs sont reçus
            for (int i = joueurs.Count; i < rankTexts.Length; i++)
            {
                rankTexts[i].text = "";
                userTexts[i].text = "";
                eloTexts[i].text = "";
            }
        }
        else
        {
            Debug.Log("Erreur lors de la récupération des classements ou aucune donnée reçue");
            // Afficher un message d'erreur ou effacer les champs
            for (int i = 0; i < rankTexts.Length; i++)
            {
                rankTexts[i].text = "-";
                userTexts[i].text = "Données non disponibles";
                eloTexts[i].text = "-";
            }
        }
    }
}
