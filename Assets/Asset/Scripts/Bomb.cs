using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class Bomb : MonoBehaviour
{
    //Make it explode after timer
    //Make it go 3x3 blocks 
    //Stop at obstacles and destroy them

    //Kill enemies and us.
    //Kick Bomb some blocks far
    //Walkable for 1 sec then be solid
    [SerializeField] private int bombRange = 2;
    public GameObject redDebugDot;
    List<Vector3Int> explosionTiles = new List<Vector3Int>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<BoxCollider2D>().enabled = false;
      
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(TimerWithCallback(3f, Explode));
    }

    void Explode()
    {
        explosionTiles = new List<Vector3Int>();
        List<Vector2Int> directions = new List<Vector2Int>()
        { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        //PlayAnimation
        var bombCell = TilemapPlacement.instance.WorldToCell(transform.position);
        explosionTiles.Add(bombCell);
        for (int j = 0; j < directions.Count; j++) // 0 1 2 3
        {
            for (int i = 1; i <= bombRange; i++) // 1 2 3
            {
                var explosionDirection = directions[j] * i; // up x 1, x2, x3
                var explosionCell = bombCell + new Vector3Int(explosionDirection.x, explosionDirection.y, 0); // up x 1, x2, x3[j].x, directions[j].y, 0);
                if (TilemapPlacement.instance.IsDestructibleCell(explosionCell))
                {
                    explosionTiles.Add(explosionCell);
                    break;
                }
                if (TilemapPlacement.instance.IsIndestructibleCell(explosionCell))
                {
                    break;
                }

                explosionTiles.Add(explosionCell);
            }
        }
        // 🔴 Spawn red dots at each explosion cell center
        foreach (var cell in explosionTiles)
        {
            Vector3 worldPos = TilemapPlacement.instance.CellWorldCenter(cell);
            Instantiate(redDebugDot, worldPos, Quaternion.identity);
        }

        DestroyBlocks();
        Destroy(gameObject);
        KillMobs();
    }

    private void DestroyBlocks()
    {
        for (int i = 0; i < explosionTiles.Count; i++)
        {
            if (TilemapPlacement.instance.IsDestructibleCell(explosionTiles[i]))
            {
                TilemapPlacement.instance.SetDestructibleCell(explosionTiles[i], null);
            }
        }
    }

    private void KillMobs()
    {
        foreach (var cell in explosionTiles)
        {
            if (cell == GameManager.instance.currentPlayerCell)
            {
                GameManager.instance.PlayerDied();
            }
        }
        
    }


    IEnumerator TimerWithCallback(float seconds, Action callback)
    {
        yield return new WaitForSeconds(seconds);
        callback();
    }
    IEnumerator Timer(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}
