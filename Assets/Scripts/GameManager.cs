using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    [Header("Game Variables")]
    public int actions;
    
    [Header("Managers")]
    public PlayerController player;
    public CursorController cursor;
    public TilemapManager map;

    [Header("Misc Objects")]
    [SerializeField] private TextMeshProUGUI actionCounter;

    private void Start() {
        map.DisplayOverlay(player);
        actionCounter.text = actions.ToString();
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
}
