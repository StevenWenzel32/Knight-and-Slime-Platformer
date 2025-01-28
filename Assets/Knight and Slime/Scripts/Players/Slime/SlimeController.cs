using System.Numerics;
using UnityEngine;

public class SlimeController : PlayerControllerBase
{
    // Public variables -- the options in unity
    [Header("Abilities Stats")]
    public float wallClimbSpeed = 3f;

    [Header("Tunnels")]
    public float colliderWidthShrinkBuffer = 0.8f;
    // private vars
    // save the origional size of the slime collider
    private UnityEngine.Vector2 startSize;
    // save the current size of the slime colldier
    private UnityEngine.Vector2 currentSize;
    // the slimes collider
    BoxCollider2D slimeCollider;
    // if the slime is currently touching a wall
    private bool isTouchingWall = false;
    // number of seperate walls the slime is touching 
    private int wallContactCount = 0;
    // if the is on the slimes left side
    private bool isTouchingWallLeft = false;
    // if the wall is on the slimes right side
    private bool isTouchingWallRight = false;
    // the slimes sprite rendere

    // consts
    const float SUPER_JUMP_FORCE = 21; 

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
        // call the base collsion
        base.OnCollisionEnter2D(collision);

        // check if the object is a liquid
        Liquid liquid = collision.gameObject.GetComponent<Liquid>(); 

        // collectables for the slime 
        if (liquid != null){
            // call the liquids absorb func
            liquid.Absorb(gameObject);
        }

        // check for liquid collisions -- put into a seperate func later on ******
        // if the object is honey
        if (collision.gameObject.CompareTag("Honey") && !GetComponent<Slime>().GetLiquidAbsorbed()){
            // mark the slime has having absorbed a liquid
            GetComponent<Slime>().SetLiquidAbsorbed(true);
            // turn on the slime ability to climbWalls
            GetComponent<Slime>().SetClimbWalls(true);
            Debug.Log("Slime climbWall state: " + GetComponent<Slime>().GetClimbWalls());
        }
        // if the object is super jump 
        if (collision.gameObject.CompareTag("Super Jump") && !GetComponent<Slime>().GetLiquidAbsorbed()){
            // check if the slime 
            // mark the slime has having absorbed a liquid
            GetComponent<Slime>().SetLiquidAbsorbed(true);
            // turn on the slime ability to climbWalls
            GetComponent<Slime>().SetSuperJump(true);
            // turn on the super jump
            SuperJump();
            Debug.Log("Slime superJump state: " + GetComponent<Slime>().GetSuperJump());
        }

