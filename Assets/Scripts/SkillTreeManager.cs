using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class SkillTreeManager : MonoBehaviour
{
    public int totalSkillPoints;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI skillPointText;
    public GameObject nodeLink;
    private PlayerController player;
    private GameManager gameManager;

    private void Start() {
        player = FindFirstObjectByType<PlayerController>();
        gameManager = FindFirstObjectByType<GameManager>();
    }

    public bool CanSpendSkillPoints(int cost) {
        if (totalSkillPoints < cost)
            return false;

        return true;
    }

    public void UpdateSkillPoints(int cost) {
        totalSkillPoints -= cost;
        skillPointText.text = totalSkillPoints.ToString();
    }

    public void UpdateStat(SkillSO skill) {
        switch (skill.skillModifier) {
            case Modifier.attackUp:
                player.atk += skill.modifierAmount;
                break;

            case Modifier.healthUp:
                player.hp += skill.modifierAmount;
                break;

            case Modifier.movementUp:
                player.mvSpd += skill.modifierAmount;
                break;

            case Modifier.timeUp:
                gameManager.time += skill.modifierAmount;
                break;

            default:
                break;
        }

        UpdateSkillPoints(skill.skillCost);
    }
}
