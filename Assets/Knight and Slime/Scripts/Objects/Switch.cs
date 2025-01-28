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
    private int backgroundLayer;
    private int objectLayer;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        backgroundLayer = LayerMask.NameToLayer("Background");
        objectLayer = LayerMask.NameToLayer("Object");
    }

    // check if the tigger collider was hit 
    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        // Check for collisions with player - add the slime here too ***
        if ((collision.gameObject.layer == LayerMask.NameToLayer("Knight") || collision.gameObject.layer == LayerMask.NameToLayer("Slime"))&& !isFlipped)
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
            Debug.Log("controlling second object");
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
        Switch otherSwitch = controlled?.GetComponent<Switch>();

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
        // if the object is a different switch
        else if (otherSwitch != null){
            otherSwitch.ToggleLayer();
        }
    }

    // change the layer of the object between background and object
    public void ToggleLayer(){
        // grab current layer
        int currentLayer = gameObject.layer;
        // get the objects current color
        Color color = gameObject.GetComponent<SpriteRenderer>().color;

        // check if the current layer is the background
        if (currentLayer == backgroundLayer){
            // set the layer to object 
            gameObject.layer = objectLayer;
            // make the color full
            color.a = 1;
            gameObject.GetComponent<SpriteRenderer>().color = color;
        } else {
            // set the layer to background 
            gameObject.layer = backgroundLayer;
            // make the color more see through
            color.a = 0.3f;
            gameObject.GetComponent<SpriteRenderer>().color = color;
        }
    }
}
