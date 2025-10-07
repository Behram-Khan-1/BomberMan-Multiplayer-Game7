using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkUIManager : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hostButton.onClick.AddListener(HostClicked);
        clientButton.onClick.AddListener(ClientClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HostClicked()
    {
        NetworkManager.Singleton.StartHost();
    }

    void ClientClicked()
    {
        NetworkManager.Singleton.StartClient();
    }
}

