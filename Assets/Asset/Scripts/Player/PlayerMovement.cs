using System;
using Unity.Netcode;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] public Vector2Int movementDirection;
    [SerializeField] public NetworkVariable<Vector3Int> currentCell;
    [SerializeField] public NetworkVariable<Vector3> targetPos;

    //Events for PlayerAnimationManager
    public event Action<Vector2Int> OnMove;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    override public void OnNetworkSpawn()
    {
        if (IsServer)
        {
            currentCell.Value = TilemapPlacement.instance.WorldToCell(transform.position);
            targetPos.Value = TilemapPlacement.instance.CellWorldCenter(currentCell.Value);
        }
        transform.position = targetPos.Value;
    }

    // Update is called once per frame
    void Update()
    {
        //First We take Input
        //Determine if we are player or server
        //Move if server
        //Request server to move us if client.
        if (!IsOwner) return;
        KeyboardInput(); 
    }


    void Movement(Vector2Int movementDirection)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos.Value, movementSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos.Value) < 0.1f)
        {
            transform.position = targetPos.Value; //Snap to cell
            currentCell.Value = TilemapPlacement.instance.WorldToCell(transform.position);

            Vector3Int nextCell = TilemapPlacement.instance.WorldToCell(transform.position +
            new Vector3(movementDirection.x, movementDirection.y, 0)); // next position relevant to player 

            if (TilemapPlacement.instance.CanMoveToCell(nextCell))
            {
                var cell = currentCell.Value + new Vector3Int(movementDirection.x, movementDirection.y, 0);
                targetPos.Value = TilemapPlacement.instance.CellWorldCenter(cell);
            }

        }
    }

    [Rpc(SendTo.Server)]
    void MovementServerRPC(Vector2Int newDir)
    {
        Movement(newDir);
    }

    void KeyboardInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        // var newInput = new Vector2Int((int)horizontal, (int)vertical);
        Vector2Int newInput;
        if (horizontal > 0)
        {
            newInput = Vector2Int.right;
        }
        else if (horizontal < 0)
        {
            newInput = Vector2Int.left;
        }
        else if (vertical > 0)
        {
            newInput = Vector2Int.up;
        }
        else if (vertical < 0)
        {
            newInput = Vector2Int.down;
        }
        else
        {
            newInput = Vector2Int.zero;
        }
        
        if (IsServer)
        {
            Movement(newInput);
            // Debug.Log("Server Moving");
        }
        if (!IsServer)
        {
            // Debug.Log("Client Moved");
            MovementServerRPC(newInput);
        }
        DirectionChange(movementDirection);
    }
    void DirectionChange(Vector2Int movementDirection)
    {
        OnMove?.Invoke(movementDirection);
    }

}
