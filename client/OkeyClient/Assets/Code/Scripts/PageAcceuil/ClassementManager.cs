using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
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
    public const int MAX_REQUEST_RETRIES = 5; // Superieur ou égale à 1
    public const int REQUEST_RETRY_DELAY = 1000; // En milisecondes
    private readonly CancellationTokenSource Source = new();

    void Start()
    {
        Debug.Log("Fetching classements...");

        // L'appel ci-dessous est configuré pour récupérer uniquement les 5 meilleurs joueurs
        this.FetchClassementsTopAsync(5);

        // 0 indique "tous les joueurs", 1 indique la première page (30 joueurs à la fois), 2 la deuxième, etc.
        this.FetchClassementsPageAsync(1);
    }

    private async void FetchClassementsTopAsync(int Limit)
    {
        List<Joueur> joueurs = null;
        for (var i = 0; i < MAX_REQUEST_RETRIES; i++)
        {
            try
            {
                joueurs = await API.FetchClassementsTopAsync(Limit, this.Source.Token);
                break;
            }
            catch (HttpRequestException e)
            {
                var Code = e.GetStatusCode();
                if (Code != null)
                {
                    Debug.Log("Request error with response code " + Code);
                    break;
                }
                else
                {
                    Debug.Log("Network error");
                    if (i != MAX_REQUEST_RETRIES - 1)
                    {
                        Debug.Log("Retrying...");
                        try
                        {
                            await Task.Delay(REQUEST_RETRY_DELAY, this.Source.Token);
                        }
                        catch (OperationCanceledException)
                        {
                            Debug.Log("Request cancelled.");
                            break;
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Request cancelled.");
                break;
            }
        }

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

    private async void FetchClassementsPageAsync(int PageNumber)
    {
        List<Joueur> joueurs = null;
        for (var i = 0; i < MAX_REQUEST_RETRIES; i++)
        {
            try
            {
                joueurs = await API.FetchClassementsPageAsync(PageNumber, this.Source.Token);
                break;
            }
            catch (HttpRequestException e)
            {
                var Code = e.GetStatusCode();
                if (Code != null)
                {
                    Debug.Log("Request error with response code " + Code);
                    break;
                }
                else
                {
                    Debug.Log("Network error");
                    if (i != MAX_REQUEST_RETRIES - 1)
                    {
                        Debug.Log("Retrying...");
                        try
                        {
                            await Task.Delay(REQUEST_RETRY_DELAY, this.Source.Token);
                        }
                        catch (OperationCanceledException)
                        {
                            Debug.Log("Request cancelled.");
                            break;
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Request cancelled.");
                break;
            }
        }

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