        // if the object is a wall
        if (collision.gameObject.layer == LayerMask.NameToLayer("Walls") || collision.gameObject.CompareTag("Wall")){
            // up the wall contact count
            wallContactCount++;
            // slime is touching wall 
            isTouchingWall = true;

// unused for now -- can improve sprit flipping and rotation by having seperate sprites maybe?? **********
//             // wall on the right
//             if (collision.contacts[0].normal.x < 0){
//                 Debug.Log("Slime is touching wall on the right");
//                 // its on the right side
//                 isTouchingWallRight = true;
//                 isTouchingWallLeft = false;
//                 // rotate the slime to the left
// //                RotateCharacter(90, collision);
//             }
//             // wall on the left
//             else if (collision.contacts[0].normal.x > 0){
//                 Debug.Log("Slime is touching wall on the left");
//                 // its on the left side
//                 isTouchingWallRight = false;
//                 isTouchingWallLeft = true;
//                 // rotate the slime to the right
//  //               RotateCharacter(-90, collision);
//             }
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
                // its on the left side
                isTouchingWallRight = false;
                isTouchingWallLeft = false;
                // reset the slimes rotation to normal
//                RotateCharacter(0, collision);
            }
        }
    }

    protected override void FixedUpdate(){
        // run the basic update
        base.FixedUpdate();
        // slime specifc update
        if (GetComponent<Slime>().GetClimbWalls() && isTouchingWall){
            Debug.Log("Slime can start climbing wall");
            // check for vertical input from the arrow keys
            float verticalInput = Input.GetAxis("VerticalArrowKeys");
            // if moving 
            if (Mathf.Abs(verticalInput) > 0.1f){
                // check the movement of the character and flip it
//                FlipCharacter();
                // move the slime vertically on the wall
                rb.velocity = new UnityEngine.Vector2(rb.velocity.x, verticalInput * wallClimbSpeed);
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
        Collectible2D collectible = collider.GetComponent<Collectible2D>();

        // collectables for the slime 
        if (collectible != null && !collectible.CompareTag("Key")){
            // call the objects collect func
            collectible.Collect(gameObject);
        }
        // check for collision with tunnel x and tunnel y
        else if (collider.gameObject.CompareTag("Tunnel Y")){
            Debug.Log("Slime has hit a tunnel Y");
            // convert the collision into a boxCollider to get its size 
            BoxCollider2D boxCollider = collider as BoxCollider2D;
            // check if the colldier is a boxCollider
            if (boxCollider != null){
                Debug.Log("Changing slime colldier size");
                Debug.Log("Current slime colldier height: " + currentSize.y);
                // get the height of the tunnel
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
        } else if (collider.gameObject.CompareTag("Tunnel X")){
            Debug.Log("Slime has hit a tunnel X");
            // convert the collision into a boxCollider to get its size 
            BoxCollider2D boxCollider = collider as BoxCollider2D;
            // check if the colldier is a boxCollider
            if (boxCollider != null){
                Debug.Log("Changing slime colldier size");
                Debug.Log("Current slime colldier width: " + currentSize.x);
                // get the width of the tunnel
                float tunnelWidth = boxCollider.size.x;
                // create a new vector for the slimes new collider size
                UnityEngine.Vector2 newCollider;
                // set the width to the tunnel width
                newCollider.x = tunnelWidth - colliderWidthShrinkBuffer;
                // set the height to the slimes current hight
                newCollider.y = currentSize.y;
                Debug.Log("Tunnel Width: " + tunnelWidth);
                // change the collider box of the slime so it can squish through the tunnel
                slimeCollider.size = newCollider;
                Debug.Log("Slime colliders size has changed");
                Debug.Log("New slime colldier width: " + slimeCollider.size.x);
            }
        }
    }   

    // slime specifc exit actions when leaving colliders
    private void OnTriggerExit2D(Collider2D collider){
        // check for collision with tunnel entrance
        if (collider.gameObject.CompareTag("Tunnel X"))
        {
            Debug.Log("Slime has left a tunnel X");
            // create a new vector for the slimes new collider size and set to slimes current collider -- keeps the height the same
            UnityEngine.Vector2 newCollider = slimeCollider.size;
            // set the width to the slimes old width
            newCollider.x = currentSize.x;
            // undo the slimes width change
            slimeCollider.size = newCollider;
        } else if (collider.gameObject.CompareTag("Tunnel Y")){
            Debug.Log("Slime has left a tunnel Y");
            // create a new vector for the slimes new collider size and set to slimes current collider -- keeps the width the same
            UnityEngine.Vector2 newCollider = slimeCollider.size;
            // set the height to the slimes old height
            newCollider.y = currentSize.y;
            // undo the slimes height change
            slimeCollider.size = newCollider;
        }
    }

    // rotate the character the given angle, and the wall collision
    private void RotateCharacter(float angle, Collision2D collision){
        // Rotate the slime
        transform.rotation = UnityEngine.Quaternion.Euler(0, 0, angle);
        Debug.Log($"Slime rotated to {angle} degrees");

        // Get the wall normal to adjust the position correctly
        UnityEngine.Vector2 wallNormal = collision.contacts[0].normal;

        // Calculate the adjustment vector based on collider size
        UnityEngine.Vector2 adjustment = wallNormal * (slimeCollider.size.x / 2f);

        // Apply the adjustment to the Rigidbody2D position
        rb.position = rb.position - adjustment;
    }

    // Flip the character based on horizontal movement
    private void FlipCharacter(){
        float VerticalInput = Input.GetAxis("VerticalArrowKeys");
        // moving up
        if (VerticalInput > 0){
            transform.localScale = UnityEngine.Vector3.one;
        } 
         // Moving down
        else if (VerticalInput < 0){
            transform.localScale = new UnityEngine.Vector3(-1, 1, 1);
        }
    }

    // activates the super jump ability
    private void SuperJump(){
        // turn the jump option on 
        this.jump = true;
        // set the jump force to SUPER
        this.jumpForce = SUPER_JUMP_FORCE;
    }
}
