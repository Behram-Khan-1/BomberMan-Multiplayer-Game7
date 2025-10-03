using UnityEngine;

public class Player : MonoBehaviour, IDamagable
{
    private Vector3Int currentPlayerCell;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.instance.AddEntity(this);
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
        GameManager.instance.PlayerDied();
    }
}
