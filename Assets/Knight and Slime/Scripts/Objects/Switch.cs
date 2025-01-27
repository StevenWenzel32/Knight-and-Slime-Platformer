using System;
using UnityEngine;

// should really make a parent class for the switches and button classes at some point **
public class Switch : MonoBehaviour
{
    [Header ("Object(s) to Control")]
    // set to the object to be controlled -- can be a door, bridge, elavator, fan, etc.
    public GameObject controlled;
    // second object to control/ event to trigger
    public GameObject controlledTwo;

    [Header ("If Moving Platform")]
    // how far to move
    public float distance; 
    public float moveSpeed = 1f;
    // how much to rotate
    public float rotation;
    public float rotationSpeed;

    // could add in a speed option later for both distance and rotation

    [Header ("Direction to Move (only one)")]
    public bool left;
    public bool right;
    public bool up;
    public bool down;

    [Header ("Direction to Rotate (only one)")]
    public bool rotateLeft;
    public bool rotateRight;

    // private vars
    private Animator anim;
    private bool isFlipped = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // check if the tigger collider was hit 
    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        // Check for collisions with player - add the slime here too ***
        if (collision.gameObject.layer == LayerMask.NameToLayer("Knight") && !isFlipped)
        {
            FlipSwitch();
        }
    }

    // flip the switch forever
    private void FlipSwitch(){
        anim.SetBool("Flip", true); 
        isFlipped = true;
        Debug.Log("Switch has been flipped by player!");
        // perform the actual action on the objects
        ControlObject(controlled);
        // check if a second object was given
        if (controlledTwo != null){
            ControlObject(controlledTwo);
        }
    }

    // control the object based on its type -- if extra checks are needed before calling the toggle<Object>() put into another function later
    private void ControlObject(GameObject controlled){
        // these object creations also check the type of the object 
        // to check if the object is something else just add another assignment for that object
        MovingPlatform bridge = controlled?.GetComponent<MovingPlatform>();
        Gate gate = controlled?.GetComponent<Gate>();
        Removable removable = controlled?.GetComponent<Removable>();

        // if the object is a moving platform
        if (bridge != null){
            // will later add in other functions to Move() and Rotate() with all the options
            if (left){
                Debug.Log("toggling platform, left");
                bridge.TogglePlatform("left", distance, moveSpeed);
            }
        } 
        // if the object is a gate
        else if (gate != null){
            gate.ToggleGate();
        }
        // if the object is something to be removed
        else if (removable != null){
            removable.ToggleRemovable();
        }
    }
}
