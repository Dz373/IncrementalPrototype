using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int mv_spd;

    [SerializeField] private Rigidbody2D rb;

    private Vector2 playerInput;

    private void Update() {
        playerInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        rb.linearVelocity = playerInput * mv_spd;
    }
}
