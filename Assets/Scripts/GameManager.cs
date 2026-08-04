using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float time;
    public float currentTime;

    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private CanvasGroup UIMenu;

    private void Start() {
        currentTime = time;
    }

    private void Update() {
        currentTime -= Time.deltaTime;
        DisplayTime();

        if (currentTime < 0) {
            currentTime = time;
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if(UIMenu.alpha == 0)
                UIMenu.alpha = 1;
            else
                UIMenu.alpha = 0;
        }
    }

    private void DisplayTime() {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
