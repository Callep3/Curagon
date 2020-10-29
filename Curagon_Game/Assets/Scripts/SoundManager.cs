using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;
    
    AudioClip[] audioClips;
    AudioSource soundEffects_audioSource;
    AudioSource curagon_audioSource;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        Init();
    }

    private void Init()
    {
        GetAllComponents();
    }

    private void GetAllComponents()
    {
        curagon_audioSource = transform.Find("AudioSources").Find("CuragonAudio").GetComponent<AudioSource>();
        soundEffects_audioSource = transform.Find("AudioSources").Find("ButtonAudio").GetComponent<AudioSource>();
        audioClips = new AudioClip[1];
        audioClips[0] = Resources.Load<AudioClip>("Audio/SFX/ButtonPressed_01");
    }

    public void ButtonSound()
    {
        soundEffects_audioSource.clip = audioClips[(int)SoundEffects_Sounds.Button];
        soundEffects_audioSource.Play();
    }

    public void PlayCuragonSound(AudioClip clip)
    {
        curagon_audioSource.clip = clip;
        curagon_audioSource.Play();
    }

    public void StopCuragonSound()
    {
        curagon_audioSource.Stop();
    }
}

public enum SoundEffects_Sounds : int
{
    Button
}
