using UnityEngine;
using UnityEngine.UI;

public class ButtonClickSound : MonoBehaviour
{
    public AudioClip clickSound; // Le fichier audio à jouer
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.volume = UIManager.singleton.soundEffects;
        }
        audioSource.playOnAwake = false;
    }

    public void PlayClickSound()
    {
        audioSource.volume = UIManager.singleton.soundEffects;
        audioSource.PlayOneShot(clickSound);
    }
}
