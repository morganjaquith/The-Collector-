using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour {
	
    /// <summary>
    /// NOTE FOR SINGLE PLAYER/TWO PLAYER SCREEN:
    /// Hitting Single Player should start the game in single player mode (Input device shall be decided on MainMenu Start)
    /// Hitting Two Player should bring up another screen where the user chooses an input device
    /// </summary>

	public int playerOneLives = 3;
	public int playerTwoLives = 3;

    public int enemyCount = 1;

    public float playerOnePoints, playerTwoPoints;

    public float runtimeGameTime;

    public float singlePlayerGameTime=60f, twoPlayerGameTime = 120f;

	public GameObject playerOnePrefab;
	public GameObject playerTwoPrefab;
    public GameObject enemyPrefab;
	
	public Transform playerOneSpawnPoint;
	public Transform playerTwoSpawnPoint;
    public Transform[] enemySpawnPoints;
	
	public bool GameStart;
    public bool TimesUp;
	public bool isTwoPlayer;

    private int maxLives = 3;
	private GameObject playerOneInstance;
	private GameObject playerTwoInstance;
    private GameObject[] enemyInstances;

    private GameObject gameUI;

    private Text playerOnePointsUI;
    private Text playerOneLivesUI;
    private Text playerTwoPointsUI;
    private Text playerTwoLivesUI;
    private Text singlePlayerWinTitleUI;
    private Text singlePlayerLoseTitleUI;
    private Text TwoPlayerWinTitleUI;
    private Text WinningScoreUI;
    private Text TimeCounterUI;
    private GameObject backButton;

    private void Start()
    {
        enemyInstances = new GameObject[enemyCount];

        runtimeGameTime = playerOnePoints;

        if (gameUI == null)
        {
            gameUI = GameObject.Find("GameUI");
        }

		//Get all gameUI elements 
            playerOnePointsUI = gameUI.transform.GetChild(0).GetChild(0).GetComponent<Text>();
            playerOneLivesUI = gameUI.transform.GetChild(0).GetChild(1).GetComponent<Text>();

            playerOneLivesUI.text = "Lives : " + playerOneLives;
            playerOnePointsUI.text = "Points : " + playerOnePoints;

            singlePlayerWinTitleUI = gameUI.transform.GetChild(2).GetChild(0).GetComponent<Text>();
            singlePlayerLoseTitleUI = gameUI.transform.GetChild(2).GetChild(1).GetComponent<Text>();

            playerTwoPointsUI = gameUI.transform.GetChild(1).GetChild(0).GetComponent<Text>();
            playerTwoLivesUI = gameUI.transform.GetChild(1).GetChild(1).GetComponent<Text>();
            TwoPlayerWinTitleUI = gameUI.transform.GetChild(2).GetChild(2).GetComponent<Text>();

            playerTwoLivesUI.text = "Lives : " + playerTwoLives;
            playerTwoPointsUI.text = "Points : " + playerTwoPoints;

            WinningScoreUI = gameUI.transform.GetChild(2).GetChild(3).GetComponent<Text>();

            TimeCounterUI = gameUI.transform.GetChild(3).GetComponent<Text>();

            backButton = gameUI.transform.GetChild(4).gameObject;
    }

    private void FixedUpdate ()
	{
		if(GameStart)
		{
            
			if(playerOneInstance == null && playerOneLives > 0)
			{
				//Spawn player one
				playerOneInstance = Instantiate(playerOnePrefab,playerOneSpawnPoint.position, playerOnePrefab.transform.rotation);
				playerOneLives--;
                playerOneLivesUI.text = "Lives : " + playerOneLives;
			}

            /*
			if(playerTwoInstance == null && playerTwoLives > 0)
			{
				//Spawn player two
				playerTwoInstance = Instantiate(playerTwoPrefab,playerTwoSpawnPoint.position, playerTwoPrefab.transform.rotation);
				playerTwoLives--;
                playerTwoLivesUI.text = "Lives : " + playerTwoLives;
			}*/
            
            runtimeGameTime -= Time.deltaTime;

			//Create float of the counter that's meant to be displayed to the screen
            float displayTime = (float) System.Math.Round(runtimeGameTime, 2);
            string timeDisplayCorrection = "";

            if (displayTime >= 100f)
            {
                if (displayTime.ToString().Length == 5)
                {
                    timeDisplayCorrection = "0";
                }
                else if (displayTime.ToString().Length == 4)
                {
                    timeDisplayCorrection = "00";
                }
                else if (displayTime.ToString().Length == 3)
                {
                    timeDisplayCorrection = ".00";
                }
                else
                {
                    timeDisplayCorrection = "";
                }
            }
            else if (displayTime >= 10f)
            {
                if (displayTime.ToString().Length == 4)
                {
                    timeDisplayCorrection = "0";
                }
                else if (displayTime.ToString().Length == 3)
                {
                    timeDisplayCorrection = "00";
                }
                else if (displayTime.ToString().Length == 2)
                {
                    timeDisplayCorrection = ".00";
                }
                else
                {
                    timeDisplayCorrection = "";
                }
            }
            else
            {
                if (displayTime.ToString().Length == 3)
                {
                    timeDisplayCorrection = "0";
                }
                else if (displayTime.ToString().Length == 2)
                {
                    timeDisplayCorrection = "00";
                }
                else if (displayTime.ToString().Length == 1)
                {
                    timeDisplayCorrection = ".00";
                }
                else
                {
                    timeDisplayCorrection = "";
                }
            }

            TimeCounterUI.text = "Time :\n" + displayTime+ timeDisplayCorrection;

			//Check if we've run out of time
            if (runtimeGameTime <= 0)
			{
                TimesUp = true;
                runtimeGameTime = 0;
                EndGame();
			}
			
		}
	}

    public void StartGame()
    {

        GameStart = true;

        //Spawn enemy in four quads
        //Tell each enemy what waypoint tags to look for
        for (int i = 0; i < enemyCount; i++)
        {
            enemyInstances[i] = GameObject.Instantiate(enemyPrefab, enemySpawnPoints[i].transform.position, enemySpawnPoints[i].transform.rotation);

            //Integrate later when we have definite quad zones with their own specific waypoints;
            //enemyInstances[i].GetComponent<Enemy>().GetWaypointsWithNewTag(waypointTags[i]); 
        }

        if (isTwoPlayer)
        {
            runtimeGameTime = twoPlayerGameTime;

            playerTwoLivesUI.enabled = true;
            playerTwoPointsUI.enabled = true;

            //Spawn player two, setting the camera/points appropraitely
            playerTwoInstance = Instantiate(playerOnePrefab, playerOneSpawnPoint.position, playerOnePrefab.transform.rotation);
            playerTwoPrefab.transform.GetChild(0).GetComponent<ThirdPersonCamera>().SetPlayerTwoCameraForTwoPlayer();
            playerTwoPoints = 0;


            playerOneLivesUI.enabled = true;
            playerOnePointsUI.enabled = true;

            //Spawn player one, setting the camera/points appropraitely
            playerOneInstance = Instantiate(playerOnePrefab, playerOneSpawnPoint.position, playerOnePrefab.transform.rotation);
            playerOnePrefab.transform.GetChild(0).GetComponent<ThirdPersonCamera>().SetPlayerOneCameraForTwoPlayer();
            playerOnePoints = 0;

        }
        else
        {
            runtimeGameTime = singlePlayerGameTime;

            playerOneLivesUI.enabled = true;
            playerOnePointsUI.enabled = true;

            //Spawn player one
            playerOneInstance = Instantiate(playerOnePrefab, playerOneSpawnPoint.position, playerOnePrefab.transform.rotation);
            playerOnePoints = 0;

        }

        TimeCounterUI.enabled = true;

    }

    private void EndGame()
    {

        //If time is up
        if (TimesUp)
        {
            //If we're in two player mode
          /*  if (isTwoPlayer)
            {
                //compare scores
                //Show who wins
            }
            else */ //If we're in single player
            {
                //Show score
                //If the player just sat there and didn't earn points
                if(playerOnePoints == 0)
                {
                    singlePlayerLoseTitleUI.enabled = true;
                    ShowScore(playerOnePoints);
                }
                else
                {
                    singlePlayerWinTitleUI.enabled = true;
                    ShowScore(playerOnePoints);
                }
            }
        }
        else
        {
            //If player runes out of lives
            if(playerOneLives <=0 && !isTwoPlayer)
            {
                //Bring up lose screen and show score
                singlePlayerLoseTitleUI.enabled = true;
                ShowScore(playerOnePoints);
            }
        }

        playerOneInstance.GetComponent<Player>().Pause(true);

        for (int i = 0; i < enemyInstances.Length; i++)
        {
            enemyInstances[i].GetComponent<Enemy>().Pause();
        }

        backButton.SetActive(true);

        //Reset variables
        GameStart = false;
        TimesUp = false;
        playerOneLives = maxLives;
        playerOnePoints = 0;
        //playerTwoLives = maxLives;
        //PlayerTwoPoints = 0;

    }

    public void AddPoints(float pointsToAdd, bool playerOnesZone)
    {
        if(playerOnesZone)
        {
            playerOnePoints += pointsToAdd;
            playerOnePointsUI.text = "Points : " + playerOnePoints;
        }
     /*   else
        {
            playerTwoPoints += pointsToAdd;
            playerTwoPointsUI.text = "Points : " + playerTwoPoints;
        } */
    }

    private void ShowScore(float winningScore)
    {
        WinningScoreUI.enabled = true;
        WinningScoreUI.text = "Score : " + winningScore;
    }

    /*
    //This assumes user has already chosen who gets what input device (keyboard+mouse or controller)
    //And this is before GameStart
	public void IsTwoPlayer()
	{
		isTwoPlayer = true;
	}*/
}
