using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class Settings : MonoBehaviour
{
    [SerializeField] GameObject[] toggleIndicators;
    [SerializeField] Slider[] sliders;
    private bool firstLoad = true;

    void Start()
    {
        //toggleIndicators[0].SetActive(soundEnabled);
        toggleIndicators[1].SetActive(invertedShooting);
        sliders[0].value = musicVolume;
        sliders[1].value = sfxVolume;

        ChangeShadowQuality(shadowQuality);
        shadowDropdown.value = shadowQuality;
        SetSpeakerConfiguration(stereoSFX ? 1 : 0);
        speakerDropdown.value = stereoSFX ? 1 : 0;
        firstLoad = false;
    }


    ////// VOLUME //////
    
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Dropdown speakerDropdown;

    public static bool stereoSFX
    {
        get { return PlayerPrefs.GetInt("StereoSFX", 0) == 1; }
        set { PlayerPrefs.SetInt("StereoSFX", value ? 1 : 0); }
    }

    public static float musicVolume
    {
        get { return PlayerPrefs.GetFloat("MusicVolume", 1); }
        set { PlayerPrefs.SetFloat("MusicVolume", value); }
    }
    public static float sfxVolume
    {
        get { return PlayerPrefs.GetFloat("SfxVolume", 1); }
        set { PlayerPrefs.SetFloat("SfxVolume", value); }
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20);
        musicVolume = volume;
    }

    public void SetSfxVolume(float volume)
    {
        audioMixer.SetFloat("sfxVolume", Mathf.Log10(volume) * 20);
        sfxVolume = volume;
    }

    public void SetSpeakerConfiguration(int value)
    {
        stereoSFX = value == 1;
        if (!firstLoad) Audio.PlaySound("InterfaceInteraction3", null);
    }

    ////// INVERTED SHOOTING //////

    public static bool invertedShooting
    {
        get { return PlayerPrefs.GetInt("InvertedShooting", 0) == 1; }
        set { PlayerPrefs.SetInt("InvertedShooting", value ? 1 : 0); }
    }

    public void ToggleInvertedShooting()
    {
        invertedShooting = !invertedShooting;
        toggleIndicators[1].SetActive(invertedShooting);
        Audio.PlaySound("InterfaceInteraction2", null);
    }

    ////// SHADOW QUALITY //////

    [SerializeField] RenderPipelineAsset[] qualityLevels;
    [SerializeField] Dropdown shadowDropdown;

    public static int shadowQuality
    {
        get { return PlayerPrefs.GetInt("ShadowQuality", 2); }
        set { PlayerPrefs.SetInt("ShadowQuality", value); }
    }

    public void ChangeShadowQuality(int value)
    {
        QualitySettings.renderPipeline = qualityLevels[value];
        shadowQuality = value;
        if (!firstLoad) Audio.PlaySound("InterfaceInteraction3", null);
    }

}
