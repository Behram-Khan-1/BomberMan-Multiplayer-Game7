using Unity.Netcode;
using UnityEngine;

public class TestPlayerMovement : NetworkBehaviour
{
    public float movementSpeed = 5f;
    private Vector2 currentInput = Vector2.zero;
    private Vector2 lastSentInput = Vector2.zero;

    void Update()
    {
        if (!IsOwner) return;

        // 1. Gather Input
        Vector2 newInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        // 2. Send only when input changes
        if (newInput != lastSentInput)
        {
            lastSentInput = newInput;
            SubmitInputServerRpc(newInput);
        }
    }

    [ServerRpc]
    void SubmitInputServerRpc(Vector2 input)
    {
        currentInput = input; // Store this player's current direction
    }

    void FixedUpdate()
    {
        // 3. Only the server moves players (authoritative)
        if (IsServer && currentInput != Vector2.zero)
        {
            transform.Translate(currentInput * movementSpeed * Time.fixedDeltaTime);
        }
    }
}
