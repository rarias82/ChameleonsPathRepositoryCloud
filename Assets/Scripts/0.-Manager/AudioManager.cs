using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource sfxAudioSource, musicAudioSource;
    [SerializeField] float musicTiempo;
    public AudioClip cancionNivel1;
    public AudioClip cancionMenu;
    public AudioClip clickButton;
    public AudioClip selectButton;
    public static AudioManager Instance;
    //{
    //    get;
    //    private set;
    //}

    private void Awake()
    {
        if (Instance == null /*&& Instance != this*/)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
        }
        else
        {

            Destroy(gameObject);
        }

    }

    private void Start()
    {
        StartCoroutine(ChangeMusic(cancionMenu));
            
    }

    public void PlaySound(AudioClip clip)
    {
        sfxAudioSource.PlayOneShot(clip);
    }


    public IEnumerator ChangeMusic(AudioClip clip)
    {
        while (musicAudioSource.volume != 0)
        {
            musicAudioSource.volume -= musicTiempo * Time.deltaTime;

            yield return null;
        }

        musicAudioSource.clip = clip;
        musicAudioSource.Play();

        while (musicAudioSource.volume != 1)
        {
            musicAudioSource.volume += musicTiempo * Time.deltaTime;

            yield return null;
        }
    }
   
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    MuteOnOff();
        //}

    }
}
