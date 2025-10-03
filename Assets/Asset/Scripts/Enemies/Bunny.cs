using UnityEngine;

public class Bunny : MonoBehaviour, IDamagable
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Vector3Int currentEnemyCell;
    [SerializeField] private Vector2Int moveDirection = Vector2Int.left;
    [SerializeField] private Vector3 targetCell;

    //Animation Stuff
    public Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.instance.AddEntity(this);
        currentEnemyCell = TilemapPlacement.instance.WorldToCell(transform.position);
        targetCell = TilemapPlacement.instance.CellWorldCenter(currentEnemyCell);
        transform.position = targetCell;
        animator.SetBool("IsWalking", true);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetCell, Time.deltaTime * speed);

        if (Vector3.Distance(transform.position, targetCell) < 0.1f)
        {
            transform.position = targetCell;
            currentEnemyCell = TilemapPlacement.instance.WorldToCell(transform.position);

            var nextCell = currentEnemyCell + new Vector3Int(moveDirection.x, moveDirection.y, 0);

            if (TilemapPlacement.instance.CanMoveToCell(nextCell))
            {
                var cell = currentEnemyCell + new Vector3Int(moveDirection.x, moveDirection.y, 0);
                targetCell = TilemapPlacement.instance.CellWorldCenter(cell);
            }
            else
            {
                animator.SetBool("IsWalking", false);
                animator.SetTrigger("IsBoxing");
            }

        }
    }

    public Vector3Int GetCurrentCell()
    {
        return currentEnemyCell;
    }

    public void TakeDamage()
    {
        Debug.Log("Bunny Took Damage");

        // DeathAnimation
        GameManager.instance.RemoveEntity(this);
        Destroy(gameObject);
    }

    public void ChangeMovementDirection()
    {
        moveDirection *= -1;
        GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
        animator.SetBool("IsWalking", true);
    }
}
