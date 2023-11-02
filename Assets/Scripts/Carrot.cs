using UnityEngine;
using UnityEngine.Tilemaps;

public class Carrot : Crop {

    // Init 
    public override void Start() {
        growthTimeMax = GrowthTimes[growthStage];
        waterLevelMax = 100;
        tiles = new TileBase[] {
            TileManager.i.carrotTile0,
            TileManager.i.carrotTile1,
            TileManager.i.carrotTile2,
            TileManager.i.carrotTile3
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