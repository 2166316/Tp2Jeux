using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Networking;

public class InstanciateDebris : NetworkBehaviour
{
    [SerializeField] public GameObject bacPoubelle;
    [SerializeField] private List<Vector3> listDePositionPredefinie;
    public override void OnNetworkSpawn()
    {
       // Debug.Log("spawn");
        listDePositionPredefinie = new List<Vector3>
        {
            new Vector3(-360.3016f, 69.86519f, 56.70361f),
            new Vector3(-357.3016f, 69.86519f, 56.70361f),
            new Vector3(-266.01f, 69.86519f, 56.70361f),
            new Vector3(-209.18f, 69.86519f, 65.03f),
            new Vector3(-226.41f, 69.86519f, 65.03f),
            new Vector3(-240.91f, 69.86519f, 64.68f)
        };


        if (IsHost)
        {
            InstantiePoubelles();
        }    
    }


    private void InstantiePoubelles()
    {
       // Debug.Log("Spawn");
        foreach (Vector3 t in listDePositionPredefinie)
        {
            GameObject poubelle = Instantiate(bacPoubelle, t, new Quaternion(0f,180f,0f,0f));
            NetworkObject networkObject = poubelle.GetComponent<NetworkObject>();
            if (networkObject != null)
            {

                networkObject.Spawn();
               
            }
            
        }
    }

}
