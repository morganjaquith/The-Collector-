using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {
	
    /// <summary>
    /// NOTE FOR SINGLE PLAYER/TWO PLAYER SCREEN:
    /// Hitting Two Player should bring up another screen where the user chooses an input device
    /// </summary>

    [Header("Enemy Spawn Count")]
    public int enemyCount = 1;

    [Header("Mode Time Limits")]
    public float singlePlayerGameTime=60f, twoPlayerGameTime = 120f;

    [Header("Main Game Prefabs")]
	public GameObject playerOnePrefab;
	public GameObject playerTwoPrefab;
    public GameObject enemyPrefab;
	
    [Header("Game Prefab Spawn Points")]
	public Transform playerOneSpawnPoint;
	public Transform playerTwoSpawnPoint;
    public Transform[] enemySpawnPoints;

    [Header("Level UI")]
    public GameObject endGameBackToMenuButton;
    public EventSystem inputHandler;

    [Header("Debug")]
    public float playerOnePoints, playerTwoPoints;
    public int playerOneLives = 3;
    public int playerTwoLives = 3;
    public float runtimeGameTime;
	public bool GameStart;
    public bool paused;
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

                if (isTwoPlayer)
                {
                    playerOneInstance.transform.GetChild(0).GetComponent<ThirdPersonCamera>().SetPlayerOneCameraForTwoPlayer();
                }
                playerOneLives--;
                playerOneLivesUI.text = "Lives : " + playerOneLives;
			}

            
			if(playerTwoInstance == null && playerTwoLives > 0 && isTwoPlayer)
			{
				//Spawn player two
				playerTwoInstance = Instantiate(playerTwoPrefab,playerTwoSpawnPoint.position, playerTwoPrefab.transform.rotation);
                playerTwoInstance.transform.GetChild(0).GetComponent<ThirdPersonCamera>().SetPlayerTwoCameraForTwoPlayer();
                playerTwoLives--;
                playerTwoLivesUI.text = "Lives : " + playerTwoLives;
			}

            if (!paused)
            {
                runtimeGameTime -= Time.deltaTime;
            }

			//Create float of the counter that's meant to be displayed to the screen
            float displayTime = (float) System.Math.Round(runtimeGameTime, 2);
            string timeDisplayCorrection = "";

            //The statments below allow us to display the timer in a good format
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

            //Check if someone's run out of lives
            if(playerOneLives <= 0 || playerTwoLives <= 0)
            {
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
            playerTwoInstance = Instantiate(playerTwoPrefab, playerTwoSpawnPoint.position, playerTwoPrefab.transform.rotation);
            playerTwoInstance.transform.GetChild(0).GetComponent<ThirdPersonCamera>().SetPlayerTwoCameraForTwoPlayer();
            playerTwoPoints = 0;


            playerOneLivesUI.enabled = true;
            playerOnePointsUI.enabled = true;

            //Spawn player one, setting the camera/points appropraitely
            playerOneInstance = Instantiate(playerOnePrefab, playerOneSpawnPoint.position, playerOnePrefab.transform.rotation);
            playerOneInstance.transform.GetChild(0).GetComponent<ThirdPersonCamera>().SetPlayerOneCameraForTwoPlayer();
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

        inputHandler.SetSelectedGameObject(endGameBackToMenuButton);

        //If time is up
        if (TimesUp)
        {
            //If we're in two player mode
            if (isTwoPlayer)
            {
                if(playerOnePoints > playerTwoPoints)
                {
                    TwoPlayerWinTitleUI.text = "Player One Wins!";
                    ShowScore(playerOnePoints);
                }
                else if(playerOnePoints < playerTwoPoints)
                {
                    TwoPlayerWinTitleUI.text = "Player Two Wins!";
                    ShowScore(playerTwoPoints);
                }
                else
                {
                    TwoPlayerWinTitleUI.text = "It's a Tie!";
                    ShowScore(playerOnePoints);
                }
            }
            else //If we're in single player
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

        if (isTwoPlayer)
        {
            playerTwoInstance.GetComponent<Player>().Pause(true);
        }

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

        if (isTwoPlayer)
        {
            playerTwoLives = maxLives;
            playerTwoPoints = 0;
        }

    }

    public void AddPoints(float pointsToAdd, bool playerOnesZone)
    {
        if(playerOnesZone)
        {
            playerOnePoints += pointsToAdd;
            playerOnePointsUI.text = "Points : " + playerOnePoints;
        }
        else
        {
            playerTwoPoints += pointsToAdd;
            playerTwoPointsUI.text = "Points : " + playerTwoPoints;
        }
    }

    private void ShowScore(float winningScore)
    {
        WinningScoreUI.enabled = true;
        WinningScoreUI.text = "Score : " + winningScore;
    }

    public void Pause()
    {
        paused = true;
    }

    public void Unpause()
    {
        paused = false;
    }

    //This assumes user has already chosen who gets what input device (keyboard+mouse or controller)
    //And this is called before GameStart and after every instantation of player 2
	public void IsTwoPlayer()
	{
		isTwoPlayer = true;
	}
}
