using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;

    public AudioSource button_audioSource;
    public AudioSource curagon_audioSource;


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
    }

    public void ButtonSound()
    {
        button_audioSource.Play();
    }

    public void PlayCuragonSound(AudioClip clip)
    {
        curagon_audioSource.clip = clip;
        curagon_audioSource.Play();
    }
}
