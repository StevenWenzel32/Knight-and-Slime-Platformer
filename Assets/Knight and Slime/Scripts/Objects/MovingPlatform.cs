using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Animator anim;
    // will be changed inside a switch or button
    private bool move = false;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        if (anim != null){
            // grab the animator on the platform object
            anim = GetComponent<Animator>();
        }
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (anim != null){
            // play the move animation based on the value of move, move will be changed by a script in a switch or button
            anim.SetBool("Move", move);
        }  
        if (move){
            // smoothly move to the target
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        } else {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, moveSpeed * Time.deltaTime);
        }
    }

    // toggle the platform and call the function to move it the right direction -- can later add more directions and rotations and speeds
    public void TogglePlatform(string direction, float distance, float speed){
        // siwtch the move state
        move = !move;
        // set the move speed to the given
        moveSpeed = speed;
        // check what direction to move the platform
        if (direction == "left"){
            moveLeft(distance);
        } else if (direction == "right"){
            moveRight(distance);
        } else if (direction == "up"){
            moveUp(distance);
        } else if (direction == "down"){
            moveDown(distance);
        }
        // check if the platform should rotate ****
    }

    private void moveLeft(float distance){
        Debug.Log("moving platform to the left");
        targetPosition = startPosition + Vector3.left * distance;
    }

    private void moveRight(float distance){
        Debug.Log("moving platform to the right");
        targetPosition = startPosition + Vector3.right * distance;
    }

    private void moveUp(float distance){
        Debug.Log("moving platform up");
        targetPosition = startPosition + Vector3.up * distance;
    }

    private void moveDown(float distance){
        Debug.Log("moving platform down");
        targetPosition = startPosition + Vector3.down * distance;
    }
}
