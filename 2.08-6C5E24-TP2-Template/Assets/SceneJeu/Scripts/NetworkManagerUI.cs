using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : NetworkBehaviour
{
    [SerializeField] private Button HostButton;
    [SerializeField] private Button ClientButton;
    [SerializeField] private TextMeshProUGUI playerNum;
    private readonly NetworkVariable<int> playersCount = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private void Awake()
    {
        HostButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
        });

        ClientButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
        });

        playersCount.OnValueChanged += OnPlayerCountChanged;
    }
    public void RegisterPlayer()
    {
        playersCount.Value++;
    }

    private void OnPlayerCountChanged(int previousValue, int newValue)
    {
        playerNum.text = newValue.ToString();
    }
}