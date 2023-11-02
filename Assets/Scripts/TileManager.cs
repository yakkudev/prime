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

    public Tile carrotTile0;
    public Tile carrotTile1;
    public Tile carrotTile2;
    public Tile carrotTile3;


}
