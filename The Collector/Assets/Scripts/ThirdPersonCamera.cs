using System.Collections;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {

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

    private bool dead;
    private Quaternion Xrotation,Yrotation;
    private float currentX, currentY;

    private void Start()
    {
        currentY += Input.GetAxis("Mouse Y");
        currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetButton("Fire2"))
        {
            currentX += Input.GetAxis("Mouse X");
            currentY += -Input.GetAxis("Mouse Y");

            currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);
        }
    }

    private void LateUpdate()
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

    public void PlayerIsDead()
    {
        dead = true;
    }
}
