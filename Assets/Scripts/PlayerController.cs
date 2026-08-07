using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public PlayerStats stats;
    public Vector3Int pos;

    [Header("Components")]
    [SerializeField] private PathFollower pathFollow;

    public void Move(List<Vector3Int> path, Vector3Int target) {
        pathFollow.SetNewPath(path);
        pos = target;
    }
}

[System.Serializable]
public class PlayerStats {
    public int mvRange = 3;
    public int atk = 1;
    public int hp = 3;

}