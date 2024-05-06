using Mirror;
using Mirror.Experimental;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using NetworkBehaviour = Unity.Netcode.NetworkBehaviour;

public class CollisionController : NetworkBehaviour
{
    private List<NetworkObject> objects = new ();
    public float distance = 5;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectsWithTag("Poubelle").ToList().ForEach(poubelle => objects.Add(poubelle.GetComponent<NetworkObject>()));
    }

    // Update is called once per frame
    void Update()
    {
        if(IsServer)
        {
            foreach(var player in NetworkManager.ConnectedClients)
            {
                foreach(NetworkObject objectss in objects)
                {
                    float distanceV = Vector3.Distance(player.Value.PlayerObject.transform.position, objectss.gameObject.transform.position);
                    if (distanceV < distance) objectss.ChangeOwnership(player.Value.ClientId);
                }
                
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
    }
}
