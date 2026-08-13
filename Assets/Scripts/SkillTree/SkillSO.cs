using UnityEngine;

[CreateAssetMenu(fileName = "SkillSO", menuName = "Scriptable Objects/SkillSO")]
public class SkillSO : ScriptableObject
{
    [Header("Skill Info")]
    public string skillName;
    public int skillMaxLevel;
    public int skillCost;

    public Modifier skillModifier;
    public int modifierAmount;

    public Sprite skillIcon;
}

public enum Modifier { 
    healthUp,
    attackUp,
    movementUp,
    timeUp
}