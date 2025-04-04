using System.Numerics;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public abstract class PlayerControllerBase : MonoBehaviour
{
    // Public variables -- the options in unity
    // The speed at which the player moves
    [Header ("Move Type")]
    // Controls whether the player can move diagonally
    public bool canMoveDiagonally = false; 
    // can they move up?
    public bool canMoveVertically = false;
    // does wasd control them?
    public bool wasd;
    // do arrow keys control them?
    public bool arrowKeys;

    [Header ("Animation Stuff")]
    public bool hasAnimations = false;
    public bool jumpAnim = false;
    public bool runAnim = false;
    public bool fallAnim = false;
    public bool grounded = false;

    [Header ("Move Stats")]
    public float speed = 5f; 
    
    // what triggers their jump
    [Header ("Jump Input Type")]
    public bool spaceBar;
    public bool upArrow;
    public bool w;

    [Header ("Jumping")]
    // can they jump 
    public bool jump;
    // how fast they jump
    public float jumpForce = 7f;
    // the ditance the box cast is offset below the player
    public float boxCastDist = 0.1f;
    // jummping offset
    public UnityEngine.Vector2 jumpColliderOffset;
    // jummping size
    public UnityEngine.Vector2 jumpColliderSize;

    [Header ("Jumping Grounding Layer")]
    // normal grounding 
    public bool normal;
    // normal + liquid grounding
    public bool normalAndLiquid;

    [Header ("Collider Changes")]
    // public vars for changing colldier for different animations
    // idle and running size
    public UnityEngine.Vector2 normalColliderSize;
    public UnityEngine.Vector2 normalColliderOffset;

    [Header ("Death")]
    // the line the player must cross to die when falling 
    public float deathLine = -10;

    // Private variables -- mostly refs
    // Reference to the Rigidbody2D component attached to the player
    protected Rigidbody2D rb; 
    // Stores the direction of player movement
    protected UnityEngine.Vector2 movement; 
     // Flag to track if the player is moving horizontally
    protected bool isMovingHorizontally = true;
    // animation vars
    protected Animator anim;
    // for changing the collider size
    protected BoxCollider2D boxCollider;
    // the layer masks for grounding their general themes
    protected LayerMask ground;
    protected LayerMask groundAndLiquids;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        // Initialize the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        // reference the box collider componet
        boxCollider = GetComponent<BoxCollider2D>();
        // Prevent the player from rotating
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        if (hasAnimations){
            //initialize the animation compoent
            anim = GetComponent<Animator>();
        }
        // setup the layers for jumping
        GroundingLayerSetup();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // vars to hold the movement input
        float horizontalInput = 0;
        float verticalInput = 0;

        // get the input from the right source
        InputType(ref horizontalInput, ref verticalInput);

        // Check if diagonal movement is allowed
        if (canMoveDiagonally)
        {
           HandleDiagonalMovement(horizontalInput, verticalInput);
        }
        // check if vertical movement is allowed
        else if (canMoveVertically)
        {
            HandleVerticalMovement(horizontalInput, verticalInput);
        }
        // can only move horizontally -- part used for this platformer
        else {
            HandleHorizontalMovement(horizontalInput);
        }

        CheckIfOffMap();
    }

    protected virtual void FixedUpdate()
    {
        // Preserve vertical velocity during the jump
        rb.velocity = new UnityEngine.Vector2(movement.x * speed, rb.velocity.y);

        // check the types of anims
        if (jumpAnim && fallAnim){
            // check the chracters velocity 
            if (rb.velocity.y > 0){
                anim?.SetBool("Jump", true);
                anim?.SetBool("Falling", false);
            } else if (rb.velocity.y < 0){
                anim?.SetBool("Jump", false);
                anim?.SetBool("Falling", true);
            }
        }
    }

    // sets up both types of grounding 
    private void GroundingLayerSetup(){
        // the layers to put in the grounding layer types
        int groundLayer = LayerMask.NameToLayer("Ground");
        int objectLayer = LayerMask.NameToLayer("Object");
        int movingPlatformLayer = LayerMask.NameToLayer("Moving Platform");

        int waterLayer = LayerMask.NameToLayer("Water");

        // build the ground layer mask
        ground = 1 << groundLayer | 1 << objectLayer | 1 << movingPlatformLayer;
        // build the ground and liquids layer mask
        groundAndLiquids = 1 << groundLayer | 1 << objectLayer | 1 << movingPlatformLayer | 1 << waterLayer;
    }

    // diagonal movement
    protected virtual void HandleDiagonalMovement(float horizontalInput, float verticalInput){
        // Set movement direction based on input
        movement = new UnityEngine.Vector2(horizontalInput, verticalInput);
        // Optionally rotate the player based on movement direction
        RotatePlayer(horizontalInput, verticalInput);
    }

    // vertical and horizonmtal but no diagonal
    protected virtual void HandleVerticalMovement(float horizontalInput, float verticalInput){
        // Determine the priority of movement based on input
        if (horizontalInput != 0)
        {
            isMovingHorizontally = true;
        }
        else if (verticalInput != 0)
        {
            isMovingHorizontally = false;
        }

        // Set movement direction and optionally rotate the player
        if (isMovingHorizontally)
        {
            movement = new UnityEngine.Vector2(horizontalInput, 0);
            RotatePlayer(horizontalInput, 0);
        }
        else
        {
            movement = new UnityEngine.Vector2(0, verticalInput);
            RotatePlayer(0, verticalInput);
        }
    }

    //handle horizontal movement
    protected virtual void HandleHorizontalMovement(float horizontalInput){
        // Set movement direction
        movement = new UnityEngine.Vector2(horizontalInput, 0);

        // flip the player if needed
        // flip player on x depending on direction movement
        if ((horizontalInput > 0.01f && transform.localScale.x < 0) || (horizontalInput < -0.01f && transform.localScale.x > 0)){
            Flip();
        }

        // handle jumping
        TriggerJump();

        // check if they have the anims -- should probably move into its own anims func later ******
        if (runAnim){
            //run running animation
            anim?.SetBool("Run", horizontalInput != 0);
        }
        if (grounded){
            // for switching to idle anim
            anim?.SetBool("Grounded", isGrounded());
        }
    }

    // check for correct player input for when to jump
    private void TriggerJump(){
        // check if they can jump and if they are grounded
        if(jump && isGrounded()){
            // check what their jump input is 
            if (spaceBar){
                // check for the input 
                if (Input.GetKeyDown(KeyCode.Space)){
                    Jump();
                    return;
                }
            }
            if (w){
                if (Input.GetKeyDown(KeyCode.W)){
                    Jump();
                    return;
                }
            }
            if (upArrow){
                if (Input.GetKeyDown(KeyCode.UpArrow)){
                    Jump();
                    return;
                }
            }
        }
    }

    private void Jump(){
        rb.velocity = new UnityEngine.Vector2(rb.velocity.x, jumpForce);
        if (jumpAnim && !fallAnim){
            anim?.SetTrigger("Jump");
        }
    }

    // set colldier to jump size -- not used right now 
    private void AdjustColliderForJump(){
        boxCollider.size = jumpColliderSize;
        boxCollider.offset = jumpColliderOffset;
    }

    // check the bools set in the unity editor and get input from the correct source
    protected void InputType(ref float horizontalInput, ref float verticalInput){
        // Get player input from keyboard or controller
        // assign the inputs based on the movement input bools
        if (wasd){
            horizontalInput = Input.GetAxisRaw("HorizontalWASD");
            // the axis does not exist yet
            // float verticalInput = Input.GetAxisRaw("VerticalWASD"); 
        } else if (arrowKeys){
            horizontalInput = Input.GetAxisRaw("HorizontalArrowKeys");
            // the axis does not exist yet
            // verticalInput = Input.GetAxisRaw("VerticalArrowKeys");
        } else if (wasd && arrowKeys){
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
        } else {
            Debug.LogError("No input assigned to the character");
        }
    }

    // actions performed on collisions
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        // Check for collisions with death objects: invisible colliders for map detection, acid, spikes, mobs, 
        if (collision.gameObject.CompareTag("DeathObject"))
        {
            // kill player using method from PlayerBase
            GetComponent<PlayerBase>().KillPlayer(); 
        }
    }

    // check if the player falls off the map
    protected void CheckIfOffMap()
    {
        if (transform.position.y < deathLine)
        {
            // kill player using method from PlayerBase
            GetComponent<PlayerBase>().KillPlayer(); 
        }
    }

    // check if player is grounded, uses the grounding layer selected, the default is no layer
    protected virtual bool isGrounded(){
        // setup the raycast hit var
        RaycastHit2D raycastHit;
        // check the grounding layer used
        if (normal){
            raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, UnityEngine.Vector2.down, boxCastDist, ground);
            return raycastHit.collider != null;
        } else if (normalAndLiquid){
            raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, UnityEngine.Vector2.down, boxCastDist, groundAndLiquids);
            return raycastHit.collider != null;
        }

        // check if they have the falling animation
        if (fallAnim){
            anim?.SetBool("Falling", false);
        }

        return false;
    }

    // rotate the player
    void RotatePlayer(float x, float y)
    {
        // If there is no input, do not rotate the player
        if (x == 0 && y == 0) return;

        // Calculate the rotation angle based on input direction
        float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        // Apply the rotation to the player
        transform.rotation = UnityEngine.Quaternion.Euler(0, 0, angle);
    }

    // reset the collider to it's normal state
    protected void ResetCollider(){
        boxCollider.size = normalColliderSize;
        boxCollider.offset = normalColliderOffset;
    }

    // flip the player on the x
    protected virtual void Flip(){
        // grab the current scale
        UnityEngine.Vector3 scale = transform.localScale;
        // flip the x
        scale.x *= -1;
        // assign new scale
        transform.localScale = scale;
    }
}
