using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private List<IDamagable> entities = new List<IDamagable>();

    //Events

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddEntity(IDamagable damagable)
    {
        entities.Add(damagable);
    }

    public void RemoveEntity(IDamagable damagable)
    {
        entities.Remove(damagable);
    }

    public void PlayerDied()
    {
        Debug.Log("Player Died");
    }

    //Bomb Logic
    public void Bomb_ExplosionDeath(List<Vector3Int> explosionTiles)
    {
        //Check all entities if they are in explosionTiles or not.
        for (int i = 0; i < explosionTiles.Count; i++)
        {
            for (int j = 0; j < entities.Count; j++)
            {
                if (explosionTiles[i] == entities[j].GetCurrentCell())
                {
                    entities[j].TakeDamage();
                }
            }
        }
    }

}
