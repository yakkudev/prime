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

    public Sprite front;
    public Sprite back;
    public Sprite side;

    public SpriteRenderer spriteRenderer;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


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

        // If the player is moving up
        if (yAxis > 0)
        {
            spriteRenderer.sprite = back;
        }
        // If the player is moving down
        else if (yAxis < 0)
        {
            spriteRenderer.sprite = front;
        }
        // If the player is moving left
        else if (xAxis < 0)
        {
            spriteRenderer.sprite = side;
            spriteRenderer.flipX = true;
        }
        // If the player is moving right
        else if (xAxis > 0)
        {
            spriteRenderer.sprite = side;
            spriteRenderer.flipX = false;
        }

    }
}
