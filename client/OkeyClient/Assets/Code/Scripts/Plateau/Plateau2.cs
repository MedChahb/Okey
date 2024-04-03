using System.Collections;
using System.Collections.Generic;
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

    // Fonction pour activer le Confirmation_Panel et désactiver le Plateau_Panel
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
