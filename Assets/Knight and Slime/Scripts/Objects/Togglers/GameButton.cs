using UnityEngine;

public class GameButton : Toggler
{
    // private vars
    private Animator anim;
    private bool isPushed = false;
    // how many things are on the button
    private int contacts = 0;

    // Start is called before the first frame update
    protected override void Start(){
        base.Start();
        if (anim != null){
            anim = GetComponent<Animator>();
        }   
    }

    // check if the trigger collider was hit 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check for collisions with player and toggle if the button is not already pushed 
        if (collision.gameObject.layer == LayerMask.NameToLayer("Knight") || collision.gameObject.layer == LayerMask.NameToLayer("Slime"))
        {
            if (contacts == 0){
                Activate();   
            }
            contacts++;
            Debug.Log("Button has been pushed by player!");
        }
    }

    // check if the trigger collider is no longer being hit
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check for collisions with player 
        if (collision.gameObject.layer == LayerMask.NameToLayer("Knight") || collision.gameObject.layer == LayerMask.NameToLayer("Slime"))
        {
            contacts--;
            // check if all players have left the button
            if (contacts == 0){
                Activate();
                Debug.Log("Player has left button");
            }
        }
    }

    // toggle the button 
    // use button specific animations and vars while calling base activate to run controls
    protected override void Activate(){
        isPushed = !isPushed;
        // no animations right now 
        anim?.SetBool("Push", isPushed);
        base.Activate();
    }
}
