using UnityEngine;

public class UITwoPlayerControllerSwitcher : MonoBehaviour {

    bool playerOnesUsingKeyboard;

    public GameObject playerOneXboxImage;
    public GameObject playerOneKeyboardImage;
    public GameObject playerTwoXboxImage;
    public GameObject playerTwoKeyboardImage;

    private void Start()
    {
        int keyboardValue = PlayerPrefs.GetInt("PlayerOneInputDevice");
        playerOnesUsingKeyboard = (keyboardValue == 1) ? true : false;
        SwitchControllers();
    }

    public void SwitchControllers()
    {
        if(playerOnesUsingKeyboard)
        {
            playerOneKeyboardImage.SetActive(true);
            playerOneXboxImage.SetActive(false);
            playerTwoKeyboardImage.SetActive(false);
            playerTwoXboxImage.SetActive(true);
            PlayerPrefs.SetInt("PlayerOneInputDevice",1);
            PlayerPrefs.SetInt("PlayerTwoInputDevice", 0);
            playerOnesUsingKeyboard = false;
        }
        else
        {
            playerOneKeyboardImage.SetActive(false);
            playerOneXboxImage.SetActive(true);
            playerTwoKeyboardImage.SetActive(true);
            playerTwoXboxImage.SetActive(false);
            PlayerPrefs.SetInt("PlayerOneInputDevice", 0);
            PlayerPrefs.SetInt("PlayerTwoInputDevice", 1);
            playerOnesUsingKeyboard = true;
        }
    }

}
