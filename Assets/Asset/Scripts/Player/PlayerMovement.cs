using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] public Vector2Int movementDirection;
    // [SerializeField] public Vector2Int nextDirection;
    [SerializeField] public Vector3Int currentCell;
    [SerializeField] public Vector3 targetPos;

    //Events for PlayerAnimationManager
    public event Action<Vector2Int> OnMove;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        movementDirection = Vector2Int.right;
        // nextDirection = Vector2Int.right;
    }
    void Start()
    {
        currentCell = TilemapPlacement.instance.WorldToCell(transform.position);
        targetPos = TilemapPlacement.instance.CellWorldCenter(currentCell);
        transform.position = targetPos;
    }

    // Update is called once per frame
    void Update()
    {
        KeyboardInput();
        Movement();
    }

    void Movement()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, movementSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            transform.position = targetPos; //Snap to cell
            currentCell = TilemapPlacement.instance.WorldToCell(transform.position);

            Vector3Int nextCell = TilemapPlacement.instance.WorldToCell(transform.position +
            new Vector3(movementDirection.x, movementDirection.y, 0)); // next position relevant to player 

            if (TilemapPlacement.instance.CanMoveToCell(nextCell))
            {
                var cell = currentCell + new Vector3Int(movementDirection.x, movementDirection.y, 0);
                targetPos = TilemapPlacement.instance.CellWorldCenter(cell);
            }

        }
    }


    void KeyboardInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (horizontal > 0)
        {
            movementDirection = new Vector2Int(1, 0);
        }
        else if (horizontal < 0)
        {
            movementDirection = new Vector2Int(-1, 0);
        }
        else if (vertical > 0)
        {
            movementDirection = new Vector2Int(0, 1);
        }
        else if (vertical < 0)
        {
            movementDirection = new Vector2Int(0, -1);
        }
        else
        {
            movementDirection = Vector2Int.zero;
        }
        DirectionChange(movementDirection);
    }
    void DirectionChange(Vector2Int movementDirection)
    {
        OnMove?.Invoke(movementDirection);
    }

}
