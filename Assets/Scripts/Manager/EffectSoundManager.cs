using System.Collections;

using System.Collections.Generic;
using UnityEngine;

public class EffectSoundManager : MonoBehaviour
{
    public static EffectSoundManager instance;
    AudioSource myAudio;
    public bool isOn = false;

    [Header("Effect")]
    [Header("GunFight")]
    public AudioClip gunShot;
    public AudioClip explosion;
    public AudioClip laser;
    
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
    public void Stop()
    {
        if (myAudio != null && isOn)
        {
            myAudio.Stop();
        }
    }
    /// <summary>
    /// Gunfight
    /// </summary>
    public void GunShot()
    {
        if (myAudio != null && isOn)
        {
            myAudio.volume = 0.3f;
            myAudio.PlayOneShot(gunShot);
        }
    }
    public void Explosion()
    {
        if (myAudio != null && isOn)
        {
            myAudio.volume = 1.0f;
            myAudio.PlayOneShot(explosion);
        }
    }
	public void Laser()
	{
		if (myAudio != null && isOn)
		{
			myAudio.volume = 1.0f;
			myAudio.PlayOneShot(laser);
		}
	}
}
