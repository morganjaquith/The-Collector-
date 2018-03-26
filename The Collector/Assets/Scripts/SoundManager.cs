using UnityEngine;

public class SoundManager : MonoBehaviour {
	
	public bool playsMusic; //If false, this'll be playing sound effects alone
	public float volume;
	AudioSource source;
	
	private void Start()
	{
		source = GetComponent<AudioSource>();
        CheckPlayerPrefs();
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
			volume = PlayerPrefs.GetInt("Music");
		}
		else
		{
			volume = PlayerPrefs.GetInt("Sound");
		}
		
		source.volume = volume;
	}
}
