using UnityEngine;

public class GameButton : MonoBehaviour
{
    [Header ("Object to Control")]
    // set to the object to be controlled -- can be a door, bridge, elavator, fan, etc.
    public GameObject controlled;

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
    private bool isPushed = false;

    // Start is called before the first frame update
    void Start()
    {   if (anim != null){
            anim = GetComponent<Animator>();
        }   
    }

    // check if the trigger collider was hit 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check for collisions with player 
        if (collision.gameObject.layer == LayerMask.NameToLayer("Knight") || collision.gameObject.layer == LayerMask.NameToLayer("Slime"))
        {
            ToggleButton();   
            Debug.Log("Button has been pushed by player!");
        }
    }

    // check if the trigger collider is no longer being hit
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check for collisions with player 
        if (collision.gameObject.layer == LayerMask.NameToLayer("Knight") || collision.gameObject.layer == LayerMask.NameToLayer("Slime"))
        {
            ToggleButton();
            Debug.Log("Player has left button");
        }
    }

    private void ToggleButton(){
        isPushed = !isPushed;
        // no animations right now 
        if (anim != null){
            anim.SetBool("Push", isPushed);
        }
        ControlObject();
    }

    // control the object based on its type -- if extra checks are needed before calling the toggle<Object>() put into another function later
    private void ControlObject(){
        // these object creations also check the type of the object 
        // to check if the object is something else just add another assignment for that object
        MovingPlatform bridge = controlled?.GetComponent<MovingPlatform>();
        Doors door = controlled?.GetComponent<Doors>();

        // if the object is a moving platform
        if (bridge != null){
            // will later add in other functions to Move() and Rotate() with all the options
            if (left){
                Debug.Log("toggling platform, left");
                bridge.TogglePlatform("left", distance, moveSpeed);
            }
        } 
        // if the object is a door
        else if (door != null){
            door.ToggleDoor();
        }
    }
}
