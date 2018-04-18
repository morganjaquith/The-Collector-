using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LeaderboardManager : MonoBehaviour {

    [Header("Required Variables")]
    public GameObject firstPlaceObject;
    public GameObject secondPlaceObject;

    public GameObject secondPlaceStand;

    public Material playerOneMat;
    public Material playerTwoMat;

    public GameObject enterNameMenuElements;

    public GameObject backToMenuButton;
    public GameObject nameTextField;
    public GameObject newScoreText;

    public EventSystem inputSystem;

    [Header("Leaderboard Objects")]
    public GameObject leaderBoardMenuElements;

    public GameObject p1scoreAndName1;

    public GameObject p1scoreAndName2;

    public GameObject p1scoreAndName3;

    public GameObject p2scoreAndName1;

    public GameObject p2scoreAndName2;

    public GameObject p2scoreAndName3;

    [Header("Debug")]
    public bool newSinglePlayerScore;
    public bool newTwoPlayerScore;
    public float newPlayerOneHighscore;
    public float newPlayerTwoHighscore;

    public string playerOneHighscoreNameOne;
    public float playerOneHighscoreOne;

    public string playerOneHighscoreNameTwo;
    public float playerOneHighscoreTwo;

    public string playerOneHighscoreNameThree;
    public float playerOneHighscoreThree;

    public string playerTwoHighscoreNameOne;
    public float playerTwoHighscoreOne;

    public string playerTwoHighscoreNameTwo;
    public float playerTwoHighscoreTwo;

    public string playerTwoHighscoreNameThree;
    public float playerTwoHighscoreThree;
    
    public bool playerOneGotTwoPlayerHighscore;

    public string nameEntered;
    
    //On default:
    //Both stands are enabled
    //Player one is first place

    //If player just enters the scene and theres no new highscore
    //Both place stands would be on by default
    //Show leaderboard and back button

    //Check if we have a new highscore
    //If singleplayer, show one mesh. If two player show both meshes.
    //If so change mesh materials to represent who got the highscore
    //Show score and prompt for name of certain length
    //Get name and place it on the leaderboard
    //Apply data to player prefs

    // Use this for initialization
    void Start()
    {


        playerOneHighscoreNameOne = PlayerPrefs.GetString("p1HighscoreOneName", "p1name1");
        playerOneHighscoreOne = PlayerPrefs.GetFloat("p1HighscoreOne", 0);

        playerOneHighscoreNameTwo = PlayerPrefs.GetString("p1HighscoreTwoName", "p1name2");
        playerOneHighscoreTwo = PlayerPrefs.GetFloat("p1HighscoreTwo", 0);

        playerOneHighscoreNameThree = PlayerPrefs.GetString("p1HighscoreThreeName", "p1name3");
        playerOneHighscoreThree = PlayerPrefs.GetFloat("p1HighscoreThree", 0);

        playerTwoHighscoreNameOne = PlayerPrefs.GetString("p2HighscoreOneName", "p2name1");
        playerTwoHighscoreOne = PlayerPrefs.GetFloat("p2HighscoreOne", 0);

        playerTwoHighscoreNameTwo = PlayerPrefs.GetString("p2HighscoreTwoName", "p2name2");
        playerTwoHighscoreTwo = PlayerPrefs.GetFloat("p2HighscoreTwo", 0);

        playerTwoHighscoreNameThree = PlayerPrefs.GetString("p2HighscoreThreeName", "p2name3");
        playerTwoHighscoreThree = PlayerPrefs.GetFloat("p2HighscoreThree", 0);

        //Check if there are any new high scores
        newSinglePlayerScore = (PlayerPrefs.GetString("NewSinglePlayerHighScore") == "true") ? true : false;
        newTwoPlayerScore = (PlayerPrefs.GetString("NewTwoPlayerHighScore") == "true") ? true : false;

        if (newTwoPlayerScore)
        {
            playerOneGotTwoPlayerHighscore = (PlayerPrefs.GetString("PlayerOneGotTwoPlayerScore") == "true") ? true : false;

            newPlayerTwoHighscore = PlayerPrefs.GetFloat("TwoPlayerHighScore");

            if(!playerOneGotTwoPlayerHighscore)
            {
                firstPlaceObject.GetComponent<Renderer>().material = playerTwoMat;
                secondPlaceObject.GetComponent<Renderer>().material = playerOneMat;
            }

            PromptForName();
        }
        else if (newSinglePlayerScore)
        {
            secondPlaceObject.SetActive(false);
            secondPlaceStand.SetActive(false);

            newPlayerOneHighscore = PlayerPrefs.GetFloat("SinglePlayerHighScore");

            PromptForName();
        }
        else
        {
            secondPlaceObject.SetActive(false);
            secondPlaceStand.SetActive(false);
            enterNameMenuElements.SetActive(false);
            leaderBoardMenuElements.SetActive(true);
            inputSystem.SetSelectedGameObject(backToMenuButton);
            ShowScores();
        }

    }

    void PromptForName()
    {
        firstPlaceObject.GetComponent<Animation>().enabled = true;

        if (newSinglePlayerScore)
        {
            newScoreText.GetComponent<Text>().text = newPlayerOneHighscore.ToString();
        }
        else
        {
            newScoreText.GetComponent<Text>().text = newPlayerTwoHighscore.ToString();
        }

        enterNameMenuElements.SetActive(true);
        leaderBoardMenuElements.SetActive(false);
        inputSystem.SetSelectedGameObject(nameTextField);
    }

    void ShowScores()
    {
        enterNameMenuElements.SetActive(false);
        leaderBoardMenuElements.SetActive(true);

        p1scoreAndName1.GetComponent<Text>().text = playerOneHighscoreNameOne+" . . . . . "+playerOneHighscoreOne;
        p1scoreAndName2.GetComponent<Text>().text = playerOneHighscoreNameTwo + " . . . . . " + playerOneHighscoreTwo;
        p1scoreAndName3.GetComponent<Text>().text = playerOneHighscoreNameThree + " . . . . . " + playerOneHighscoreThree;

        p2scoreAndName1.GetComponent<Text>().text = playerTwoHighscoreNameOne + " . . . . . " + playerTwoHighscoreOne;
        p2scoreAndName2.GetComponent<Text>().text = playerTwoHighscoreNameTwo + " . . . . . " + playerTwoHighscoreTwo;
        p2scoreAndName3.GetComponent<Text>().text = playerTwoHighscoreNameThree + " . . . . . " + playerTwoHighscoreThree;

        inputSystem.SetSelectedGameObject(backToMenuButton);
    }

    void SaveScores(float p1S1,string p1N1, float p1S2, string p1N2, float p1S3, string p1N3, float p2S1, string p2N1, float p2S2, string p2N2, float p2S3, string p2N3)
    {
        PlayerPrefs.SetFloat("p1HighscoreOne",p1S1);
        PlayerPrefs.SetString("p1HighscoreOneName",p1N1);
        PlayerPrefs.SetFloat("p1HighscoreTwo", p1S2);
        PlayerPrefs.SetString("p1HighscoreTwoName", p1N2);
        PlayerPrefs.SetFloat("p1HighscoreThree", p1S3);
        PlayerPrefs.SetString("p1HighscoreThreeName", p1N3);

        PlayerPrefs.SetFloat("p2HighscoreOne", p2S1);
        PlayerPrefs.SetString("p2HighscoreOneName", p2N1);
        PlayerPrefs.SetFloat("p2HighscoreTwo", p2S2);
        PlayerPrefs.SetString("p2HighscoreTwoName", p2N2);
        PlayerPrefs.SetFloat("p2HighscoreThree", p2S3);
        PlayerPrefs.SetString("p2HighscoreThreeName", p2N3);

        PlayerPrefs.Save();

    }

    public void AcceptName()
    {
        nameEntered = nameTextField.GetComponent<InputField>().text;

        if (nameEntered != "")
        {

            if (newSinglePlayerScore)
            {
                //Apply new changes to first few objects then move all information down
                playerOneHighscoreNameThree = playerOneHighscoreNameTwo;
                playerOneHighscoreThree = playerOneHighscoreTwo;

                playerOneHighscoreNameTwo = playerOneHighscoreNameOne;
                playerOneHighscoreTwo = playerOneHighscoreOne;

                playerOneHighscoreNameOne = nameEntered;
                playerOneHighscoreOne = newPlayerOneHighscore;

                PlayerPrefs.SetString("NewSinglePlayerHighScore", "false");
            }
            else if (newTwoPlayerScore)
            {
                playerTwoHighscoreNameThree = playerTwoHighscoreNameTwo;
                playerTwoHighscoreThree = playerTwoHighscoreTwo;

                playerTwoHighscoreNameTwo = playerTwoHighscoreNameOne;
                playerTwoHighscoreTwo = playerTwoHighscoreOne;

                playerTwoHighscoreNameOne = nameEntered;
                playerTwoHighscoreOne = newPlayerTwoHighscore;

                PlayerPrefs.SetString("NewTwoPlayerHighScore", "false");
            }

            //Check if for single or multiplayer then assign items to their respective sections and save
            SaveScores(playerOneHighscoreOne, playerOneHighscoreNameOne, playerOneHighscoreTwo, playerOneHighscoreNameTwo, playerOneHighscoreThree, playerOneHighscoreNameThree, playerTwoHighscoreOne, playerTwoHighscoreNameOne, playerTwoHighscoreTwo, playerTwoHighscoreNameTwo, playerTwoHighscoreThree, playerTwoHighscoreNameThree);

            enterNameMenuElements.SetActive(false);
            leaderBoardMenuElements.SetActive(true);

            PlayerPrefs.SetFloat("SinglePlayerHighScore", 0f);
            PlayerPrefs.SetFloat("TwoPlayerHighScore", 0f);

            PlayerPrefs.Save();

            ShowScores();
        }
    }
}
