using Unity.Netcode;
using UnityEngine;

//Add Death effects

//Add 2nd mob that chases
//Move to multiplayer
public class Player : NetworkBehaviour, IDamagable
{
    private Vector3Int startingCell;
    private Vector3Int currentPlayerCell;
    [SerializeField] private NetworkVariable<int> bombRange = new();
    [SerializeField] private NetworkVariable<int> bombCount = new();
    [SerializeField] private NetworkVariable<int> bombPlaced = new();
    [SerializeField] private int initialBombRange = 2;
    [SerializeField] private int initialBombCount = 1;
    [SerializeField] private int initialBombPlaced = 0;
    [SerializeField] private int lifes = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    override public void OnNetworkSpawn()
    {
        if (IsServer)
        {
            bombRange.Value = 2;
            bombCount.Value = 1;
            bombPlaced.Value = 0;
        }
        // bombPlaced.OnValueChanged += 
        
    }
    void Start()
    {
        GameManager.instance.AddEntity(this);
        startingCell = TilemapPlacement.instance.WorldToCell(transform.position);

    }

    // Update is called once per frame
    void Update()
    {
        currentPlayerCell = TilemapPlacement.instance.WorldToCell(transform.position);
    }

    public Vector3Int GetCurrentCell()
    {
        return currentPlayerCell;
    }

    public void TakeDamage()
    {
        Debug.Log("Player Took Damage");
        lifes--;
        //Respawn
        //Later make player drop all the powerups so other can rush to it.
        if (lifes <= 0)
        {
            GameManager.instance.PlayerDied();
            GameManager.instance.RemoveEntity(this);
        }
        Respawn();
    }

    private void Respawn()
    {
        bombRange.Value = initialBombRange;
        bombCount.Value = initialBombCount;
        bombPlaced.Value = initialBombPlaced;

        GetComponent<PlayerMovement>().targetPos.Value = TilemapPlacement.instance.CellWorldCenter(startingCell);
        transform.position = TilemapPlacement.instance.CellWorldCenter(startingCell);
    }
    public int GetBombRange()
    {
        return bombRange.Value;
    }
    public int GetBombCount()
    { return bombCount.Value; }
    public int IncreaseBombCount()
    { return bombCount.Value; }
    public int GetPlacedBombCount()
    { return bombPlaced.Value; }
    [Rpc(SendTo.Server)]
    public void SetBombPlacedCountRPC(int bombPlaced)
    { this.bombPlaced.Value = bombPlaced; }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PowerUp>() != null)
        {
            PowerupType powerupType = collision.gameObject.GetComponent<PowerUp>().powerupType;
            if (powerupType == PowerupType.ExtraBomb)
            {
                Debug.Log("Player Got Extra Bomb");
                bombCount.Value++;
            }
            else if (powerupType == PowerupType.ExtraLife)
            {
                Debug.Log("Player Got Extra Life");
                lifes++;
            }
            else if (powerupType == PowerupType.ExtraRange)
            {
                Debug.Log("Player Got Extra Range");
                bombRange.Value++;
            }
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage();
        }
    }

}
