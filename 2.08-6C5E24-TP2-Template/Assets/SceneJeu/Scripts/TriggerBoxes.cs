using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TriggerBoxes : NetworkBehaviour
{
    // Start is called before the first frame update
    public GameObject boxPrefab;
    void Start()
    {
        
    }

    [Rpc(SendTo.Server)]
    private void SpawnBoxRpc()
    {
        if (!IsServer) return;
        for (int i = 0; i < 3; i++)
        {
            GameObject box = Instantiate(boxPrefab, new Vector3(-316.524f, 76.232f, Random.Range(23f, 30f)), Quaternion.identity);
            var instanceNetworkObject = box.GetComponent<NetworkObject>();
            instanceNetworkObject.Spawn();
            box.GetComponent<Rigidbody>().AddForce(new Vector3(-5, 1, 0), ForceMode.Impulse);
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            SpawnBoxRpc();
        }
    }
}
