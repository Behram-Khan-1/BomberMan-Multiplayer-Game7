using UnityEngine;

//Add Death effects

//Add 2nd mob that chases
//Move to multiplayer
public class Player : MonoBehaviour, IDamagable
{
    private Vector3Int startingCell;
    private Vector3Int currentPlayerCell;
    [SerializeField] private int bombRange = 2;
    [SerializeField] private int bombCount = 1;
    [SerializeField] private int bombPlaced = 0;
    [SerializeField] private int initialBombRange = 2;
    [SerializeField] private int initialBombCount = 1;
    [SerializeField] private int initialBombPlaced = 0;
    [SerializeField] private int lifes = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
        bombRange = initialBombRange;
        bombCount = initialBombCount;
        bombPlaced = initialBombPlaced;

        GetComponent<PlayerMovement>().targetPos.Value = TilemapPlacement.instance.CellWorldCenter(startingCell);
        transform.position = TilemapPlacement.instance.CellWorldCenter(startingCell);
    }
    public int GetBombRange()
    {
        return bombRange;
    }
    public int GetBombCount()
    { return bombCount; }
    public int IncreaseBombCount()
    { return bombCount; }
    public int GetPlacedBombCount()
    { return bombPlaced; }
    public void SetBombPlacedCount(int bombPlaced)
    { this.bombPlaced = bombPlaced; }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PowerUp>() != null)
        {
            PowerupType powerupType = collision.gameObject.GetComponent<PowerUp>().powerupType;
            if (powerupType == PowerupType.ExtraBomb)
            {
                Debug.Log("Player Got Extra Bomb");
                bombCount++;
            }
            else if (powerupType == PowerupType.ExtraLife)
            {
                Debug.Log("Player Got Extra Life");
                lifes++;
            }
            else if (powerupType == PowerupType.ExtraRange)
            {
                Debug.Log("Player Got Extra Range");
                bombRange++;
            }
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage();
        }
    }

}
