using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    [SerializeField] private float pushSpeed = 2f;
    [SerializeField] private Vector2Int pushDirection = Vector2Int.zero;
    private int bombRange;

    List<Vector3Int> explosionTiles = new List<Vector3Int>();
    private Vector3Int bombCell;

    public GameObject redDebugDot;
    public bool isDebugging = false;
    private bool isPushing = false;
    public static event Action<Vector3Int> OnTileDestroy;
    public static event Action OnExplode;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // GetComponent<BoxCollider2D>().enabled = false;
        bombCell = TilemapPlacement.instance.WorldToCell(transform.position);
        nextCell = bombCell + new Vector3Int(pushDirection.x, pushDirection.y, 0);
        nextCellCenter = TilemapPlacement.instance.CellWorldCenter(nextCell);

    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(TimerWithCallback(3f, Explode));
        if (isPushing)
        {
            Pushing();
        }
    }

    Vector3Int nextCell;
    Vector3 nextCellCenter;
    void Pushing()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextCellCenter, pushSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, nextCellCenter) < 0.1f)
        {
            transform.position = nextCellCenter;
            bombCell = TilemapPlacement.instance.WorldToCell(transform.position);
            nextCell = bombCell + new Vector3Int(pushDirection.x, pushDirection.y, 0);
            nextCellCenter = TilemapPlacement.instance.CellWorldCenter(nextCell);

            RaycastHit2D hit = Physics2D.Raycast(transform.position
             + new Vector3(pushDirection.x, pushDirection.y, 0) / 2, pushDirection, 0.5f);


            if (!TilemapPlacement.instance.CanMoveToCell(nextCell)
                || hit.collider != null && hit.collider.CompareTag("Bomb"))
            {
                isPushing = false;
                Debug.Log("Stopped Pushing");
                return;
            }
        }
    }


    void Explode()
    {
        explosionTiles = new List<Vector3Int>();
        List<Vector2Int> directions = new List<Vector2Int>()
        { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        //PlayAnimation
        bombCell = TilemapPlacement.instance.WorldToCell(transform.position);
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
        if (isDebugging)
        {
            foreach (var cell in explosionTiles)
            {
                Vector3 worldPos = TilemapPlacement.instance.CellWorldCenter(cell);
                Instantiate(redDebugDot, worldPos, Quaternion.identity);
            }
        }
        BombDestroy();

    }

    void BombDestroy()
    {
        DestroyBlocks();
        KillMobs();
        OnExplode?.Invoke();
        GetComponent<NetworkObject>().Despawn();
        // Destroy(gameObject);
    }

    private void DestroyBlocks()
    {
        for (int i = 0; i < explosionTiles.Count; i++)
        {
            if (TilemapPlacement.instance.IsDestructibleCell(explosionTiles[i]))
            {
                TilemapPlacement.instance.SetDestructibleCell(explosionTiles[i], null);
                OnTileDestroy?.Invoke(explosionTiles[i]);
            }
        }
    }

    private void KillMobs()
    {
        GameManager.instance.Bomb_ExplosionDeath(explosionTiles);
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
    public void SetIsPushing(bool isPushing, Vector2Int dir)
    {
        this.isPushing = isPushing;
        pushDirection = dir;
    }
    public void SetBombRange(int range)
    {
        bombRange = range;
    }

}
