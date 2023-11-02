using System;
using System.Linq;
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

    public Camera cam;

    public Vector3Int gameCursorPos;
    public BuildMode currentMode = BuildMode.NONE;

    public int rotation;


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
            SetMode(BuildMode.BUILD_BELT);
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

        if (Input.GetKeyDown(KeyCode.R)) {
            rotation = (rotation + 1) % 4;
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
            case BuildMode.BUILD_BELT:
                BuildBelt(0);
                break; 
        }
    }

    private void RotateTile(Vector3Int pos, int rot, Tilemap tilemap) {
        Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, 90f*rot), Vector3.one);
        tilemap.SetTransformMatrix(new Vector3Int(0, 0, 0), matrix);
    }

    private void BuildSeed() {
        groundTilemap.GetTile(gameCursorPos);
        if (TileManager.i.farmTiles.Contains(groundTilemap.GetTile(gameCursorPos))) {
            CropManager.i.CreateCrop<Carrot>(gameCursorPos);
        }
    }

    public void UpdateCrop(Crop crop) {
        // Change growth stage
        overlayTilemap.SetTile(crop.position, crop.tiles[crop.growthStage]);

        // Change water level
        if (crop.waterLevel > 20) {
            groundTilemap.SetTile(crop.position, TileManager.i.farmTiles[1]);
        } else {
            groundTilemap.SetTile(crop.position, TileManager.i.farmTiles[0]);
        } /* zabije sie */
    }

    private void BuildFarm() {
        // Check if tile is contained in grass tiles
        if (!TileManager.i.grassTiles.Contains(groundTilemap.GetTile(gameCursorPos))) return;
        groundTilemap.SetTile(gameCursorPos, TileManager.i.farmTiles[0]);
    }

    private void BuildMachines() {
        // Check if tile is empty
        if (!TileManager.i.grassTiles.Contains(groundTilemap.GetTile(gameCursorPos))) {
            Debug.LogWarning("Machines can only be built on grass tiles");
            return;
        }

        // Get building index
        int index = 0;

        // Get building
        GameObject building = buildings[index];

        // Instantiate building
        var s = Instantiate(building, gameCursorPos, Quaternion.identity);

    }

    public void UpdateBelt (Belt belt) {
        // Change growth stage
        buildingTilemap.SetTile(belt.position, belt.tiles[0]);
    }

    private void BuildBelt(int rotation) {
        // Check if tile is empty
        if (!TileManager.i.grassTiles.Contains(groundTilemap.GetTile(gameCursorPos))) {
            Debug.LogWarning("Machines can only be built on grass tiles");
            return;
        }

        // Get building index
        int index = 1;

        // Get building
        GameObject building = buildings[index];

        // Instantiate building
        var s = Instantiate(building, gameCursorPos, Quaternion.Euler(0f, 0f, 90f*rotation));

        // Set tile underneath to belt tile
        buildingTilemap.SetTile(gameCursorPos, TileManager.i.beltTiles[0]);
        
        // Rotate tile
        RotateTile(gameCursorPos, rotation, buildingTilemap);
    }

}
