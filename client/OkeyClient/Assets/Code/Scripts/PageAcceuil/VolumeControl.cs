using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public Slider volumeSlider; // Référence au slider
    public AudioSource audioSource; // Référence à l'AudioSource

    void Start()
    {
        // Assurez-vous que le volume initial du slider correspond au volume de l'AudioSource
        volumeSlider.value = audioSource.volume;

        // Ajoutez un listener pour détecter les changements de valeur du slider
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    // Méthode appelée quand la valeur du slider change
    void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
