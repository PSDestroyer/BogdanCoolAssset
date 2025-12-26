using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        public bool loop;
    }

    public Sound[] sounds;
    public AudioSource musicSource;
    public AudioSource[] sfxSources;
    public const float NORMAL_PITCH = 1f;
    private int _currentSfxSourceIndex;

    public float SoundVolume { get; set; } = 1f;

    public Sound CurrentSoundPlaying => FindSoundByName(sfxSources[_currentSfxSourceIndex].clip.name);

    public Sound FindSoundByName(string name)
    {
        if (sounds.Length == 0) return null;
        return Array.Find(sounds, s => s.name == name);
    }

    private void Start()
    {
        musicSource.loop = false;
        musicSource.playOnAwake = false;
    }


    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
    public void SetSfxVolume(float volume)
    {
        foreach (var s in sfxSources)
            s.volume = volume;
    }

    public void PlayMusic(string name)
    {
        var s = FindSoundByName(name);
        if (s != null)
        {
            musicSource.clip = s.clip;
            musicSource.loop = s.loop;
            musicSource.Play();

        }
    }

    public void StopPlayingSound(string name)
    {
        Array.Find(sfxSources, s => s.clip == FindSoundByName(name).clip)?.Stop();
    }

    public Sound PlaySound(string name)
    {
        var s = FindSoundByName(name);
        var curSoruce = sfxSources[_currentSfxSourceIndex];
        curSoruce.pitch = NORMAL_PITCH;
        if (s != null)
        {
            curSoruce.loop = s.loop;  
            curSoruce.PlayOneShot(s.clip);
        }

        _currentSfxSourceIndex = (_currentSfxSourceIndex + 1) % sfxSources.Length;
        return s;
    }

    public void Play(AudioClip audio, float pitch = NORMAL_PITCH, float volume = 1f)
    {
        pitch = Mathf.Clamp01(pitch);
        volume = Mathf.Clamp01(volume);
        var curSoruce = sfxSources[_currentSfxSourceIndex];
        curSoruce.pitch = pitch;
        curSoruce.volume = volume;
        curSoruce.clip = audio;
        curSoruce.Play();
    }

    public void PlaySound(string name, float increaseVolume , float pitch = 1f)
    {
        var s = FindSoundByName(name);
        var curSoruce = sfxSources[_currentSfxSourceIndex];
        curSoruce.volume = System.Math.Clamp(SoundVolume + increaseVolume, 0f, 1f);
        PlaySound(name, pitch);
    }

    public void PlaySound(string name, float pitch)
    {
        var s = FindSoundByName(name);
        var curSoruce = sfxSources[_currentSfxSourceIndex];
        curSoruce.pitch = NORMAL_PITCH;

        if (s != null)
        {
            curSoruce.loop = s.loop;
            curSoruce.pitch = pitch;
            curSoruce.PlayOneShot(s.clip);
        }
        curSoruce.volume = SoundVolume;

        _currentSfxSourceIndex = (_currentSfxSourceIndex + 1) % sfxSources.Length;
    }

    public void Play3DSound(string name, Vector3 position)
    {
        var s = FindSoundByName(name);
        if (s == null) return;

        GameObject tempGO = new GameObject("3D Sound " + name);
        tempGO.transform.position = position;

        AudioSource tempSource = tempGO.AddComponent<AudioSource>();
        tempSource.clip = s.clip;
        tempSource.volume = SoundVolume;
        tempSource.spatialBlend = 1f;
        tempSource.loop = s.loop;
        tempSource.Play();

        if (!s.loop) Destroy(tempGO, s.clip.length);
    }

    public void Play3DSound(string name, float pitch, Vector3 position)
    {
        var s = FindSoundByName(name);
        if (s == null) return;

        GameObject tempGO = new GameObject("3D Sound " + name);
        tempGO.transform.position = position;

        AudioSource tempSource = tempGO.AddComponent<AudioSource>();
        tempSource.volume = SoundVolume;
        tempSource.pitch = pitch;
        tempSource.clip = s.clip;
        tempSource.spatialBlend = 1f;
        tempSource.loop = s.loop;
        tempSource.Play();

        if (!s.loop) Destroy(tempGO, s.clip.length);
    }

}
