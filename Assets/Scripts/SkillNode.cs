using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SkillNode : MonoBehaviour
{
    public int currentLevel;

    public SkillSO skillSO;

    [Header("Node Referenecs")]
    [SerializeField] private Button skillButton;
    [SerializeField] private TextMeshProUGUI skillLevelText;
    [SerializeField] private Image skillIcon;

    private void Start() {
        skillButton.onClick.AddListener(UpgradeSkill);
    }

    private void OnValidate() {
        if (skillSO != null) {
            UpdateUI();
        }
    }

    private void UpdateUI() {
        skillIcon.sprite = skillSO.skillIcon;

        skillLevelText.text = currentLevel.ToString() + "/" + skillSO.skillMaxLevel.ToString();

    }

    public void UpgradeSkill() {
        if(currentLevel < skillSO.skillMaxLevel) {
            currentLevel++;
            UpdateUI();
        }
    }
}
