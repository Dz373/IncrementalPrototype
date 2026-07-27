using UnityEngine;

[CreateAssetMenu(fileName = "SkillSO", menuName = "Scriptable Objects/SkillSO")]
public class SkillSO : ScriptableObject
{
    [Header("Skill Info")]
    public string skillName;
    public int skillMaxLevel;
    public int skillCost;
    public Sprite skillIcon;
}
