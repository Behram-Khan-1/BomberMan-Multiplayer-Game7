using Unity.Netcode;
using UnityEngine;

public class BombPlacement : NetworkBehaviour
{
    private PlayerMovement playerMovement;
    Player player;
    [SerializeField] private GameObject bombPrefab;
    public Vector3Int nextCell;
    public Vector2Int previousDir;
    private bool canPlaceBomb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        player = GetComponent<Player>();
        // Listen to movement direction changes to know where to place bombs
        playerMovement.OnMove += UpdateNextCell;
        // When any bomb explodes, regain 1 bomb
        Bomb.OnExplode += IncreaseBombCount;
    }

    void UpdateNextCell(Vector2Int dir)
    {
        if (dir != Vector2Int.zero)
        {
            previousDir = dir;
        }

        nextCell = TilemapPlacement.instance.WorldToCell(transform.position
        + new Vector3(previousDir.x, previousDir.y, 0));
        canPlaceBomb = TilemapPlacement.instance.CanPlaceBombAtCell(nextCell);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) { return; }

        if (Input.GetKeyDown(KeyCode.Space) && canPlaceBomb
            && player.GetPlacedBombCount() <= player.GetBombCount())
        {
            // Tell the server to spawn bomb at nextCell
            SpawnBombRPC(nextCell);
        }
    }
  // Only client calls this; runs on server
    [Rpc(SendTo.Server)]
    private void SpawnBombRPC(Vector3Int targetCell)
    {
        var bomb = Instantiate(bombPrefab,
                  TilemapPlacement.instance.CellWorldCenter(targetCell),
                  Quaternion.identity);

        bomb.GetComponent<NetworkObject>().Spawn(true);
        //Init bomb
        bomb.GetComponent<Bomb>().SetBombRange(player.GetBombRange());
        ReduceBombCount();
    }

    void ReduceBombCount()
    {
        player.SetBombPlacedCountRPC(player.GetPlacedBombCount() + 1);
    }
    void IncreaseBombCount()
    {
        player.SetBombPlacedCountRPC(player.GetPlacedBombCount() - 1);
    }
}
