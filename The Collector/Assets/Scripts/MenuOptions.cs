using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuOptions : MonoBehaviour {
    
    public bool notFading;
    public EventSystem inputHandler;
    public GameObject ButtonToSelectOnPause;
    private GameObject[] players;
    private GameObject[] enemys;
    private Image fadeMat;
    private float maxMatValue = 1f;
    private float fadeOutIncrement = 0.02f;
    private float fadeInIncrement = 0.02f;
    private string scene;
    private bool fadingOut;
    private bool fadingIn;
    private bool goingToNextLevel;

    private void Start()
    {

        if (fadeMat == null && !notFading)
        {
            fadeMat = transform.GetChild(0).GetComponent<Image>();

            if(fadeMat.material.color.a >= maxMatValue)
            {
                fadingIn = true;
            }
            else
            {
                fadeMat.material.color = new Color();
            }
        }
        
    }
    
    private void FixedUpdate()
    {
        //If we are fading out
        if (fadingOut)
        {

            //Get the color on this material 
            Color color = fadeMat.material.color;

            //Increment the Alpha channel
            float newMatValue = color.a + fadeOutIncrement;

            //Set the alpha channel to this new value
            color.a = newMatValue;

            //Apply this new color to the material's color
            fadeMat.material.color = color;
            
            //If we have faded out, load the next scene
            if (newMatValue >= maxMatValue)
            {
                SceneManager.LoadScene(scene);
            }
        }
        //If we're fading in
        else if(fadingIn)
        {

            //Get the color on this material 
            Color color = fadeMat.material.color;

            //Decrement the Alpha channel
            float newMatValue = color.a - fadeInIncrement;

            //Set the alpha channel to this new value
            color.a = newMatValue;

            //Apply this new color to the material's color
            fadeMat.material.color = color;

            //If we have faded out, load the next scene
            if (newMatValue <= 0)
            {
                fadingIn = false;
            }
        }
    }

    // -----  Functions for Main Menu / Pause Menu UI  ------

    /// <summary>
    /// Takes the level name and begin to fadeOut, level loads after fadeOut is complete
    /// </summary>
    /// <param name="levelName"></param>
    public void LoadLevel(string levelName)
    {
        //Check if there are any new high scores
        if (!goingToNextLevel&&(PlayerPrefs.GetString("NewSinglePlayerHighScore") == "true" || PlayerPrefs.GetString("NewTwoPlayerHighScore") == "true"))
        {
            //If so, Load Leaderboard scene immedietly
            scene = "Leaderboard";
        }
        else
        {
            scene = levelName;
        }

        fadingOut = true;
    }

    /// <summary>
    /// Sets Music volume preference
    /// </summary>
    /// <param name="value"></param>
    public void SetMusicPref(float value)
    {
        PlayerPrefs.SetFloat("Music", value);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Sets Sound volume preference
    /// </summary>
    /// <param name="value"></param>
    public void SetSoundPref(float value)
    {
        PlayerPrefs.SetFloat("Sound", value);
        PlayerPrefs.Save();
    }

    public void NextLevel()
    {
        goingToNextLevel = true;
    }

    /// <summary>
    /// Shuts down the Game/Application
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Finds player/enemy objects and calls functions in order to 'pause' the game
    /// </summary>
    public void PauseObjects(bool usingController = false)
    {
        if (players == null || enemys == null)
        {
            players = GameObject.FindGameObjectsWithTag("Player");
            enemys = GameObject.FindGameObjectsWithTag("Enemy");
        }
        
        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<Player>().Pause(true);
        }

        for (int i = 0; i < enemys.Length; i++)
        {
            enemys[i].GetComponent<Enemy>().Pause();
        }

        if(usingController && inputHandler != null && ButtonToSelectOnPause != null)
        {
            inputHandler.SetSelectedGameObject(ButtonToSelectOnPause);
        }
    }

    /// <summary>
    /// Finds player/enemy objects and calls functions in order to 'unpause' the game
    /// </summary>
    public void UnpauseObjects()
    {
        if (players == null || enemys == null)
        {
            players = GameObject.FindGameObjectsWithTag("Player");
            enemys = GameObject.FindGameObjectsWithTag("Enemy");
        }

        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<Player>().Unpause(true);
        }

        for (int i = 0; i < enemys.Length; i++)
        {
            enemys[i].GetComponent<Enemy>().Unpause();
        }
    }
}
