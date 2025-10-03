using UnityEngine;

public class BombPlacement : MonoBehaviour
{
    private PlayerMovement playerMovement;
    Player player;
    [SerializeField] private GameObject bombPrefab;
    private Vector3Int nextCell;
    Vector2Int previousDir;
    private bool canPlaceBomb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        playerMovement = GetComponent<PlayerMovement>();
        playerMovement.OnMove += PlaceBomb;
        player = GetComponent<Player>();

        Bomb.OnExplode += IncreaseBombCount;
    }

    void PlaceBomb(Vector2Int dir)
    {
        if (dir != Vector2Int.zero)
        {
            previousDir = dir;
        }

        nextCell = TilemapPlacement.instance.WorldToCell(transform.position
        + new Vector3(previousDir.x, previousDir.y, 0));

        if (TilemapPlacement.instance.CanPlaceBombAtCell(nextCell))
        {
            canPlaceBomb = true;
        }
        else
        {
            canPlaceBomb = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canPlaceBomb
            && player.GetPlacedBombCount() <= player.GetBombCount()
        )
        {
            var bomb = Instantiate(bombPrefab,
            TilemapPlacement.instance.CellWorldCenter(nextCell),
            Quaternion.identity);

            bomb.GetComponent<Bomb>().SetBombRange(player.GetBombRange());
            ReduceBombCount();
        }
    }

    void ReduceBombCount()
    {
        player.SetBombPlacedCount(player.GetPlacedBombCount() + 1);
    }
    void IncreaseBombCount()
    {
        player.SetBombPlacedCount(player.GetPlacedBombCount() - 1);
    }
}
