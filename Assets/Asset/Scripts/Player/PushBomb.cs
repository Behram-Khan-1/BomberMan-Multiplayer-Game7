using UnityEngine;
using UnityEngine.Tilemaps;

public class PushBomb : MonoBehaviour
{

    private PlayerMovement playerMovement;
    Vector2Int previousDir;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerMovement.OnMove += TryPushBomb;
    }

    // Update is called once per frame
    void Update()
    {

    }



    private void TryPushBomb(Vector2Int dir)
    {
        if (dir != Vector2Int.zero)
        {
            previousDir = dir;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(previousDir.x, previousDir.y, 0) / 2,
             previousDir, 0.5f);

            if (hit.collider != null && hit.collider.CompareTag("Bomb"))
            {
                Debug.Log(hit.collider.name);
                hit.transform.GetComponent<Bomb>().SetIsPushing(true, previousDir);
            }
        }
    }

}