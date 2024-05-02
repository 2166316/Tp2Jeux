using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : NetworkBehaviour
{
    [SerializeField] private Button HostButton;
    [SerializeField] private Button ClientButton;
    [SerializeField] private TextMeshProUGUI playerNum;
    private NetworkVariable<int> playersCount = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);
    private void Awake()
    {
        HostButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
        });

        ClientButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
        });
    }

    public void Update()
    {
       if(!IsServer) return;
       playersCount.Value = NetworkManager.Singleton.ConnectedClients.Count;
       playerNum.text = playersCount.Value.ToString();
    }
}