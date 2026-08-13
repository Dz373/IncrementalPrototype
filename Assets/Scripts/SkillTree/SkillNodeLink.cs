using UnityEngine;

public class SkillNodeLink : MonoBehaviour
{
    [SerializeField] private RectTransform highlight;

    private bool unlocked;

    public void Instantiate(GameObject nodeA, GameObject nodeB) {
        Vector2 direction = (Vector2)nodeA.transform.position - (Vector2)nodeB.transform.position;
        transform.right = direction;

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(direction.magnitude, rectTransform.sizeDelta.y);
    }

    private void Update() {
        if (unlocked) {
            highlight.localScale -= new Vector3(1/0.5f * Time.deltaTime, 0, 0);

            if(highlight.localScale.x <= 0) {
                unlocked = false;
            }
        }
    }

    public void Unlock() {
        unlocked = true;
    }

    public void OnLoadUnlock() {
        highlight.localScale.Set(0, 1, 1);
    }
}
