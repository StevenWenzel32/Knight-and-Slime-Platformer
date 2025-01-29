using System;
using UnityEngine;

// should really make a parent class for the switches and button classes at some point **
public class Switch : Toggler
{
    // private vars - unique
    private Animator anim;
    private bool isFlipped = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
    }

    // check if the tigger collider was hit 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check for collisions with player
        if ((collision.gameObject.layer == LayerMask.NameToLayer("Knight") || collision.gameObject.layer == LayerMask.NameToLayer("Slime"))&& !isFlipped)
        {
            Activate();
        }
    }

    // flip the switch forever
    // use switch specifc anims and vars
    protected override void Activate(){
        anim.SetBool("Flip", true); 
        isFlipped = true;
        Debug.Log("Switch has been flipped by player!");
        base.Activate();
    }
}
