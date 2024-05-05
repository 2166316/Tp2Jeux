using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class BoxLifeController : NetworkBehaviour
{

    [SerializeField] public float distanceMin = 5f;
    void Start()
    {
        //vie 30secondes
        StartCoroutine(LifeTime());
    }

    void Update()
    {
        if(!IsServer) return;

        if(NetworkManager.ConnectedClients.Count <= 0) return;

        AssigneAuthoriteJoueurPlusProche();
    }

    private void AssigneAuthoriteJoueurPlusProche()
    {
        foreach (var player in NetworkManager.ConnectedClients)
        {
            float distanceV = Vector3.Distance(player.Value.PlayerObject.transform.position, this.gameObject.transform.position);
            if (distanceV < distanceMin) this.GetComponent<NetworkObject>().ChangeOwnership(player.Value.ClientId);
        }
    }

    [Rpc(SendTo.Server)]
    public void DespawnRpc()
    {
        try {
            var instanceNetworkObject = gameObject.GetComponent<NetworkObject>();
            instanceNetworkObject.Despawn();
        } 
        catch {
        }
    }


    public IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(30f);
        DespawnRpc();
        yield return new WaitForSeconds(1f);
    }
}
