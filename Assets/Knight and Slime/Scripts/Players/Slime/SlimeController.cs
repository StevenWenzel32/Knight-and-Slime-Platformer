using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class SlimeController : PlayerControllerBase
{
    // Public variables -- the options in unity

    // private vars
    // save the current size of the slime colldier
    private Vector2 currentSize;
    BoxCollider2D slimeCollider;

    protected override void Start()
    {
        base.Start();
        // set the slime collider 
        slimeCollider = GetComponent<BoxCollider2D>();
        // get the current size of the slimes collider and save it for later 
        currentSize = slimeCollider.size; 
    }

    protected override void TriggerDisableAnimation()
    {

    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        // Check for collisions with death objects: invisible colliders for map detection, acid, spikes, mobs, 
        if (collision.gameObject.CompareTag("DeathObject"))
        {
            // kill player using method from PlayerBase
            GetComponent<PlayerBase>().KillPlayer(); 
        } 
        // if the object is honey
        if (collision.gameObject.CompareTag("Honey")){
            // turn on the slime ability to climbWalls
            GetComponent<Slime>().SetClimbWalls(true);
            Debug.Log("Slime climbWall state: " + GetComponent<Slime>().GetClimbWalls());
        }
        // if the object is a wall and climbWalls is true
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall") && GetComponent<Slime>().GetClimbWalls()){
            // allow wall climbing 

        }
    }

    // adds in the slime specifc collisions
    protected void OnTriggerEnter2D(Collider2D collider)
    {   
        // check for collision with tunnel entrance
        if (collider.gameObject.CompareTag("Tunnel Entrance"))
        {
            Debug.Log("Slime has hit a tunnel entrance");
            // convert the collision into a boxCollider to get its size 
            BoxCollider2D boxCollider = collider as BoxCollider2D;
            // check if the colldier is a boxCollider
            if (boxCollider != null){
                Debug.Log("Changing slime colldier size");
                Debug.Log("Current slime colldier height: " + currentSize.y);
                // get the height of the tunnel entrance
                float tunnelHeight = boxCollider.size.y;
                // create a new vector for the slimes new collider size
                Vector2 newCollider;
                // set the width to the slimes current collider width
                newCollider.x = currentSize.x;
                // set the height to the tunnel colliders height
                newCollider.y = tunnelHeight - 0.1f;
                Debug.Log("Tunnel height: " + tunnelHeight);
                // change the collider box of the slime so it can squish through the tunnel
                slimeCollider.size = newCollider;
                Debug.Log("Slime colliders size has changed");
                Debug.Log("New slime colldier height: " + slimeCollider.size.y);
            }
        }
    }

    // slime specifc exit actions when leaving colliders
    private void OnTriggerExit2D(Collider2D collider){
        // check for collision with tunnel entrance
        if (collider.gameObject.CompareTag("Tunnel Entrance"))
        {
            Debug.Log("Slime has left a tunnel entrance");
            // change the collider box of the slime so it can squish through the tunnel
            slimeCollider.size = currentSize;
        }
    }

}
