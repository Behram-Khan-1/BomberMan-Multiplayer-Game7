using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerAnimationManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerMovement playerMovement;
    Vector2Int previousDir;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerMovement.OnMove += PlayerMovement_DirectionChange;
    }

    void PlayerMovement_DirectionChange(Vector2Int movementDirection)
    {
        if (movementDirection != Vector2Int.zero)
        {
            previousDir = movementDirection; //Storing prev dir when input is 0
            animator.SetBool("IsWalking", true);
        }

        if(movementDirection == Vector2.zero)
        {
            animator.SetBool("IsWalking", false);
            animator.SetFloat("IdleX", previousDir.x);
            animator.SetFloat("IdleY", previousDir.y);
            return;
        }



        if (movementDirection == Vector2.right)
        {
            transform.GetChild(0).transform.localScale = new Vector3(-1, 1, 1);
            animator.SetFloat("MoveX", -movementDirection.x);
            animator.SetFloat("MoveY", -movementDirection.y);
            return;
        }
        if (movementDirection == Vector2.left)
        {
            transform.GetChild(0).transform.localScale = new Vector3(1, 1, 1);
        }

        animator.SetFloat("MoveX", movementDirection.x);
        animator.SetFloat("MoveY", movementDirection.y);



        // animator.SetFloat("MoveX", movementDirection.x);
        // animator.SetFloat("MoveY", movementDirection.y);

    }


    // Update is called once per frame
    void Update()
    {

    }
}
