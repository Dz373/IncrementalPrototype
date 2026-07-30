using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class SkillNode : MonoBehaviour
{
    public int currentLevel;
    public SkillSO skillSO;

    private bool isLocked;
    public List<SkillNode> nodeUnlock;
    public List<SkillNode> nodeRequired;

    [Header("Node References")]
    [SerializeField] private TextMeshProUGUI skillLevelText;
    [SerializeField] private Image skillIcon;
    [SerializeField] private Button skillButton;
    private SkillTreeManager skillManager;

    private void Start() {
        skillManager = FindFirstObjectByType<SkillTreeManager>();

        foreach (SkillNode skl in nodeUnlock) {
            if (nodeRequired.Contains(skl)) {
                Debug.Log(skl + " already contains " + this);
                return;
            }

            skl.nodeRequired.Add(this);
            skl.UnlockNode();
            LinkNode(skl.gameObject);
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
        if(!isLocked && currentLevel < skillSO.skillMaxLevel && skillManager.CanSpendSkillPoints(skillSO.skillCost)) {
            currentLevel++;
            UpdateUI();

            skillManager.UpdateStat(skillSO);

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

    private void LinkNode(GameObject node) {
        GameObject link = Instantiate(skillManager.nodeLink, transform);

        Vector2 direction = (Vector2)node.transform.position - (Vector2)transform.position;
        link.transform.right = direction;

        RectTransform rectTransform = link.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(direction.magnitude, rectTransform.sizeDelta.y);
    }
}
