using UnityEngine;

public class Switch : MonoBehaviour
{
    private Animator anim;
    // set to the object to be controlled -- can be a door, bridge, elavator, fan, etc.
    public GameObject controlled;
    private bool isFlipped = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // check if the tigger collider was hit 
    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        // Check for collisions with player 
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && !isFlipped)
        {
            FlipSwitch();
        }
    }

    private void FlipSwitch(){
        anim.SetBool("Flip", true); 
        isFlipped = true;
        Debug.Log("Switch has been flipped by player!");
        Doors door = controlled?.GetComponent<Doors>();
        door?.ToggleDoor();
    }
}
