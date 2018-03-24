using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{

    [Header("Target to Follow")]
    public Transform target;

    [Header("Distance from Target")]
    public float distance = 10f;

    [Header("Y Rotation Limit")]
    public float maxYAngle = 50.0f;
    public float minYAngle = 0f;

    [Header("Camera Sensitivity")]
    public float sensitivityX = 4.0f;
    public float sensitivityY = 1.0f;

    [Header("Other")]
    public Transform playerMesh;
    public bool isTheSecondPlayer;
    public bool paused;
    public bool ClickToMoveCamera;

    public bool usingKeyboard = true;
    private bool dead;
    private Quaternion Xrotation,Yrotation;
    private float currentX, currentY;

    private void Start()
    {
        currentY += Input.GetAxis("Mouse Y");
        currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);
        int inputValue;

        if (!isTheSecondPlayer)
        {
            inputValue = PlayerPrefs.GetInt("PlayerOneInputDevice");
        }
        else
        {
            inputValue = PlayerPrefs.GetInt("PlayerTwoInputDevice");
        }

        //Implement later when we get to do multiple input devices
        usingKeyboard = (inputValue == 1) ? true : false;

    }

    // Update is called once per frame
    void Update ()
    {
        if (!paused)
        {
            if (ClickToMoveCamera)
            {
                if (Input.GetButton("Fire2") && usingKeyboard)
                {
                    currentX += Input.GetAxis("Mouse X");
                    currentY += Input.GetAxis("Mouse Y");

                    currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);
                }
                else if (Input.GetButton("ControllerFire2") && !usingKeyboard)
                {
                    //Controller input
                    currentX += Input.GetAxis("ControllerMouse X");
                    currentY += Input.GetAxis("ControllerMouse Y");

                    currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);
                }
            }
            else
            {
                if (usingKeyboard)
                {
                    currentX += Input.GetAxis("Mouse X");
                    currentY += -Input.GetAxis("Mouse Y");

                    currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);
                }
                else if (!usingKeyboard)
                {
                    //Controller input
                    currentX += Input.GetAxis("ControllerMouse X");
                    currentY += Input.GetAxis("ControllerMouse Y");

                    currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (!paused)
        {
            Vector3 dir = new Vector3(0, 0, -distance);

            Yrotation = Quaternion.Euler(currentY, currentX, 0);
            Xrotation = Quaternion.Euler(0, currentX, 0);

            transform.position = target.position + Yrotation * dir;

            if (!dead)
            {
                playerMesh.rotation = Xrotation;
            }

            transform.LookAt(target.position);
        }
    }

    public void SetPlayerOneCameraForTwoPlayer()
    {
        Camera cam = GetComponent<Camera>();
        cam.rect = new Rect(0f,0.5f,1f,1f);
    }

    public void SetPlayerTwoCameraForTwoPlayer()
    {
        Camera cam = GetComponent<Camera>();
        cam.rect = new Rect(0f, -0.5f, 1f, 1f);
    }

    public void Pause()
    {
        paused = true;
    }

    public void Unpause()
    {
        paused = false;
    }

    public void PlayerIsDead()
    {
        dead = true;
    }
}
