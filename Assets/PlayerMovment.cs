using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    public float speed = 5f;
    float xAxis;
    float yAxis;
    Vector3 movement;
    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        xAxis = Input.GetAxis("Horizontal");
        yAxis = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.LeftShift) && yAxis == 1)
        {
            xAxis *= 0.5f;
            yAxis *= 1.5f;
        }
        movement = (xAxis * transform.right) + (yAxis * transform.up);
        rb.velocity = movement * speed;
    }
}
