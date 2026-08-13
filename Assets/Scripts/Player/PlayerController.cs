using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public PlayerStats stats;
    public Vector3Int pos;

    [Header("Components")]
    [SerializeField] private PathFollower pathFollow;
    [SerializeField] private GameManager gm;

    public void Move(List<Vector3Int> path, Vector3Int target) {
        gm.phase = ActionPhase.Moving;
        pathFollow.SetNewPath(path);
        pos = target;
    }

    public void InstantMove(Vector3Int target) {
        pos = target;
        transform.position = target;
    }
}

[System.Serializable]
public class PlayerStats {
    public int mvRange = 3;
    public int atk = 1;
    public int hp = 3;

    public int minAtkRange = 1;
    public int maxAtkRange = 1;
}