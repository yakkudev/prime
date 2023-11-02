using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class Crop {
    public Vector3Int position;
    public int growthStage = 0;
    public int growthTime = 0;
    public int growthTimeMax = 0;
    public int waterLevel = 0;
    public int waterLevelMax = 0;

    public TileBase[] tiles = new TileBase[4];

    public abstract void Start();

    // Growth stages
    public static int[] GrowthTimes = {
        0,  // level 0
        10, // level 1
        30, // level 2
        60, // level 3
    };

    public abstract void Grow();
    public abstract void Water();
    public abstract void TryWater();
    public abstract void Dry();
    public abstract void Harvest();
    public abstract void Destroy();
}