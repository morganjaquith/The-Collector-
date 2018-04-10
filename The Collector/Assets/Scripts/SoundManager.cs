﻿using UnityEngine;

public class SoundManager : MonoBehaviour {

    public bool dontDestroyOnLevelLoad;
	public bool playsMusic; //If false, this'll be playing sound effects alone
	public float volume;
	AudioSource source;
	
	private void Start()
	{
		source = GetComponent<AudioSource>();
        CheckPlayerPrefs();

        if(dontDestroyOnLevelLoad)
        {
            DontDestroyOnLoad(gameObject);
        }

	}

    private void FixedUpdate()
    {
        if (playsMusic)
        {
            if(volume != PlayerPrefs.GetFloat("Music"))
            {
                volume = PlayerPrefs.GetFloat("Music");
            }
        }
        else
        {
            if (volume != PlayerPrefs.GetFloat("Sound"))
            {
                volume = PlayerPrefs.GetFloat("Sound");
            }
        }
    }

    public void PlaySound(AudioClip sound)
	{
		source.clip = sound;
		source.Play();
	}
	
	public void CheckPlayerPrefs()
	{
		if(playsMusic)
		{
			volume = PlayerPrefs.GetFloat("Music");
		}
		else
		{
			volume = PlayerPrefs.GetFloat("Sound");
		}
		
		source.volume = volume;
	}
}