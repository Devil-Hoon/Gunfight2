using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGSoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static BGSoundManager instance;
    AudioSource myAudio;
    public bool isOn = false;

    [Header("BGM")]
    [Header("GunFight")]
    public AudioClip GunfightMainBGM;
    public AudioClip GunfightLobbyBGM;
    public AudioClip GunfightPlayBGM;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            isOn = true;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        myAudio = GetComponent<AudioSource>();
    }
    public void Play()
    {
        if (myAudio != null && isOn)
        {
            myAudio.Play();
        }

    }
    public void VolumeSet(float volume)
    {
        myAudio.Pause();
        myAudio.volume = volume;
        myAudio.Play();
    }
    public void StopBGM()
    {
        if (myAudio != null)
        {
            myAudio.Stop();
        }
    }
    /// <summary>
    /// GunFight BGM
    /// </summary>
    public void PlayGunfightMainBGM()
    {
        if (myAudio != null && isOn)
        {
            myAudio.clip = GunfightMainBGM;
            myAudio.loop = true;
            myAudio.volume = 0.1f;
            myAudio.Play();
        }
    }

    public void PlayGunfightLobbyBGM()
    {
        if (myAudio != null && isOn)
        {
            myAudio.clip = GunfightLobbyBGM;
            myAudio.loop = true;
            myAudio.volume = 0.1f;
            myAudio.Play();
        }
    }

    public void PlayGunfightGameBGM()
    {
        if (myAudio != null && isOn)
        {
            myAudio.clip = GunfightPlayBGM;
            myAudio.loop = true;
            myAudio.volume = 0.1f;
            myAudio.Play();
        }
    }
}
