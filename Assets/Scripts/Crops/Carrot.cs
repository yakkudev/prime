using UnityEngine;
using UnityEngine.Tilemaps;

public class Carrot : Crop {

    // Init 
    public override void Start() {
        growthTimeMax = GrowthTimes[growthStage];
        waterLevelMax = 100;
        tiles = new TileBase[] {
            TileManager.i.carrotTiles[0],
            TileManager.i.carrotTiles[1],
            TileManager.i.carrotTiles[2],
            TileManager.i.carrotTiles[3]
        };
    }

    public override void Grow() {
        growthTime += 10;
        for (int i = 0; i < GrowthTimes.Length; i++) {
            if (growthTime >= GrowthTimes[i]) {
                growthStage = i;
            }
        }

        BuildSystem.i.UpdateCrop(this);
    }

    public override void Water() {
        throw new System.NotImplementedException();
    }

    public override void Harvest() {
        throw new System.NotImplementedException();
    }

    public override void Destroy() {
        throw new System.NotImplementedException();
    }
}