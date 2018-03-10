using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuOptions : MonoBehaviour {

    private GameObject player;
    private GameObject[] enemys;

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void SetMusicPref(float value)
    {
        PlayerPrefs.SetFloat("Music", value);
    }

    public void SetSoundPref(float value)
    {
        PlayerPrefs.SetFloat("Sound", value);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseObjects()
    {
        if (player == null || enemys == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            enemys = GameObject.FindGameObjectsWithTag("Enemy");
        }

        player.SetActive(false);
        
        for(int i = 0; i < enemys.Length; i++)
        {
            enemys[i].SetActive(false);
        }
    }

    public void UnpauseObjects()
    {
        if (player == null || enemys == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            enemys = GameObject.FindGameObjectsWithTag("Enemy");
        }

        player.SetActive(true);

        for (int i = 0; i < enemys.Length; i++)
        {
            enemys[i].SetActive(true);
        }
    }
}
