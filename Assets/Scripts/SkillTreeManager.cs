using UnityEngine;
using System.Collections.Generic;

public class SkillTreeManager : MonoBehaviour
{
    public int totalSkillPoints;

    private PlayerController player;

    private void Start() {
        player = FindFirstObjectByType<PlayerController>();
    }

    public bool CanSpendSkillPoints(int cost) {
        if (totalSkillPoints < cost)
            return false;

        return true;
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

            default:
                break;
        }
    }
}
