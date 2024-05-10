using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Plateau2 : MonoBehaviour
{
    public GameObject PlateauPanel;
    public GameObject ConfirmationPanel;
    public GameObject ParamPanel;
    public GameObject EmojiPanel; //Panneau d'emoji

    public Sprite[] emojiSprites;
    public Image EmojiDisplay;
    public Button[] wordButtons; // Boutons pour les mots
    public TextMeshProUGUI gameDisplayText; // Texte affiché sur le plateau de jeu

    // Fonction pour activer le Confirmation_Panel et désactiver le Plateau_Panel
    // Tableau des mots, assurez-vous que cela correspond à l'ordre des boutons


    public void OnWordButtonClicked(int buttonIndex)
    {
        string[] words =
        {
            "Bonne chance",
            "Bien joué !",
            "Super Combat",
            "Merci !",
            "Super !",
            "Aie...."
        };
        Debug.Log($"Button clicked with index: {buttonIndex}");
        if (buttonIndex >= 0 && buttonIndex < words.Length)
        {
            Debug.Log($"Displaying word: {words[buttonIndex]}");
            gameDisplayText.text = words[buttonIndex];
            StartCoroutine(ClearTextAfterDelay(gameDisplayText, 2f));
            EmojiPanel.SetActive(false);
        }
        else
        {
            Debug.LogError(
                $"Index out of range: {buttonIndex}, words array length: {words.Length}"
            );
        }
    }

    IEnumerator ClearTextAfterDelay(TextMeshProUGUI textComponent, float delay)
    {
        yield return new WaitForSeconds(delay);
        textComponent.text = ""; // Efface le texte après le délai
    }

    public void ShowConfirmationPanel()
    {
        ConfirmationPanel.SetActive(true);
        PlateauPanel.SetActive(false);
    }

    public void toggleEmojiPanel()
    {
        EmojiPanel.SetActive(!EmojiPanel.activeSelf);
    }

    // Est appelée quand un bouton émoji est cliqué
    public void ShowEmojiPanel()
    {
        EmojiPanel.SetActive(true);
    }

    public void OnEmojiSelected(int emojiIndex)
    {
        if (emojiIndex < 0 || emojiIndex >= emojiSprites.Length)
            return;

        // Change le sprite de l'objet Image pour correspondre à l'émoji sélectionné
        EmojiDisplay.sprite = emojiSprites[emojiIndex];

        // Active l'objet Image pour afficher l'émoji et le désactive après 2 secondes
        EmojiDisplay.gameObject.SetActive(true);
        StartCoroutine(DisableAfterDelay(EmojiDisplay.gameObject, 2f));

        // Cache le panneau d'emojis après sélection
        EmojiPanel.SetActive(false);
    }

    IEnumerator DisableAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }

    // Fonction pour activer le Plateau_Panel et désactiver le Confirmation_Panel
    public void ShowPlateauPanel()
    {
        PlateauPanel.SetActive(true);
        ConfirmationPanel.SetActive(false);
        ParamPanel.SetActive(false);
    }

    public void ShowParamPanel()
    {
        ParamPanel.SetActive(true);
        ConfirmationPanel.SetActive(false);
        PlateauPanel.SetActive(false);
    }

    //Fonction Pour retourner a la page D'accuile
    public void ChangeScene()
    {
        SceneManager.LoadScene(0);
    }
}
