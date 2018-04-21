using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {

    [Header("Music To Play")]
    public AudioClip music;

    [Header("Enemy Spawn Count")]
    public int enemyCount = 5;

    [Header("Individual Enemy Spawn Points")]
    public Transform[] enemySpawnPoints;

    [Header("Enemy Waypoint Preferences")]
    public bool modifyWaypointPrefs;
    public float waypointWaitTime = 2f;
    public float waypointFollowDistanceLimit = 2f;
    public bool goToRandomWaypoint = false;

    [Header("Enemy Quads")]
    public bool useQuadrants;
    public string[] waypointQuadrantTags;

    [Header("Item Limit")]
    public int numberOfItemsToCollect;

    [Header("Mode Time Limits")]
    public float singlePlayerGameTime = 60f;
    public float twoPlayerGameTime = 120f;

    [Header("Main Game Prefabs")]
    public GameObject playerOnePrefab;
    public GameObject playerTwoPrefab;
    public GameObject enemyPrefab;
    public GameObject WinCamera;

    [Header("Player One / Player Two Points")]
    public Transform playerOneSpawnPoint;
    public Transform playerTwoSpawnPoint;

    [Header("Level UI")]
    public GameObject singlePlayerOrMultiplayerOptions;
    public GameObject nextLevelMenuButton;
    public GameObject endGameBackToMenuButton;
    public EventSystem inputHandler;

    [Header("Level Zones")]
    public GameObject playerOneZone;
    public GameObject playerTwoZone;

    [Header("Item Bases")]
    public GameObject[] itemBases;

    [Header("Debug")]
    public float playerOnePoints = 0;
    public float playerTwoPoints = 0;
    public int playerOneLives = 3;
    public int playerTwoLives = 3;
    public float runtimeGameTime;
    public bool GameStart;
    public bool paused;
    public bool TimesUp;
    public bool isTwoPlayer;

    public GameObject playerOneInstance;
    public GameObject playerTwoInstance;
    public GameObject[] enemyInstances;

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

    private SoundManager soundManager;
    private int totalItemsCollected;

    private void Start()
    {
        if (music != null)
        {
            try
            {
                soundManager = GameObject.Find("MusicPlayer").GetComponent<SoundManager>();

            }
            catch (System.Exception ex)
            {
                Debug.Log("MusicPlayer not found! Did you not start your game in the MainMenu level? That's where MusicPlayer gets created.   "+ex.Message);
            }

            if (soundManager != null)
            {
                soundManager.PlaySound(music);
            }
        }

        enemyInstances = new GameObject[enemyCount];

        if (gameUI == null)
        {
            gameUI = GameObject.Find("GameUI");
        }

        playerOnePoints = 0;
        playerTwoPoints = 0;

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


        int modeOption = PlayerPrefs.GetInt("PromptedPlayMode");

        //We're immedietly starting in singleplayer mode
        if (modeOption == 1)
        {
            singlePlayerOrMultiplayerOptions.SetActive(false);
            endGameBackToMenuButton.SetActive(false);
            IsSinglePlayer();
            if (PlayerPrefs.GetFloat("PlayerOneSessionPoints") != 0)
            {
                AddPoints(PlayerPrefs.GetFloat("PlayerOneSessionPoints"), true);
            }
            StartGame();
        }
        //We're immedietly starting in multiplayer mode
        else if (modeOption == 2)
        {
            singlePlayerOrMultiplayerOptions.SetActive(false);
            endGameBackToMenuButton.SetActive(false);
            IsTwoPlayer();

            if (PlayerPrefs.GetFloat("PlayerTwoSessionPoints") != 0)
            {
                AddPoints(PlayerPrefs.GetFloat("PlayerTwoSessionPoints"), false);
            }

            if (PlayerPrefs.GetFloat("PlayerOneSessionPoints") != 0)
            {
                AddPoints(PlayerPrefs.GetFloat("PlayerOneSessionPoints"), true);
            }
            StartGame();
        }

    }

    private void FixedUpdate()
    {
        if (GameStart)
        {
            
            if (totalItemsCollected == numberOfItemsToCollect && numberOfItemsToCollect != 0)
            {
                ZeroOutTime();  
            }

            if (playerOneInstance == null && playerOneLives > 0)
            {
                if (playerOneLives > 1)
                {
                    //Spawn player one
                    playerOneInstance = Instantiate(playerOnePrefab, playerOneSpawnPoint.position, playerOnePrefab.transform.rotation);

                    if (isTwoPlayer)
                    {
                        playerOneInstance.transform.GetChild(0).GetComponent<ThirdPersonCamera>().SetPlayerOneCameraForTwoPlayer();
                    }

                }

                playerOneLives--;
                playerOneLivesUI.text = "Lives : " + playerOneLives;
            }


            if (playerTwoInstance == null && playerTwoLives > 0 && isTwoPlayer)
            {
                if (playerTwoLives > 1)
                {
                    //Spawn player two
                    playerTwoInstance = Instantiate(playerTwoPrefab, playerTwoSpawnPoint.position, playerTwoPrefab.transform.rotation);
                    playerTwoInstance.transform.GetChild(0).GetComponent<ThirdPersonCamera>().SetPlayerTwoCameraForTwoPlayer();
                }

                playerTwoLives--;
                playerTwoLivesUI.text = "Lives : " + playerTwoLives;

            }

            if (!paused)
            {
                runtimeGameTime -= Time.deltaTime;
            }

            //Create float of the counter that's meant to be displayed to the screen
            float displayTime = (float)System.Math.Round(runtimeGameTime, 2);
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

            TimeCounterUI.text = "Time :\n" + displayTime + timeDisplayCorrection;

            //Check if we've run out of time
            if (runtimeGameTime <= 0)
            {
                TimesUp = true;
                runtimeGameTime = 0;
                TimeCounterUI.text = "Time :\n" + runtimeGameTime;
                EndGame();
            }

            //Check if someone's run out of lives
            if (playerOneLives <= 0 || playerTwoLives <= 0)
            {
                EndGame();
            }

        }
    }

    public void ZeroOutTime()
    {
        runtimeGameTime = 0;
    }

    public void StartGame()
    {

        GameStart = true;

        //Spawn enemy in four quads
        //Tell each enemy what waypoint tags to look for
        for (int i = 0; i < enemyCount; i++)
        {
            enemyInstances[i] = GameObject.Instantiate(enemyPrefab, enemySpawnPoints[i].transform.position, enemySpawnPoints[i].transform.rotation);

            if (useQuadrants || modifyWaypointPrefs)
            {
                Enemy enemyInstanceScript = enemyInstances[i].GetComponent<Enemy>();

                if (useQuadrants)
                {
                    enemyInstanceScript.GetWaypointsWithNewTag(waypointQuadrantTags[i]);
                }

                if (modifyWaypointPrefs)
                {
                    enemyInstanceScript.ChangeWaypointPreferences(waypointWaitTime, waypointFollowDistanceLimit, goToRandomWaypoint);
                }
            }

        }

        if (isTwoPlayer)
        {
            runtimeGameTime = twoPlayerGameTime;

            playerTwoLivesUI.enabled = true;
            playerTwoPointsUI.enabled = true;
            playerTwoZone.SetActive(true);

            //Spawn player two, setting the camera/points appropraitely
            playerTwoInstance = Instantiate(playerTwoPrefab, playerTwoSpawnPoint.position, playerTwoPrefab.transform.rotation);
            playerTwoInstance.transform.GetChild(0).GetComponent<ThirdPersonCamera>().SetPlayerTwoCameraForTwoPlayer();


            playerOneLivesUI.enabled = true;
            playerOnePointsUI.enabled = true;
            playerOneZone.SetActive(true);

            //Spawn player one, setting the camera/points appropraitely
            playerOneInstance = Instantiate(playerOnePrefab, playerOneSpawnPoint.position, playerOnePrefab.transform.rotation);
            playerOneInstance.transform.GetChild(0).GetComponent<ThirdPersonCamera>().SetPlayerOneCameraForTwoPlayer();

        }
        else
        {
            runtimeGameTime = singlePlayerGameTime;

            playerOneLivesUI.enabled = true;
            playerOnePointsUI.enabled = true;
            playerOneZone.SetActive(true);

            //Spawn player one
            playerOneInstance = Instantiate(playerOnePrefab, playerOneSpawnPoint.position, playerOnePrefab.transform.rotation);

        }

        EnableItemBases();

        TimeCounterUI.enabled = true;

    }

    private void EndGame()
    {
        if (GameStart)
        {

            inputHandler.SetSelectedGameObject(endGameBackToMenuButton);
            
            //If time is up
            if (TimesUp)
            {
                //If we're in two player mode
                if (isTwoPlayer)
                {
                    TwoPlayerWinTitleUI.enabled = true;

                    if (playerOnePoints > playerTwoPoints)
                    {
                        TwoPlayerWinTitleUI.text = "Player One Wins!";
                        SpawnWinCamera(playerOneInstance);
                        ShowScore(playerOnePoints,true);
                    }
                    else if (playerOnePoints < playerTwoPoints)
                    {
                        TwoPlayerWinTitleUI.text = "Player Two Wins!";
                        SpawnWinCamera(playerTwoInstance);
                        ShowScore(playerTwoPoints);
                    }
                    else
                    {
                        TwoPlayerWinTitleUI.text = "It's a Tie!";
                        ShowScore(playerOnePoints);
                    }

                    nextLevelMenuButton.SetActive(true);

                }
                else //If we're in single player
                {
                    //Show score
                    //If the player just sat there and didn't earn points
                    if (playerOnePoints == 0)
                    {
                        singlePlayerLoseTitleUI.enabled = true;
                        ShowScore(playerOnePoints);
                    }
                    else
                    {
                        singlePlayerWinTitleUI.enabled = true;
                        SpawnWinCamera(playerOneInstance);
                        ShowScore(playerOnePoints);
                        nextLevelMenuButton.SetActive(true);
                    }
                }
            }
            else
            {
                if (isTwoPlayer)
                {
                    TwoPlayerWinTitleUI.enabled = true;

                    if (playerOneLives <= 0)
                    {
                        TwoPlayerWinTitleUI.text = "Player Two Wins!";
                        SpawnWinCamera(playerTwoInstance);
                        ShowScore(playerTwoPoints);
                    }
                    else if (playerOneLives <= 0 && playerTwoLives <= 0)
                    {
                        TwoPlayerWinTitleUI.text = "It's a Tie!";
                        ShowScore(playerOnePoints);
                    }
                    else
                    {
                        TwoPlayerWinTitleUI.text = "Player One Wins!";
                        SpawnWinCamera(playerOneInstance);
                        ShowScore(playerOnePoints);
                    }
                    
                    nextLevelMenuButton.SetActive(true);

                }
                //If player runes out of lives
                else if (playerOneLives <= 0)
                {
                    //Bring up lose screen and show score
                    singlePlayerLoseTitleUI.enabled = true;
                    ShowScore(playerOnePoints);
                }
            }

            //AddUpLives add them to score later

            if (playerOneInstance != null)
            {
                playerOneInstance.GetComponent<Player>().Pause(true);
            }

            if (isTwoPlayer && playerTwoInstance != null)
            {
                playerTwoInstance.GetComponent<Player>().Pause(true);
            }

            for (int i = 0; i < enemyInstances.Length; i++)
            {
                enemyInstances[i].GetComponent<Enemy>().Pause();
            }

            DisableGameUI();

            backButton.SetActive(true);

            GameStart = false;

        }
    }

    public void AddPoints(float pointsToAdd, bool playerOnesZone,bool itemCollected=false)
    {
        if (playerOnesZone)
        {
            playerOnePoints += pointsToAdd;
            playerOnePointsUI.text = "Points : " + playerOnePoints;
        }
        else
        {
            playerTwoPoints += pointsToAdd;
            playerTwoPointsUI.text = "Points : " + playerTwoPoints;
        }

        if(itemCollected && numberOfItemsToCollect != 0)
        {
            totalItemsCollected++;
        }
    }

    private void ShowScore(float winningScore,bool playerOneGotHighScore=false)
    {
        WinningScoreUI.enabled = true;
        WinningScoreUI.text = "Score : " + winningScore;

        if(isTwoPlayer)
        {
            if (PlayerPrefs.GetFloat("p2HighscoreOne", 0) < winningScore)
            {
                PlayerPrefs.SetFloat("TwoPlayerHighScore", winningScore);

                Debug.Log("New highscore for player two leaderboard : "+winningScore);

                PlayerPrefs.SetString("NewTwoPlayerHighScore", "true");

                if(playerOneGotHighScore)
                {
                    PlayerPrefs.SetString("PlayerOneGotTwoPlayerScore", "true");
                }
                else
                {
                    PlayerPrefs.SetString("PlayerOneGotTwoPlayerScore", "false");
                }

                PlayerPrefs.Save();
            }
        }
        else
        {
            if (PlayerPrefs.GetFloat("p1HighscoreOne", 0) < winningScore)
            {
                Debug.Log("New highscore for player one leaderboard : " +winningScore);

                PlayerPrefs.SetFloat("SinglePlayerHighScore", winningScore);
                PlayerPrefs.SetString("NewSinglePlayerHighScore", "true");
                PlayerPrefs.Save();
            }
        }

        PlayerPrefs.SetFloat("PlayerOneSessionPoints", playerOnePoints);
        PlayerPrefs.SetFloat("PlayerTwoSessionPoints", playerTwoPoints);
        PlayerPrefs.Save();
    }

    private void EnableItemBases()
    {
        for (int i = 0; i < itemBases.Length; i++)
        {
            itemBases[i].SetActive(true);
        }
    }

    private void SpawnWinCamera(GameObject objectToSpawnOn)
    {
        Instantiate(WinCamera, objectToSpawnOn.transform.localPosition,objectToSpawnOn.transform.rotation);
    }

    public void Pause()
    {
        paused = true;
    }

    public void Unpause()
    {
        paused = false;
    }
    
	public void IsTwoPlayer()
	{
		isTwoPlayer = true;
        PlayerPrefs.SetInt("PromptedPlayMode", 2);
        PlayerPrefs.Save();
    }

    public void IsSinglePlayer()
    {
        isTwoPlayer = false;
        PlayerPrefs.SetInt("PromptedPlayMode", 1);
        PlayerPrefs.Save();
    }

    void DisableGameUI()
    {
        TimeCounterUI.enabled = false;

        playerOnePointsUI.enabled = false;
        playerOneLivesUI.enabled = false;

        if(isTwoPlayer)
        {
            playerTwoLivesUI.enabled = false;
            playerTwoPointsUI.enabled = false;
        }
    }
}
