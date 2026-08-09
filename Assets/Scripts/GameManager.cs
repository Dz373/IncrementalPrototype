using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    [Header("Game Variables")]
    public int actions;
    public GameData data;
    
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

        LoadGame();
        //data = GameManager.NewGame();
    }

    private void Start() {
        actionCounter.text = actions.ToString();
        map.DisplayOverlay(player);
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
            
            if (!target.Equals(player.pos) && map.CanMoveToTile(target)) {
                map.ClearTiles();
                player.Move(map.FindMovePath(target, player.pos), target);
            }
        }
    }

    public void NextMove() {
        UpdateActions(-1);
        
        if (actions <= 0) {
            EndGame();
        }
        else {
            map.DisplayOverlay(player);
        }
    }

    public void EndGame() {
        endMenu.SetActive(true);
        actionCounter.gameObject.SetActive(false);
    }

    private void UpdateActions(int amount) {
        actions += amount;
        actionCounter.text = actions.ToString();
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