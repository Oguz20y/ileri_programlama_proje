using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;

    [SerializeField] private Sound[] sounds;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.audioClip;
                s.source.loop = s.isLoop;
                s.source.volume = s.volume;
                // clipNames.Add(s.clipName);

                switch (s.audioType)
                {
                    case Sound.AudioTypes.soundEffect:
                        s.source.outputAudioMixerGroup = sfxMixerGroup;
                        break;
                    case Sound.AudioTypes.music:
                        s.source.outputAudioMixerGroup = musicMixerGroup;
                        break;
                    default:
                        break;
                }

                if (s.playOnAwake)
                {
                    s.source.Play();
                }

            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Play_Pause(string clipName)
    {
        Sound s = Array.Find(sounds, sound => sound.clipName == clipName);
        if (s == null)
        {
            Debug.LogError("Sound: " + clipName + " does NOT exist!");
            return;
        }

        if (s.source.isPlaying)
        {
            s.source.Stop();
        }
        else
        {
            s.source.Play();
        }
    }
    public void UpdateMixerVolume()
    {
        musicMixerGroup.audioMixer.SetFloat("Music Volume", Mathf.Log10(AudioOptionsManager.musicVolume) * 20);
        sfxMixerGroup.audioMixer.SetFloat("SFX Volume", Mathf.Log10(AudioOptionsManager.sfxVolume) * 20);
    } 
}
