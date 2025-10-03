using System.Collections.Generic;
using UnityEngine;

public class DropManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> powerups;
    [SerializeField] private int dropChance = 10;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Bomb.OnTileDestroy += Bomb_DropPowerup; 
    }

    void Bomb_DropPowerup(Vector3Int dropCell)
    {
        if (Random.Range(0, 100) < dropChance)
        {
            Instantiate(powerups[Random.Range(0, powerups.Count)],
            TilemapPlacement.instance.CellWorldCenter(dropCell), Quaternion.identity);
        }
    }

    
}
