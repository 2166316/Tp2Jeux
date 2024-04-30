using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : NetworkBehaviour
{
    [SerializeField] private Button HostButton;
    [SerializeField] private Button ClientButton;
    [SerializeField] private TextMeshProUGUI playerNum;
    private void Awake()
    {
        HostButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
        });

        ClientButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
        });
    }
}