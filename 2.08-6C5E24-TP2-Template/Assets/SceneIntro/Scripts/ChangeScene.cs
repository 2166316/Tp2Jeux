using System;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class ChangeScene : NetworkBehaviour
{
    public void LoadScene(string name)
    {
        try
        {
            TestServerRPC();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void TestServerRPC(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        Debug.Log("id : " + clientId);
        NetworkObject clientPickup = NetworkManager.Singleton.ConnectedClients.Values.ToList().FirstOrDefault(n => n.ClientId == clientId).PlayerObject;
        if (clientPickup != null)
        {
            NetworkManager.Singleton.DisconnectClient(clientId);
            clientPickup.Despawn();
        }
    }
}