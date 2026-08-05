using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    public int mvRange;
    public int atk;
    public int hp;
    public Vector3Int pos;

    [Header("Components")]
    [SerializeField] private PathFollower pathFollow;

    public void Move(List<Vector3Int> path, Vector3Int target) {
        pathFollow.SetNewPath(path);
        pos = target;
    }
}
