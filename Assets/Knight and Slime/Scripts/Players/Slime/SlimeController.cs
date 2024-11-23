using System.Numerics;
using UnityEngine;

public class SlimeController : PlayerControllerBase
{
    // Public variables -- the options in unity
    public float wallClimbSpeed = 3f;
    // private vars
    // save the current size of the slime colldier
    private UnityEngine.Vector2 currentSize;
    // the slimes collider
    BoxCollider2D slimeCollider;
    // if the slime is currently touching a wall
    private bool isTouchingWall = false;
    // number of seperate walls the slime is touching 
    private int wallContactCount = 0;

    protected override void Start()
    {
        base.Start();
        // set the slime collider 
        slimeCollider = GetComponent<BoxCollider2D>();
        // get the current size of the slimes collider and save it for later 
        currentSize = slimeCollider.size; 
        rb = GetComponent<Rigidbody2D>();
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
        // if the object is a wall
        if (collision.gameObject.layer == LayerMask.NameToLayer("Walls") || collision.gameObject.CompareTag("Wall")){
            Debug.Log("Slime is touching wall");
            // up the wall contact count
            wallContactCount++;
            // slime is touching wall 
            isTouchingWall = true;
        }
    }

    // actions to take when leaving colldiers
    private void OnCollisionExit2D(Collision2D collision)
    {
        // if the object is a wall
        if (collision.gameObject.layer == LayerMask.NameToLayer("Walls") || collision.gameObject.CompareTag("Wall")){
            Debug.Log("Slime is no longer touching wall");
            // reduce the wall count 
            wallContactCount--;
            // check if wall count is 0
            if (wallContactCount == 0){
                // slime is no longer touching wall 
                isTouchingWall = false;
            }
        }
    }

    protected override void Update(){
        // run the basic update
        base.Update();
        // slime specifc update
        if (GetComponent<Slime>().GetClimbWalls() && isTouchingWall){
            Debug.Log("Slime can start climbing wall");
            // check for vertical input from the arrow keys
            float verticalInput = Input.GetAxis("VerticalArrowKeys");
            // if moving 
            if (Mathf.Abs(verticalInput) > 0.1f){
                // move the slime vertically on the wall
                rb.velocity = new UnityEngine.Vector2(rb.velocity.x, verticalInput *wallClimbSpeed);
            }else{
                // stick to the wall when not moving
                rb.velocity = new UnityEngine.Vector2(rb.velocity.x, 0f);
            }

            // set gravity to 0 -- see how it feels
            rb.gravityScale = 0f;
        } else {
            // set the gravity to normal when not climbing
            rb.gravityScale = 1.5f;
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
                UnityEngine.Vector2 newCollider;
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
