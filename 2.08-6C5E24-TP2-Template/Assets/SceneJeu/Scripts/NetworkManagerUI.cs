

using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : NetworkBehaviour
{
    [SerializeField] private Button HostButton;
    [SerializeField] private Button ClientButton;
    [SerializeField] private TextMeshProUGUI playerData;
    [SerializeField] private NetworkVariable<FixedString64Bytes> infoViePlayer = new NetworkVariable<FixedString64Bytes>("", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private void Awake()
    {
        HostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });

        ClientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
    }

    // [Rpc(SendTo.Server)]
    private void Update()
    {
        playerData.text = infoViePlayer.Value.ToString();

        getData();
    }


    public void getData()
    {
        if (!IsServer) return;
        List<NetworkClient> clients = NetworkManager.Singleton.ConnectedClients.Values.ToList();
        string info = "";
        foreach (NetworkClient client in clients)
        {
            //Debug.Log(client.ClientId);
            //Debug.Log(client.PlayerObject.GetComponent<PickupController>().vie.Value);
            info += "Player " + client.ClientId + " : " + client.PlayerObject.GetComponent<PickupController>().vie.Value + "\n";
        }
        infoViePlayer.Value = info;
        
    }
}