using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private AudioSource audioSource;
    
    public bool sound = true;
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }
    public void SoundOnOff()
    {
        sound = !sound;
    }
    public void PlaySoundFX(AudioClip clip, float voulme)
    {
        if (sound)
            audioSource.PlayOneShot(clip, voulme);
        
    }
}
