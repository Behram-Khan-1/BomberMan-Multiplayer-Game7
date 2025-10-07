using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;
    private List<IDamagable> entities = new List<IDamagable>();

    [SerializeField] private GameObject playerPrefabs;
    [SerializeField] private List<Transform> spawnPoint;

    void Awake()
    {
        instance = this;
        var cell1 = TilemapPlacement.instance.WorldToCell(spawnPoint[0].position);
        var cell2 = TilemapPlacement.instance.WorldToCell(spawnPoint[1].position);
        var cell1Center = TilemapPlacement.instance.CellWorldCenter(cell1);
        var cell2Center = TilemapPlacement.instance.CellWorldCenter(cell2);

        spawnPoint[0].position = cell1Center;
        spawnPoint[1].position = cell2Center;
    }

    override public void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }
        Debug.Log(NetworkManager.Singleton.LocalClientId);
    }

    private void OnClientConnected(ulong clientId)
    {
        var spawnedPlayer = Instantiate(playerPrefabs, spawnPoint[((int)clientId)].position, Quaternion.identity);
        spawnedPlayer.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
    }

    //Events

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {

    }

    public void AddEntity(IDamagable damagable)
    {
        entities.Add(damagable);
    }

    public void RemoveEntity(IDamagable damagable)
    {
        entities.Remove(damagable);
    }

    public void PlayerDied()
    {
        Debug.Log("Player Died");
    }

    //Bomb Logic
    public void Bomb_ExplosionDeath(List<Vector3Int> explosionTiles)
    {
        //Check all entities if they are in explosionTiles or not.
        for (int i = 0; i < explosionTiles.Count; i++)
        {
            for (int j = 0; j < entities.Count; j++)
            {
                if (explosionTiles[i] == entities[j].GetCurrentCell())
                {
                    entities[j].TakeDamage();
                }
            }
        }
    }

}
