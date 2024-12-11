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

    [Header ("Collider Changes")]
    public bool hasAnimations = false;

    [Header ("Move Stats")]
    public float speed = 5f; 
    
    [Header ("Jumping")]
    // can they jump 
    public bool jump;
    // how fast they jump
    public float jumpForce = 7f;
    // jummping offset
    public Vector2 jumpColliderOffset;
    // jummping size
    public Vector2 jumpColliderSize;
    // might have something to do with the camera????
    public LayerMask groundLayer;

    [Header ("Collider Changes")]
    // public vars for changing colldier for different animations
    // idle and running size
    public Vector2 normalColliderSize;
    public Vector2 normalColliderOffset;

    [Header ("Death")]
    // the line the player must cross to die when falling 
    public float deathLine = -10;

    // Private variables -- mostly refs
    // Reference to the Rigidbody2D component attached to the player
    protected Rigidbody2D rb; 
    // Stores the direction of player movement
    protected Vector2 movement; 
     // Flag to track if the player is moving horizontally
    protected bool isMovingHorizontally = true;
    // animation vars
    protected Animator anim;
    // for changing the collider size
    protected BoxCollider2D boxCollider;

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
        rb.velocity = new Vector2(movement.x * speed, rb.velocity.y);
    }

    // diagonal movement
    protected virtual void HandleDiagonalMovement(float horizontalInput, float verticalInput){
        // Set movement direction based on input
        movement = new Vector2(horizontalInput, verticalInput);
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
            movement = new Vector2(horizontalInput, 0);
            RotatePlayer(horizontalInput, 0);
        }
        else
        {
            movement = new Vector2(0, verticalInput);
            RotatePlayer(0, verticalInput);
        }
    }

    //handle horizontal movement
    protected virtual void HandleHorizontalMovement(float horizontalInput){
        // Set movement direction
        movement = new Vector2(horizontalInput, 0);

        // flip player on x depending on direction movement
        if (horizontalInput > 0.01f){
            transform.localScale = Vector3.one;
        } else if (horizontalInput < -0.01f){
            transform.localScale = new Vector3(-1, 1, 1);
        }

        // let the player jump
        if (jump && isGrounded() && (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W))){
            Jump();
        }

        // check if the char has animations
        if (hasAnimations){
            //run running animation
            anim.SetBool("Run", horizontalInput != 0);
            // run jumping animation
            anim.SetBool("Grounded", isGrounded());
        }
    }

    // unique to the knight
    private void Jump(){
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        anim.SetTrigger("Jump");
  //      AdjustColliderForJump();
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

    // check if player is grounded
    protected virtual bool isGrounded(){
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    // rotate the player
    void RotatePlayer(float x, float y)
    {
        // If there is no input, do not rotate the player
        if (x == 0 && y == 0) return;

        // Calculate the rotation angle based on input direction
        float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        // Apply the rotation to the player
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // reset the collider to it's normal state
    protected void ResetCollider(){
        boxCollider.size = normalColliderSize;
        boxCollider.offset = normalColliderOffset;
    }
}
