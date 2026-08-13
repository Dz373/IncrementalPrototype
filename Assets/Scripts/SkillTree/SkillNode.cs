using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class SkillNode : MonoBehaviour
{
    public int currentLevel;
    public bool isLocked;

    public SkillSO skillSO;
    public List<SkillNode> nodeUnlock;
    public List<SkillNode> nodeRequired;
    private Dictionary<SkillNode, SkillNodeLink> nodeLinks = new Dictionary<SkillNode, SkillNodeLink>();

    public int id;

    [Header("Node References")]
    [SerializeField] private TextMeshProUGUI skillLevelText;
    [SerializeField] private Image skillIcon;
    [SerializeField] private Button skillButton;
    private SkillTreeManager skillManager;

    private void Awake() {
        skillManager = FindFirstObjectByType<SkillTreeManager>();

        skillButton.onClick.AddListener(() => skillManager.UpdateStat(skillSO));

        foreach (SkillNode skl in nodeUnlock) {
            if (nodeRequired.Contains(skl)) {
                Debug.Log(skl + " already contains " + this);
                return;
            }

            skl.nodeRequired.Add(this);
            skl.UnlockNode();

            GameObject link = Instantiate(skillManager.nodeLink, transform);
            SkillNodeLink script = link.GetComponent<SkillNodeLink>();
            script.Instantiate(skl.gameObject, gameObject);
            nodeLinks.Add(skl, script);
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

            if(IsUnlocked()) {
                UnlockLinkedNodes();
                skillButton.interactable = false;
            }
        }
    }

    public void OnLoadUpgradeSkill(int level) {
        if(level > 0) {
            currentLevel = level;
            UpdateUI();

            if (IsUnlocked()) {
                foreach (SkillNode skl in nodeUnlock) {
                    nodeLinks[skl].OnLoadUnlock();
                    skl.UnlockNode();
                }
                skillButton.interactable = false;
            }
        }
    }

    private void UnlockLinkedNodes() {
        foreach (SkillNode skl in nodeUnlock) {
            StartCoroutine(UnlockNodeTimer(skl));
        }
    }

    private IEnumerator UnlockNodeTimer(SkillNode skl) {
        nodeLinks[skl].Unlock();
        yield return new WaitForSeconds(0.5f);
        skl.UnlockNode();
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
