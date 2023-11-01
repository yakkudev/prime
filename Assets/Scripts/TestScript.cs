using UnityEngine;

public class TestScript : MonoBehaviour
{

    void Start() {
        Debug.Log("Dziala");
    }

    void Update() {
        transform.position = new Vector3(0, Mathf.Sin(Time.time) , 0);
    }
}
