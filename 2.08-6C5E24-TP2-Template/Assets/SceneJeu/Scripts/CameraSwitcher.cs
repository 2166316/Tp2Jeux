using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera firstCamera;
    public Camera secondCamera;
    public Camera thirdCamera;

    private int currentCameraIndex = 0;

    void Start()
    {
        SetActiveCamera(1);
    }

    void Update()
    {
       
            if (Input.GetKeyDown(KeyCode.C))
        {
            currentCameraIndex = (currentCameraIndex + 1) % 3;
            SetActiveCamera(currentCameraIndex);
        }
    }

    void SetActiveCamera(int index)
    {
        firstCamera.enabled = false;
        secondCamera.enabled = false;
        thirdCamera.enabled = false;

        switch (index)
        {
            case 0:
                firstCamera.enabled = true;
                break;
            case 1:
                secondCamera.enabled = true;
                break;
            case 2:
                thirdCamera.enabled = true;
                break;
            default:
                Debug.LogError("Camera invalide");
                break;
        }
    }
}