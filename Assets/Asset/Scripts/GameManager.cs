using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Vector3Int currentPlayerCell { get;  private set; }
    private bool OnPlayerDeath;

    //Events

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayerDied()
    {
        OnPlayerDeath = true;
        Debug.Log("Player Died");
    }
    public void PlayerPosition(Vector3Int position)
    {
        currentPlayerCell = position;
    }


}
