using UnityEngine;

public class Pickup : MonoBehaviour
{
    public int amount;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            FindFirstObjectByType<SkillTreeManager>().UpdateSkillPoints(amount);
            Destroy(gameObject);
        }
    }
}
