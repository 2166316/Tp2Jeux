using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Linq;

public class CameraSwitcher : NetworkBehaviour
{
    private List<Camera> cameras;

    private int currentCameraIndex = 0;

    public override void OnNetworkSpawn()
    {
       
        if (IsLocalPlayer)
        {
            cameras = GetComponentsInChildren<Camera>(true).ToList();

            if (!IsOwner) return;

            cameras[0].enabled = true;
        }
        cameras.Add(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>());
        base.OnNetworkSpawn();
    }

    void Update()
    {
        if (!IsLocalPlayer)
            return;


            if (Input.GetKeyDown(KeyCode.C))
            {
                currentCameraIndex = (currentCameraIndex + 1) % cameras.Count;
                SetActiveCamera(currentCameraIndex);
            }
    }

    void SetActiveCamera(int index)
    {

        cameras[0].enabled = false;
        cameras[1].enabled = false;
        cameras[2].enabled = false;
        cameras[3].enabled = false;

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
            case 3:
                cameras[3].enabled = true;
                break;
            default:
                Debug.LogError("Camera invalide");
                break;
        }
    }
}