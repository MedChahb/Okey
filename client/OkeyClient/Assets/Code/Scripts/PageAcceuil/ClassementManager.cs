using System.Collections.Generic;
using LogiqueJeu.Joueur;
using TMPro;
using UnityEngine;

public class ClassementManager : MonoBehaviour
{
    public GameObject playerEntryPrefab; // Pour les entrées dynamiques dans le ScrollView
    public Transform contentPanel; // Content du ScrollView
    public GameObject ClassementPanel;

    public GameObject AvatarPanel;
    public GameObject ClassementAvantConnexion;
    public TextMeshProUGUI[] rankTexts;
    public TextMeshProUGUI[] userTexts;
    public TextMeshProUGUI[] eloTexts;

    void Start()
    {
        Debug.Log("Fetching classements...");
        // L'appel ci-dessous est configuré pour récupérer uniquement les 5 meilleurs joueurs
        JoueurManager.Instance.StartFetchClassements(null, 5, OnClassementsReceived);

        JoueurManager.Instance.StartFetchClassements(null, 0, OnAllClassementsReceived); // Assumer que 0 indique "tous les joueurs"
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

    private void OnAllClassementsReceived(List<Joueur> joueurs)
    {
        // Vérification si la liste des joueurs est nulle
        if (joueurs == null)
        {
            Debug.Log("Liste des joueurs reçue est nulle.");
            return;
        }

        if (joueurs.Count > 0)
        {
            foreach (var joueur in joueurs)
            {
                // Vérifier si le prefab ou le contentPanel est nul
                if (playerEntryPrefab == null || contentPanel == null)
                {
                    Debug.LogError("Prefab ou Content Panel non assigné dans l'éditeur.");
                    return; // Sortie de la fonction pour éviter des erreurs
                }

                GameObject newEntry = Instantiate(playerEntryPrefab, contentPanel);
                if (newEntry == null)
                {
                    Debug.LogError("Instantiation du prefab a échoué.");
                    continue; // Continue avec le prochain joueur
                }

                // Vérification et récupération des composants
                var rankText = newEntry.transform.Find("Rank")?.GetComponent<TextMeshProUGUI>();
                var userText = newEntry.transform.Find("UserId")?.GetComponent<TextMeshProUGUI>();
                var eloText = newEntry.transform.Find("Score")?.GetComponent<TextMeshProUGUI>();

                if (rankText == null || userText == null || eloText == null)
                {
                    Debug.LogError(
                        "Un ou plusieurs composants TextMeshProUGUI sont manquants sur le prefab."
                    );
                    continue; // Continue avec le prochain joueur
                }

                rankText.text = joueur.Classement.ToString();
                userText.text = joueur.NomUtilisateur;
                eloText.text = joueur.Elo.ToString();
            }
        }
        else
        {
            Debug.Log("Aucun joueur trouvé dans la liste.");
        }
    }

    public void BackAcceuil()
    {
        ClassementAvantConnexion.SetActive(false);
        AvatarPanel.SetActive(true);
        ClassementPanel.SetActive(false);
    }
}
