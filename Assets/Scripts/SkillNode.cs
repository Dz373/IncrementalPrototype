using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class SkillNode : MonoBehaviour
{
    public int currentLevel;
    public SkillSO skillSO;

    public bool isLocked;
    public List<SkillNode> nodeUnlock;
    public List<SkillNode> nodeRequired;

    [Header("Node Referenecs")]
    [SerializeField] private TextMeshProUGUI skillLevelText;
    [SerializeField] private Image skillIcon;
    [SerializeField] private Button skillButton;

    private void Start() {
        foreach (SkillNode skl in nodeUnlock) {
            skl.nodeRequired.Add(this);
            skl.UnlockNode();
        }
    }

    private void OnValidate() {
        if (skillSO != null) {
            UpdateUI();
        }
    }

    private void UpdateUI() {
        skillIcon.sprite = skillSO.skillIcon;

        if (isLocked) {
            skillLevelText.text = "Locked";
            skillButton.interactable = false;
        }
        else {
            skillLevelText.text = currentLevel.ToString() + "/" + skillSO.skillMaxLevel.ToString();
            skillButton.interactable = true;
        }
    }

    public void UpgradeSkill() {
        if(!isLocked && currentLevel < skillSO.skillMaxLevel) {
            currentLevel++;
            UpdateUI();

            FindFirstObjectByType<PlayerController>().UpdateStat(skillSO);

            if(IsUnlocked()) {
                UnlockLinkedNodes();
                skillButton.interactable = false;
            }
        }
    }

    private void UnlockLinkedNodes() {
        foreach (SkillNode skl in nodeUnlock) {
            skl.UnlockNode();
        }
    }

    private void UnlockNode() {
        if (LinkedNodesUnlocked())
            isLocked = false;
        else
            isLocked = true;
        
        UpdateUI();
    }

    public bool LinkedNodesUnlocked() {
        foreach(SkillNode skl in nodeRequired) {
            if (!skl.IsUnlocked())
                return false;
        }

        return true;
    }

    public bool IsUnlocked() {
        return currentLevel == skillSO.skillMaxLevel;
    }
}
