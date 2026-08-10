using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;


public class SkillTreeManager : MonoBehaviour
{
    public int totalSkillPoints;

    private string savePath;
    public GameData data;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI skillPointText;
    public GameObject nodeLink;
    private SkillNode[] nodes;

    private void Awake() {
        savePath = Path.Combine(Application.persistentDataPath, "savefile.json");

        LoadGame();
        //data = GameManager.NewGame();
    }

    private void Start() {
        skillPointText.text = totalSkillPoints.ToString();

        nodes = GetComponentsInChildren<SkillNode>();
        for (int i = 0; i < nodes.Length; i++) {
            nodes[i].id = i;

            if(i < data.skillNodeLevels.Count)
                nodes[i].OnLoadUpgradeSkill(data.skillNodeLevels[i]);
        }
    }

    public bool CanSpendSkillPoints(int cost) {
        if (totalSkillPoints < cost)
            return false;

        return true;
    }

    public void UpdateSkillPoints(int cost) {
        totalSkillPoints += cost;
        skillPointText.text = totalSkillPoints.ToString();
    }

    public void UpdateStat(SkillSO skill) {
        if (!CanSpendSkillPoints(skill.skillCost))
            return;

        switch (skill.skillModifier) {
            case Modifier.timeUp:
                data.actions += skill.modifierAmount;
                break;

            case Modifier.movementUp:
                data.pStats.mvRange += skill.modifierAmount;
                break;
            
            default:
                break;
        }

        UpdateSkillPoints(-skill.skillCost);

        Debug.Log(skill.skillModifier);
    }

    private void SaveGame() {
        if (data.skillNodeLevels.Count == 0) {
            for (int i = 0; i < nodes.Length; i++) {
                data.skillNodeLevels.Add(nodes[i].currentLevel);
            }
        }
        else {
            for (int i = 0; i < nodes.Length; i++) {
                data.skillNodeLevels[i] = nodes[i].currentLevel;
            }
        }

        data.totalSkillPoints = totalSkillPoints;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
    }

    private void LoadGame() {
        if (File.Exists(savePath)) {
            string json = File.ReadAllText(savePath);
            data = JsonUtility.FromJson<GameData>(json);
        }
        else
            data = GameManager.NewGame();

        totalSkillPoints = data.totalSkillPoints;
    }

    public void PlayGame() {
        SaveGame();
        SceneManager.LoadScene(0);
    }
}
