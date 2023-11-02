using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CropManager : MonoBehaviour {
    public static CropManager i;
    List<Crop> crops = new();
    void Awake() {
        if (i != null) {
            Debug.LogWarning("More than one instance of CropManager found!");
            Destroy(this);
            return;
        }

        i = this;

        StartCoroutine(Tick());
    }

    IEnumerator Tick() {
        while (true) {
            yield return new WaitForSeconds(1f);
            DryAll();
            GrowAll();
        }
    }


    private void Update() {
        if (Input.GetKey(KeyCode.L)) {
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

    public List<Crop> GetCropsInRange(Vector3Int position, float radius) {
        List<Crop> cropsInRange = new();
        foreach (Crop crop in crops) {
            if (Vector3.Distance(position, crop.position) <= radius) {
                cropsInRange.Add(crop);
            }
        }

        return cropsInRange;
    }

    public void AddCrop(Crop crop) {
        crops.Add(crop);
    }

    public void RemoveCrop(Crop crop) {
        crops.Remove(crop);
    }

    public void DryAll() {
        foreach (Crop crop in crops) {
            crop.Dry();
        }
    }

    public void GrowAll() {
        foreach (Crop crop in crops) {
            crop.Grow();
        }
    }
}