using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ParametreLobby : MonoBehaviour
{
    public Slider volumeSliderAmbience;
    public Slider volumeSliderEffects;

    public AudioSource audioSourceAmbience;

    public void Start()
    {
        volumeSliderAmbience.value = UIManager.singleton.backgroundMusic;
        volumeSliderEffects.value = UIManager.singleton.soundEffects;

        volumeSliderAmbience.onValueChanged.AddListener(OnVolumeSliderValueChangedAmbience);
        volumeSliderEffects.onValueChanged.AddListener(OnVolumeSliderValueChangedEffects);
    }

    private void OnVolumeSliderValueChangedAmbience(float value)
    {
        audioSourceAmbience.volume = value;
        UIManager.singleton.backgroundMusic = value;
    }

    private void OnVolumeSliderValueChangedEffects(float value)
    {
        UIManager.singleton.soundEffects = value;
    }
}
