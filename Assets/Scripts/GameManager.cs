using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    [Header("Game Variables")]
    public int actions;
    public ActionPhase phase;
    public GameData data;
    private Vector3Int playerCurPos;
    
    [Header("Managers")]
    public PlayerController player;
    public CursorController cursor;
    public TilemapManager map;

    [Header("Misc Objects")]
    [SerializeField] private TextMeshProUGUI actionCounter;
    [SerializeField] private GameObject endMenu;

    private string savePath;
    private void Awake() {
        savePath = Path.Combine(Application.persistentDataPath, "savefile.json");

        //LoadGame();
        data = NewGame();
        player.stats = data.pStats;
    }

    private void Start() {
        actionCounter.text = actions.ToString();

        map.SetTilesInRange();
        map.DisplayOverlay();
        map.DisplayAtkOverlay();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.D))
            Debug.Log("Tile Cost: " + map.GetMoveCost(cursor.pos));

        if (Input.GetKeyDown(KeyCode.S)) {
            data = NewGame();
            SaveGame();
            SceneManager.LoadScene(0);
        }

        if (Input.GetMouseButtonDown(0)) {
            Vector3Int target = cursor.pos;

            switch (phase) {
                case ActionPhase.SelectMoveTile:
                    if (!target.Equals(player.pos) && map.CanSelectTile(target, phase)) {
                        map.ClearTiles();
                        playerCurPos = player.pos;
                        player.Move(map.FindMovePath(target, player.pos), target);
                    }
                    break;

                case ActionPhase.SelectAttackTile:
                    if (map.CanSelectTile(target, phase)) {
                        map.SetTilesInRange();
                        map.DisplayOverlay();
                        map.DisplayAtkOverlay();
                        
                        UpdateActions();
                    }
                    break;
            }
        }

        if (Input.GetMouseButtonDown(1)) {
            switch (phase) {
                case ActionPhase.SelectAttackTile:
                    player.InstantMove(playerCurPos);
                    map.DisplayOverlay();
                    map.DisplayAtkOverlay();
                    phase = ActionPhase.SelectMoveTile;
                    break;
            }
        }
    }

    public void FinishMoving() {
        phase = ActionPhase.SelectAttackTile;

        map.DisplayAtkRangeOfTile(player.pos);
    }

    public void EndGame() {
        endMenu.SetActive(true);
        actionCounter.gameObject.SetActive(false);
    }

    private void UpdateActions() {
        actions--;
        actionCounter.text = actions.ToString();
        phase = ActionPhase.SelectMoveTile;

        if (actions <= 0) {
            EndGame();
        }
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
            data = NewGame();

        actions = data.actions;
        player.stats = data.pStats;
    }

    static public GameData NewGame() {
        GameData gameData = new GameData();
        gameData.pStats = new PlayerStats();
        gameData.skillNodeLevels = new List<int>();

        return gameData;
    }

    public void GoToSkillTree() {
        SaveGame();
        SceneManager.LoadScene(1);
    }
}

[System.Serializable]
public class GameData {
    public int actions = 5;
    public int totalSkillPoints = 10;

    public PlayerStats pStats;

    public List<int> skillNodeLevels;
}

public enum ActionPhase {
    SelectMoveTile,
    Moving,
    SelectAttackTile,
    EndTurn
}