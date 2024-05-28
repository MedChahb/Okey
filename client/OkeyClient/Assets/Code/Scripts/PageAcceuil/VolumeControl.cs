using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public Slider volumeSlider; // Référence au slider
    public AudioSource backgroundAudioSource; // Référence à l'AudioSource pour la musique de fond
    public AudioSource buttonAudioSource; // Référence à l'AudioSource pour les sons des boutons

    void Start()
    {
        // Assurez-vous que le volume initial du slider correspond au volume de l'AudioSource de la musique de fond
        volumeSlider.value = backgroundAudioSource.volume;

        // Ajoutez un listener pour détecter les changements de valeur du slider
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    // Méthode appelée quand la valeur du slider change
    void SetVolume(float volume)
    {
        // Définir le volume de la musique de fond
        backgroundAudioSource.volume = volume;

        // Définir le volume de l'AudioSource des boutons
        buttonAudioSource.volume = volume;
    }
}
