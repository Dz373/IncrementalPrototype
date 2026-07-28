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

    public void UpdateStat(SkillSO skill) {
        switch (skill.skillModifier) {
            case Modifier.attack:
                atk += skill.modifierAmount;
                break;

            case Modifier.health:
                hp += skill.modifierAmount;
                break;

            case Modifier.movement:
                mvSpd += skill.modifierAmount;
                break;

            default:
                break;
        }
    }
}
