using UnityEngine;
using Unity.Netcode;
public class CameraSwitcher : NetworkBehaviour
{
    private Camera[] cameras;

    private int currentCameraIndex = 0;

    public override void OnNetworkSpawn()
    {
        if (IsLocalPlayer)
        {
            cameras = GetComponentsInChildren<Camera>(true);
             cameras[0].enabled = true;
        }

        base.OnNetworkSpawn();
    }

    void Start()
    {
        if (IsLocalPlayer)
        {
           // cameras = GetComponentsInChildren<Camera>(true);
           // cameras[0].enabled = true;
        }
    }

    void Update()
    {
        if (!IsLocalPlayer)
            return;


            if (Input.GetKeyDown(KeyCode.C))
            {
                 currentCameraIndex = (currentCameraIndex + 1) % 3;
                SetActiveCamera(currentCameraIndex);
            }
    }

    void SetActiveCamera(int index)
    {

        cameras[0].enabled = false;
        cameras[1].enabled = false;
        cameras[2].enabled = false;

        switch (index)
        {
            case 0:
                cameras[0].enabled = true;
                break;
            case 1:
                cameras[1].enabled = true;
                break;
            case 2:
                cameras[2].enabled = true;
                break;
            default:
                Debug.LogError("Camera invalide");
                break;
        }
    }
}