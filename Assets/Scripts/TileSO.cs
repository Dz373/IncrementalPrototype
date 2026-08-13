using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileSO", menuName = "Scriptable Objects/TileSO")]
public class TileSO : ScriptableObject {
    public TileBase[] tiles;

    public int mvCost;
    public bool noPass;
    public bool canInteract;
}
