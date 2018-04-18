using UnityEngine;

public class PlayerPrefDefaultSetter : MonoBehaviour {

	// Use this for initialization
	void Start () {

        PlayerPrefs.SetString("NewSinglePlayerHighScore", "false");
        PlayerPrefs.SetString("NewTwoPlayerHighScore","false");
        PlayerPrefs.SetFloat("SinglePlayerHighScore",0f);
        PlayerPrefs.SetFloat("TwoPlayerHighScore",0f);
        PlayerPrefs.SetFloat("PlayerOneSessionPoints", 0f);
        PlayerPrefs.SetFloat("PlayerTwoSessionPoints", 0f);
        PlayerPrefs.SetInt("PromptedPlayMode", 0);
        PlayerPrefs.Save();
	}
}
