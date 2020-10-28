using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;

    public AudioSource audioSource_ButtonSound;
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
        audioSource_ButtonSound.Play();
    }

    public void PlayCuragonSound(AudioClip clip)
    {
        curagon_audioSource.clip = clip;
        curagon_audioSource.Play();
    }
}
