using UnityEngine;

public class AudioClipPlayer : MonoBehaviour {

    public AudioClip musicToPlayOnStart;
    private SoundManager soundManager;

	// Use this for initialization
	void Start () {

        if (musicToPlayOnStart != null)
        {
            try
            {
                soundManager = GameObject.Find("MusicPlayer").GetComponent<SoundManager>();

            }
            catch (System.Exception ex)
            {
                Debug.Log("MusicPlayer not found! Did you not start your game in the MainMenu level? That's where MusicPlayer gets created.   " + ex.Message);
            }

            if (soundManager != null)
            {
                soundManager.PlaySound(musicToPlayOnStart);
            }
        }
    }
}
