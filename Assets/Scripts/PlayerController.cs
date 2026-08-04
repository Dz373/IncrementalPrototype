using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    public int mvSpd;
    public int atk;
    public int hp;

    [Header("Script Variables")]
    public bool canMove = true;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;

    private Vector2 playerInput;

    private void Update() {
        if (canMove) {
            playerInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        }
        else {
            playerInput = Vector2.zero;
        }
    }

    private void FixedUpdate() {
        rb.linearVelocity = playerInput * mvSpd;
    }
}
