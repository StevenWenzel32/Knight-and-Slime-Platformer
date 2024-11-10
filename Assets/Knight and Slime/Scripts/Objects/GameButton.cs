using UnityEngine;

public class GameButton : MonoBehaviour
{
    [Header ("Platform/Object to Move")]
    // set to the object to be controlled -- can be a door, bridge, elavator, fan, etc.
    public GameObject controlled;
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
        if (collision.gameObject.CompareTag("Player"))
        {
            ToggleButton();   
            Debug.Log("Button has been pushed by player!");
        }
    }

    // check if the trigger collider is no longer being hit
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check for collisions with player 
        if (collision.gameObject.CompareTag("Player"))
        {
            ToggleButton();
            Debug.Log("Player has left button");
        }
    }

    private void ToggleButton(){
        isPushed = !isPushed;
        if (anim != null){
            anim.SetBool("Push", isPushed);
        }
        ControlObject();
    }

    private void ControlObject(){
        // can later add another function to find what type of object it is  
        MovingPlatform bridge = controlled?.GetComponent<MovingPlatform>();
        // will later add in other functions to Move() and Rotate() with all the options
        if (left){
            Debug.Log("toggling platform, left");
            bridge?.TogglePlatform("left", distance, moveSpeed);
        }
    }

    private void ObjectType()
    {
        
    }
}
