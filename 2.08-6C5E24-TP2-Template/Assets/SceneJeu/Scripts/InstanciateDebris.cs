using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Networking;

public class InstanciateDebris : NetworkBehaviour
{
    [SerializeField] public GameObject bacPoubelle;
    [SerializeField] public GameObject spectateur;
    [SerializeField] private List<Vector3> listDePositionPredefinie;
    [SerializeField] private List<Vector3> listDePositionSpectateur;
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

        listDePositionSpectateur = new List<Vector3>
        {
            new Vector3(-344.846f, 69.66448f, 16.99996f),
            new Vector3(-348.846f, 69.66448f, 16.99996f),
            new Vector3(-354.646f, 69.66448f, 16.99996f),
            new Vector3(-344.846f, 69.66448f, 3f),
            new Vector3(-348.846f, 69.66448f, 3f),
            new Vector3(-354.646f, 69.66448f, 3f)
        };

        if (IsHost)
        {
            InstantiePoubelles();
            InstantieSpectateurs();
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

    private void InstantieSpectateurs()
    {
        int i = 0;
        foreach (Vector3 t in listDePositionSpectateur)
        {
            i++;
            GameObject spectateurObj = null;
            if (i > 3)
            {
               spectateurObj = Instantiate(spectateur, t, new Quaternion(0f, 0f, 0f, 0f));
            }
            else
            {
                spectateurObj = Instantiate(spectateur, t, new Quaternion(0f, 180f, 0f, 0f));
            }

            NetworkObject networkObject = spectateurObj.GetComponent<NetworkObject>();
            if (networkObject != null)
            {
                networkObject.Spawn();
            }

        }
    }

}
