using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    public int mvSpd;
    public int atk;
    public int hp;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;

    private Vector2 playerInput;

    private void Update() {
        playerInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        rb.linearVelocity = playerInput * mvSpd;
    }
}
