using UnityEngine.UI;
using UnityEngine;

public class ScoreGetter : MonoBehaviour {

    public bool getTwoPlayerScore;
    public float scoreFound;

	void Awake ()
    {
        if (getTwoPlayerScore)
        {
            scoreFound = PlayerPrefs.GetFloat("TwoPlayerHighScore");
        }
        else
        {
            scoreFound = PlayerPrefs.GetFloat("SinglePlayerHighScore");
        }

        GetComponent<Text>().text = scoreFound.ToString();

    }
}