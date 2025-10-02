using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapPlacement : MonoBehaviour
{
    public static TilemapPlacement instance;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap indestructibleTilemap;
    [SerializeField] private Tilemap destructibleTilemap;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        instance = this;
    }

    private bool IsCellBlocked(Vector3Int cell)
    {
        if (groundTilemap.GetTile(cell) == null)
        {
            var tile = groundTilemap.GetTile(cell);
            // Debug.Log(tile.name);
            return false;
        }
        if (indestructibleTilemap.GetTile(cell) != null)
        {
            var tile = indestructibleTilemap.GetTile(cell);
            // Debug.Log(tile.name);
            return false;
        }
        if (destructibleTilemap.GetTile(cell) != null)
        {
            var tile = destructibleTilemap.GetTile(cell);
            // Debug.Log(tile.name);
            return false;
        }

        return true;
    }

    public bool IsDestructibleCell(Vector3Int cell)
    {
        if (destructibleTilemap.GetTile(cell) != null)
        {
            return true;
        }
        else return false;
    }
    public bool IsIndestructibleCell(Vector3Int cell)
    {
        if (indestructibleTilemap.GetTile(cell) != null)
        {
            return true;
        }
        else return false;
    }

    public bool CanMoveToCell(Vector3Int cell) => IsCellBlocked(cell);
    public bool CanPlaceBombAtCell(Vector3Int cell) => IsCellBlocked(cell);

    public Vector3Int WorldToCell(Vector3 worldPos)
    {
        return groundTilemap.WorldToCell(worldPos);
    }

    public Vector3 CellWorldCenter(Vector3Int cell)
    {
        return groundTilemap.GetCellCenterWorld(cell);
    }

    // public Vector3Int GetDestructibleCell(Vector3Int cell)
    // {
    //     return destructibleTilemap.WorldToCell(cell);
    // }
    public Vector3Int SetDestructibleCell(Vector3Int cell, Tile tile)
    {
        destructibleTilemap.SetTile(cell, tile);
        return cell;
    }

}
