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

    private void Awake() {
        savePath = Path.Combine(Application.persistentDataPath, "savefile.json");
    }

    private void Start() {
        NewGame();
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
        switch (skill.skillModifier) {
            case Modifier.timeUp:
                data.actions++;
                break;
            
            default:
                break;
        }

        UpdateSkillPoints(-skill.skillCost);
    }

    private void SaveGame() {
        string json = JsonUtility.ToJson(data, true);

        File.WriteAllText(savePath, json);
    }

    private void LoadGame() {
        if (File.Exists(savePath)) {
            string json = File.ReadAllText(savePath);
            data = JsonUtility.FromJson<GameData>(json);

            Debug.Log("Loaded existing GameData");
        }
        else
            NewGame();
    }

    private void NewGame() {
        data = new GameData();
        data.pStats = new PlayerStats();

        Debug.Log("No file path: new GameData");
    }

    public void PlayGame() {
        SaveGame();
        SceneManager.LoadScene(0);
    }
}
