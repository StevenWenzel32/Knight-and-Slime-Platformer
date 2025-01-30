using System;
using UnityEngine;

// should really make a parent class for the switches and button classes at some point **
public class Toggler : MonoBehaviour
{
    [Header ("Object(s) to Control")]
    // might need to make another class to be the controlled objects if this needs to be any more robust
    // moving controlsd currently only apply to this 
    // set to the object to be controlled -- can be a door, bridge, elavator, fan, etc.
    public GameObject controlledOne;
    // if it should continue to move back and forth on its own
    public bool LoopModeOne;
    // second object to control/ event to trigger -- currently can't move two objects
    public GameObject controlledTwo;

    [Header ("If Moving")]
    //should this move
    public bool move;
    // how far to move
    public float distance; 
    public float moveSpeed = 1f;

    // could add in a speed option later for both distance and rotation

    [Header ("Direction to Move (only one)")]
    public bool left;
    public bool right;
    public bool up;
    public bool down;

    [Header ("If Rotating")]
    // should this rotate
    public bool rotate;
    // how much to rotate
    public float rotation;
    public float rotationSpeed;

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
        ControlObject(controlledOne);
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
        MovingPlatform platform = controlled?.GetComponent<MovingPlatform>();
        Gate gate = controlled?.GetComponent<Gate>();
        Removable removable = controlled?.GetComponent<Removable>();
        Switch otherSwitch = controlled?.GetComponent<Switch>();

        // if the object is a moving platform
        if (platform != null){
            if (move){
                Move(platform);
            } else if (rotate){
                Rotate(platform);
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

    // moves the controlled platform based on inputs
    public void Move(MovingPlatform platform){
        // add in enums for the directions *********
        if (left){
            Debug.Log("toggling platform, left");
            platform.TogglePlatform("left", distance, moveSpeed, LoopModeOne);
        } else if (right){
            Debug.Log("toggling platform, right");
            platform.TogglePlatform("right", distance, moveSpeed, LoopModeOne);
        }
        // check if moving up or down
        if (up){
            Debug.Log("toggling platform, up");
            platform.TogglePlatform("up", distance, moveSpeed, LoopModeOne);
        } else if (down){
            Debug.Log("toggling platform, down");
            platform.TogglePlatform("down", distance, moveSpeed, LoopModeOne);
        }
    }

    // rotates the controlled platform based on inputs -- finish later
    public void Rotate(MovingPlatform platform){
        if (rotateLeft){
            Debug.Log("rotating platform, left");
//            platform.TogglePlatform("left", distance, moveSpeed, auto);
        } else if (rotateRight){
            Debug.Log("rotating platform, right");
//            platform.TogglePlatform("right", distance, moveSpeed, auto);
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
