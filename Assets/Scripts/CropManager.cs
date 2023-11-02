using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CropManager : MonoBehaviour {
    public static CropManager i;
    void Awake() {
        if (i != null) {
            Debug.LogWarning("More than one instance of CropManager found!");
            Destroy(this);
            return;
        }

        i = this;
    }    

    List<Crop> crops = new();

    private void Update() {
        if (Input.GetKeyDown(KeyCode.G)) {
            GrowAll();
            print(crops.Count);
        }
    }

    public void CreateCrop<CropType>(Vector3Int position) where CropType : Crop, new() {
        print("Tryign to create crop");
        if (GetCropAt(position) != null)  { 
            Debug.LogWarning("Crop already exists at " + position);
            return;
        }

        CropType crop = new();
        crop.position = position;
        crop.Start();
        crops.Add(crop);

        Debug.Log("Created crop at " + position);

        // Add to tilemap
        BuildSystem.i.UpdateCrop(crop);
    }

    public Crop GetCropAt(Vector3Int position) {
        foreach (Crop crop in crops) {
            if (crop.position == position) {
                return crop;
            }
        }

        return null;
    }

    public void AddCrop(Crop crop) {
        crops.Add(crop);
    }

    public void RemoveCrop(Crop crop) {
        crops.Remove(crop);
    }

    public void GrowAll() {
        foreach (Crop crop in crops) {
            crop.Grow();
        }
    }
}