using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class KnightController : PlayerControllerBase
{
    // Public variables -- the options in unity
    // how fast they jump
    public float jumpForce = 7f;
    // jummping offset
    public Vector2 jumpColliderOffset;
    // jummping size
    public Vector2 jumpColliderSize;
    
    protected override void Start()
    {
        base.Start();
    }

    protected override void HandleHorizontalMovement(float horizontalInput)
    {
        base.HandleHorizontalMovement(horizontalInput);
                
        // let the player jump
        if (Input.GetKey(KeyCode.Space) && isGrounded()){
            Jump();
        }
        // run jumping animation
        anim.SetBool("Grounded", isGrounded());
        // if grounded reset colldier
    //    if (isGrounded()){
    //         ResetCollider();
    //     }
    }

    // unique to the knight
    private void Jump(){
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        anim.SetTrigger("Jump");
  //      AdjustColliderForJump();
    }

    // set colldier to jump size
    private void AdjustColliderForJump(){
        boxCollider.size = jumpColliderSize;
        boxCollider.offset = jumpColliderOffset;
    }
}
