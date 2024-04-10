using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoIntroSequence : MonoBehaviour
{
    public GameObject blackBackground;
    public GameObject imageToShow;

    public float timeToShowBlackBackground = 2f; // Temps que le fond noir reste affiché.
    public float timeToShowImage = 2f; // Temps que l'image reste affichée avant d'activer le panel.

    // Utilisez Start pour lancer la coroutine dès que la scène démarre.
    void Start()
    {
        StartCoroutine(ShowIntroSequence());
    }

    IEnumerator ShowIntroSequence()
    {
        // Assurez-vous que les éléments sont dans l'état désiré avant de commencer.
        blackBackground.SetActive(true); // Affiche le fond noir dès le départ.
        imageToShow.SetActive(false); // S'assure que l'image est initialement désactivée.

        // Attend avant de cacher le fond noir et afficher l'image.
        yield return new WaitForSeconds(timeToShowBlackBackground);

        blackBackground.SetActive(false); // Cache le fond noir.
        imageToShow.SetActive(true); // Affiche l'image.
        yield return new WaitForSeconds(timeToShowImage); // Attend encore un peu.

        imageToShow.SetActive(false); // Optionnel : cache l'image si nécessaire.
    }
}
