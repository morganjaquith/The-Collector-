using UnityEngine.UI;
using UnityEngine;

public class ScoreGetter : MonoBehaviour {

    public bool keepText;
    public bool getTwoPlayerScore;
    public float scoreFound;
    public GameManager gameManager;

	void Awake ()
    {
        if(gameManager != null)
        {
            getTwoPlayerScore = gameManager.isTwoPlayer;
        }

        if (getTwoPlayerScore)
        {
            scoreFound = PlayerPrefs.GetFloat("TwoPlayerHighScore");
        }
        else
        {
            scoreFound = PlayerPrefs.GetFloat("SinglePlayerHighScore");
        }

        if (!keepText)
        {
            GetComponent<Text>().text = scoreFound.ToString();
        }
        else
        {
            GetComponent<Text>().text = "High Score : "+scoreFound.ToString();
        }
    }
}