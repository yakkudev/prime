using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprinkler : MonoBehaviour {
    public Vector3Int position;

    public float waterLevel = 0f;
    public float waterLevelMax = 100f;

    public bool isInfinite = false;

    public float range = 3f;
    public List<Crop> cropsInRange;

    public ParticleSystem waterParticles;

    private void Start() {
        position = new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
        transform.position = position + new Vector3(0.5f, 0.5f, 0f);
        StartCoroutine(Tick());
    }

    void UpdateRange() {
        cropsInRange = CropManager.i.GetCropsInRange(position, range);
    }

    IEnumerator Tick() {
        while (true) {
            yield return new WaitForSeconds(1f);
            // make sure water level isn't negative or zero
            if (waterLevel <= 0) {
                waterLevel = 0;
                continue;
            }

            // water crops
            UpdateRange();
            foreach (var c in cropsInRange) {
                c.TryWater();
            }
            waterLevel--;

            waterParticles.Play();
        }
    }
}