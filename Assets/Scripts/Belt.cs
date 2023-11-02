using UnityEngine;
using UnityEngine.Tilemaps;

public class Belt {
    public Vector3Int position;

    public BBelt bbelt;

    public AnimatedTile[] tiles = new[] {
        TileManager.i.beltTiles[0],
        TileManager.i.beltTiles[1],    
    };

    public Belt(BBelt bbelt) {
        this.bbelt = bbelt;
    }
}