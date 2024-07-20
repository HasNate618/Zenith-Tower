using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    [System.Serializable]
    public class Sound
    {
        public AudioClip clip;
        public float volumeScale = 1;
    }

    private static AudioSource soundEffectSource;

    [SerializeField] AudioClip[] musicClips;
    [SerializeField] AudioClip bossMusic;
    public static AudioSource musicSource;

    [SerializeField] Sound[] soundEffectsArray;
    private static Dictionary<string, Sound> soundEffects;

    public static Audio instance;

    [SerializeField] Transform positionalSoundObject;

    private void Start()
    {
        instance = this;
        soundEffectSource = GetComponent<AudioSource>();

        if (soundEffects == null)
        {
            soundEffects = new Dictionary<string, Sound>();
            foreach (Sound sound in soundEffectsArray)
            {
                soundEffects.Add(sound.clip.name, sound);
            }
        }

        musicSource = GetComponentsInChildren<AudioSource>()[1];
        musicSource.clip = musicClips[Random.Range(0, musicClips.Length)];
        musicSource.Play();
    }

    public static void PlayBossMusic()
    {
        musicSource.Stop();
        musicSource.clip = instance.bossMusic;
        musicSource.Play();
    }

    public static void PlaySound(string name, AudioSource source)
    {
        if (soundEffects.ContainsKey(name))
        {
            Sound sound = soundEffects[name];
            source ??= soundEffectSource;
            source.PlayOneShot(sound.clip, sound.volumeScale);
        }
        else
        {
            Debug.LogWarning("Sound Effect \"" + name + "\" not found");
        }
    }    

    public static void PlaySoundAtPosition(string name, Vector3 position)
    {
        if (soundEffects.ContainsKey(name))
        {
            if (Settings.stereoSFX)
            {
                PlaySound(name, null);
                print("using stereo");
                return;
            }
            Sound sound = soundEffects[name];
            Transform obj = Instantiate(instance.positionalSoundObject, position, Quaternion.identity);
            obj.GetComponent<AudioSource>().PlayOneShot(sound.clip, sound.volumeScale);
            Destroy(obj.gameObject, sound.clip.length);
        }
        else
        {
            Debug.LogWarning("Sound Effect \"" + name + "\" not found");
        }
    }
}
