using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildSystem : MonoBehaviour
{
    
    public GameObject[] buildings;

    public Tilemap tilemap;

    public GameObject highlight;

    public TileBase farmlandTile;

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
        Vector3Int v = tilemap.WorldToCell(mouse);

        gameCursorPos = v;
        // offset the highlight
        highlight.transform.position = gameCursorPos + new Vector3(0.5f, 0.5f, 0f);

    }

    void Update() {
        if (currentMode == BuildMode.NONE) return;

        MoveHighlight();

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

    private void BuildSeed()
    {
        throw new NotImplementedException();
    }

    private void BuildFarm() {
        tilemap.SetTile(gameCursorPos, TileManager.i.farmTile);
    }

    private void BuildMachines()
    {
        throw new NotImplementedException();
    }
}
