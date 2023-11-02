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
        if (waterLevel <= 0) return;
        growthTime ++;
        for (int i = 0; i < GrowthTimes.Length; i++) {
            if (growthTime >= GrowthTimes[i]) {
                growthStage = i;
            }
        }

        BuildSystem.i.UpdateCrop(this);
    }

    public override void TryWater() {
        // 1/3 chance to water  
        if (Random.Range(0, 3) == 0) {
            Water();
        }
    }

    public override void Water() {
        waterLevel += 6;
        if (waterLevel > waterLevelMax) {
            waterLevel = waterLevelMax;
        }
        BuildSystem.i.UpdateCrop(this);
    }

    public override void Dry() {
        // 1/2 chance to dry
        if (Random.Range(0, 2) == 0) {
            waterLevel -= 1;
        }
    }

    public override void Harvest() {
        throw new System.NotImplementedException();
    }

    public override void Destroy() {
        throw new System.NotImplementedException();
    }
}