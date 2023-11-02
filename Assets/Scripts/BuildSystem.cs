using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildSystem : MonoBehaviour
{

    public static BuildSystem i;
    void Awake() {
        if (i != null) {
            Debug.LogWarning("More than one instance of BuildSystem found!");
            Destroy(this);
            return;
        }

        i = this;
    }    
    public GameObject[] buildings;

    public Tilemap groundTilemap;
    public Tilemap overlayTilemap;
    public Tilemap buildingTilemap;

    public GameObject highlight;

    Camera cam;

    public Vector3Int gameCursorPos;
    public BuildMode currentMode = BuildMode.NONE;


    public void SetMode(BuildMode mode) {
        currentMode = mode;
    }


    void Start() {
        cam = Camera.main;
        SetMode(BuildMode.BUILD_FARM);
    }

    void MoveHighlight() {
        Vector3 mouse = cam.ScreenToWorldPoint(Input.mousePosition);

        // Get tile position
        Vector3Int v = groundTilemap.WorldToCell(mouse);

        gameCursorPos = v;
        // offset the highlight
        highlight.transform.position = gameCursorPos + new Vector3(0.5f, 0.5f, 0f);

    }

    void Update() {
        if (currentMode == BuildMode.NONE) return;

        MoveHighlight();

        if (Input.GetKeyDown(KeyCode.Escape)) {
            SetMode(BuildMode.NONE);
            return;
        }

        /* to do wyjebania pozniej */

        if (Input.GetKeyDown(KeyCode.Z)) {
            SetMode(BuildMode.BUILD_FARM);
            return;
        }

        if (Input.GetKeyDown(KeyCode.X)) {
            SetMode(BuildMode.BUILD_SEED);
            return;
        }

        if (Input.GetKeyDown(KeyCode.C)) {
            SetMode(BuildMode.BUILD_FERTILIZER);
            return;
        }

        if (Input.GetKeyDown(KeyCode.V)) {
            SetMode(BuildMode.BUILD_WATER);
            return;
        }

        if (Input.GetKeyDown(KeyCode.B)) {
            SetMode(BuildMode.BUILD_MACHINES);
            return;
        }

        /* pieklo */

        if (!Input.GetMouseButtonDown(0)) return;
        
        switch (currentMode) {
            case BuildMode.BUILD_MACHINES:
                BuildMachines();
                break;
            case BuildMode.BUILD_FARM:
                BuildFarm();
                break;
            case BuildMode.BUILD_SEED:
                BuildSeed();
                break;
            case BuildMode.BUILD_FERTILIZER:
                BuildFertilizer();
                break; 
        }

            // // Check if tile is empty
            // if (tile == null) {
            //     // Get building index
            //     int index = Random.Range(0, buildings.Length);

            //     // Get building
            //     GameObject building = buildings[index];

            //     // Instantiate building
            //     Instantiate(building, tilePos, Quaternion.identity);
            // }
        }

    private void BuildFertilizer()
    {
        throw new NotImplementedException();
    }

    private void BuildSeed() {
        groundTilemap.GetTile(gameCursorPos);
        if (groundTilemap.GetTile(gameCursorPos) == TileManager.i.farmTile) {
            CropManager.i.CreateCrop<Carrot>(gameCursorPos);
        }
    }

    public void UpdateCrop(Crop crop) {
        // Add to tilemap
        overlayTilemap.SetTile(crop.position, crop.tiles[crop.growthStage]);
    }

    private void BuildFarm() {
        groundTilemap.SetTile(gameCursorPos, TileManager.i.farmTile);
    }

    private void BuildMachines()
    {
        throw new NotImplementedException();
    }
}
