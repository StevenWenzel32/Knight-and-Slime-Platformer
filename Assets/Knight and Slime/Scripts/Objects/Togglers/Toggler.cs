using System;
using UnityEngine;

// should really make a parent class for the switches and button classes at some point **
public class Toggler : MonoBehaviour
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

    // protected vars
    protected int backgroundLayer;
    protected int objectLayer;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        backgroundLayer = LayerMask.NameToLayer("Background");
        objectLayer = LayerMask.NameToLayer("Object");
    }

    // Activate the toggler
    protected virtual void Activate(){
        // perform the actual action on the objects
        ControlObject(controlled);
        // check if a second object was given
        if (controlledTwo != null){
            Debug.Log("controlling second object");
            ControlObject(controlledTwo);
        }
    }

    // control the object based on its type -- if extra checks are needed before calling the toggle<Object>() put into another function later
    protected virtual void ControlObject(GameObject controlled){
        // these object creations also check the type of the object 
        // to check if the object is something else just add another assignment for that object
        // create make a seperate assignObject() to do the checks and assigns and then passes the object var to other funcs ************
        MovingPlatform bridge = controlled?.GetComponent<MovingPlatform>();
        Gate gate = controlled?.GetComponent<Gate>();
        Removable removable = controlled?.GetComponent<Removable>();
        Switch otherSwitch = controlled?.GetComponent<Switch>();

        // if the object is a moving platform
        if (bridge != null){
            // will later add in other functions to Move() and Rotate() with all the options
            // add in enums for the directions *********
            if (left){
                Debug.Log("toggling platform, left");
                bridge.TogglePlatform("left", distance, moveSpeed);
            } else if (right){
                Debug.Log("toggling platform, right");
                bridge.TogglePlatform("right", distance, moveSpeed);
            }
            // check if moving up or down
            if (up){
                Debug.Log("toggling platform, up");
                bridge.TogglePlatform("up", distance, moveSpeed);
            } else if (down){
                Debug.Log("toggling platform, down");
                bridge.TogglePlatform("down", distance, moveSpeed);
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
        // if the object is a switch
        else if (otherSwitch != null){
            otherSwitch.ToggleLayer();
        }
    }

    // change the layer of the object between background and object
    // might want to try and move this to a generic object class at some point ****
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
