using UnityEngine;

public class Button : MonoBehaviour
{
    private Animator anim;
    // set to the object to be controlled -- can be a door, bridge, elavator, fan, etc.
    public GameObject controlled;
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
        }
    }

    // check if the trigger collider is no longer being hit
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check for collisions with player 
        if (collision.gameObject.CompareTag("Player"))
        {
            ToggleButton();
        }
    }

    private void ToggleButton(){
        isPushed = !isPushed;
        anim.SetBool("Push", isPushed);
        Debug.Log("Button has been pushed by player!");
        ControlObject();
    }

    private void ControlObject(){
        // can later add in checks for finding what object it is 
        MovingPlatform bridge = controlled?.GetComponent<MovingPlatform>();
        bridge?.ToggleBridge();
    }
}
