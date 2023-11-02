using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{

    public static TileManager i;

    private void Awake() {
        if (i != null) {
            Debug.LogWarning("More than one instance of TileManager found!");
            Destroy(this);
            return;
        }

        i = this;
    }

    public TileBase farmTile;
    public TileBase seedTile;


}
