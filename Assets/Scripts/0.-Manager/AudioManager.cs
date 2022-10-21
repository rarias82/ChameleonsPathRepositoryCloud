using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource sfxAudioSource, musicAudioSource;

    public static AudioManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

    }

    public void PlaySound(AudioClip clip)
    {
        sfxAudioSource.PlayOneShot(clip);
    }

    private void MuteOnOff()
    {

        musicAudioSource.mute = !musicAudioSource.mute;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            MuteOnOff();
        }

    }
}
