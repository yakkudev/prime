using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BeltManager : MonoBehaviour {
    public static BeltManager i;
    List<Belt> belts = new();


    public Item testItem;

    void Awake() {
        if (i != null) {
            Debug.LogWarning("More than one instance of BeltManager found!");
            Destroy(this);
            return;
        }

        i = this;

        StartCoroutine(UpdateBeltItems());
    }


    IEnumerator UpdateBeltItems() {
        while (true) {
            List<Item> items = new List<Item>();
            foreach (Belt b in belts) {
                items.Add(b.bbelt.item);
            }

            // clear belts
            foreach (Belt b in belts) {
                b.bbelt.nextContainer = null;
            }

            // push items by one
            for (int i = 0; i < items.Count; i++) {
                Belt b = belts[i];
                if (i == items.Count - 1) {
                    b.bbelt.nextContainer = null;
                } else {
                    b.bbelt.nextContainer = belts[i + 1].bbelt;
                }
                b.bbelt.item = items[i];
            }


            yield return new WaitForSeconds(1f);
        }
    }


    public void CreateBelt(Vector3Int position) {
        print("Tryign to create belt");
        if (GetBeltAt(position) != null)  { 
            Debug.LogWarning("Belt already exists at " + position);
            return;
        }

        Belt b = new Belt(Instantiate(BuildSystem.i.buildings[1], position, Quaternion.identity).GetComponent<BBelt>());
        b.position = position;
        belts.Add(b);

        Debug.Log("Created belt at " + position);

        // Add to tilemap
        BuildSystem.i.UpdateBelt(b);
    }

    public Belt GetBeltAt(Vector3Int position) {
        foreach (Belt b in belts) {
            if (b.position == position) {
                return b;
            }
        }

        return null;
    }

    public void AddBelt(Belt b) {
        belts.Add(b);
    }

    public void RemoveBelt(Belt b) {
        belts.Remove(b);
    }
}