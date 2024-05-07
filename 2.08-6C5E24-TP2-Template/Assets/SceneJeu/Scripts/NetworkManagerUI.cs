

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
    [SerializeField] private NetworkVariable<FixedString4096Bytes> infoViePlayer = new NetworkVariable<FixedString4096Bytes>("", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server); 

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

    private void Update()
    {
        playerData.text = infoViePlayer.Value.ToString();
        GetData();
    }


    public void GetData()
    {
        if (!IsServer) return;
        List<NetworkClient> clients = NetworkManager.Singleton.ConnectedClients.Values.ToList();
        string info = "";
        info += "Nombre de joueurs : " + clients.Count + "\n";
        foreach (NetworkClient client in clients)
        {
            if (client.PlayerObject.GetComponent<PickupController>().vie.Value >0)
            {
                info += "Player " + client.ClientId + " : " + client.PlayerObject.GetComponent<PickupController>().vie.Value + "\n";
            }
            else
            {
                info += "Player " + client.ClientId + " : Dead \n";
            }
            
            
        }
        infoViePlayer.Value = info;
        
    }
}