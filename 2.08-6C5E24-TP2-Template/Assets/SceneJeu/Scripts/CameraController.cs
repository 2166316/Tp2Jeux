using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
public class CameraController : Mirror.NetworkBehaviour
{
    public GameObject cameraHolder;
    void Start()
    {
        
    }

    /*public override void OnStartAuthority()
    {
        cameraHolder.SetActive(true);
    }

    public override void OnChange()
    {
        cameraHolder.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().name == "Scene2") {
            cameraHolder.transform.position = Camera.main.transform.position;
        }
    }*/
}
