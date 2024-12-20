using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    // the rigid body
    private Rigidbody2D body;
    // Start is called before the first frame update
    void Start()
    {
        body = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check for collisions with the players 
        if (collision.gameObject.CompareTag("Slime") || collision.gameObject.CompareTag("Knight"))
        {
            Debug.Log("player hit the falling platform");
            // check the gravity scale
            if (body.gravityScale == 0){
                body.gravityScale = 1;
            }
        }
    }
}
