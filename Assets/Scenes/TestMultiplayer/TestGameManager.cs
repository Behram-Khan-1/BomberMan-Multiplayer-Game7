using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TestGameManager : NetworkBehaviour
{
    public List<Transform> spawnPoint;
    public GameObject playerPrefab;
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }
    }

    void OnClientConnected(ulong clientId)
    {
        var spawnedPlayer = Instantiate(playerPrefab, spawnPoint[((int)clientId)].position, Quaternion.identity);
        spawnedPlayer.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
