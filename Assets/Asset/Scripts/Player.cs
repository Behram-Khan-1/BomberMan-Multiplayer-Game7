using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3Int currentPlayerCell;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        currentPlayerCell = TilemapPlacement.instance.WorldToCell(transform.position);
        GameManager.instance.PlayerPosition(currentPlayerCell);
    }

 
}
