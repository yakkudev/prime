using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class BBelt : MonoBehaviour, IContainer
{

    public Vector3Int position;

    public Belt beltTile;


    public float speed;

    public SpriteRenderer itemHolder;

    public IContainer nextContainer;


    public Item item { get; set; }

    void Start() {
        position = new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
        transform.position = position + new Vector3(0.5f, 0.5f, 0f);

        item = null;
        if (Input.GetKey(KeyCode.LeftShift)) {
            item = BeltManager.i.testItem;
        }

        var nextPos = position + new Vector3Int(0, 1, 0);
        nextContainer = (IContainer)BeltManager.i.GetBeltAt(nextPos);

        // StartCoroutine(MoveItem());
    }

    IEnumerator MoveItem() {
        //! THIS DOESN'T EVER RUN
        while (true) {
            if (item != null) {
                itemHolder.sprite = item.icon;
                itemHolder.color = Color.white;
            } else {
                itemHolder.sprite = null;
                itemHolder.color = Color.clear;
            }
            yield return new WaitForSeconds(1f/speed);

            bool valid = item != null && nextContainer != null;

            while (!valid || !nextContainer.PutItem(item)) {
                yield return null;
            }
            item = null;

            yield return null;
        }
    }

    void Update() {
        if (item != null) {
            itemHolder.sprite = item.icon;
            itemHolder.color = Color.white;
        } else {
            itemHolder.sprite = null;
            itemHolder.color = Color.clear;
        }

    }

    public bool PutItem(Item item) {
        if (this.item != null) return false;
        this.item = item;
        return true;
    }

    public Item TakeItem() {
        Item temp = item;
        item = null;
        return temp;
    }
}
