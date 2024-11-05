using Unity.VisualScripting;
using UnityEngine;

public class Controller : MonoBehaviour
{
    // for the room camera
    // how fast the camera moves
    public float speed;
    private float currentPosX;
    private Vector3 velocity = Vector3.zero;

    // for player
    public Transform player;
    public float aheadDistance;
    public float cameraSpeed;
    private float lookAhead;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // room camera
        //transform.position = Vector3.SmoothDamp(transform.position, new Vector3(currentPosX, transform.position.y, transform.position.z), ref velocity, speed);

        // follow player
        // only follows on the x right now
        transform.position = new Vector3(player.position.x, transform.position.y, transform.position.z);
        lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * player.localScale.x), Time.deltaTime * cameraSpeed);
    }

    public void MoveToNewRoom(Transform newRoom){
        currentPosX = newRoom.position.x;
    }
}
