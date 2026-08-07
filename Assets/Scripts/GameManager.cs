using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

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

    private string savePath;
    private void Awake() {
        savePath = Path.Combine(Application.persistentDataPath, "savefile.json");
    }

    private void Start() {
        LoadGame();

        actionCounter.text = actions.ToString();

        map.DisplayOverlay(player);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.D)) {
            Debug.Log("Tile Cost: " + map.GetMoveCost(cursor.pos));
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
        
        if (actions <= 0){
            SceneManager.LoadScene(0);
        }
        else {
            map.DisplayOverlay(player);
        }

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
        else {
            data = new GameData();
            data.pStats = new PlayerStats();
            
            Debug.Log("No file path: new GameData");
        }

        actions = data.actions;
        player.stats = data.pStats;
    }
    
}

[System.Serializable]
public class GameData {
    public int actions = 5;

    public PlayerStats pStats;
}