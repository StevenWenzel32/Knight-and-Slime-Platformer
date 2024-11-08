using UnityEngine;

public class Controller : MonoBehaviour
{
    // movement and zoom speed settings
    private Vector3 velocity = Vector3.zero;

    [Header ("Base Camera")]
    // how fast the camera follows the players
    public float cameraSpeed;

    [Header ("Zooming Camera")]
    // how fast the camera adjusts its size
    public float zoomSpeed;
    // the min camera size
    public float minZoom;
    // the max camera size
    public float maxZoom;
    // give extra space around the players
    public float zoomBuffer;
    // distance from the edge of the camera before zooming out starts
    public float distFromEdge;
    // distance between players where it does not zoom in
    public float closeTogether;

    // for player
    [Header ("Knight")]
    public Transform knight;

    [Header ("Slime")]
    public Transform slime;

    [Header ("Level Info")]

    // private vars
    private Camera cam;
    private Vector3 startPosition;

    private void Start()
    {
        // Get the camera component
        cam = GetComponent<Camera>();
        // get the current camera position to keep y and z constant
        startPosition = transform.position;
    }

    // late update is called after update allowing the camera to be set after the players have been updated
    // better for accuracy
    void LateUpdate()
    {
        // follow players
        // calculate the midpoint between players
        Vector3 midpoint = (knight.position + slime.position) / 2f;

        // have the camera follow the midpoint -- only follows on the x right now -- no y no z changes
        Vector3 cameraPos = new Vector3(midpoint.x, transform.position.y, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, cameraPos, ref velocity, cameraSpeed);

        // find the bottom of the level
        float levelBottom = CalculateLevelBottom();

        // get the distance between the players
        float distance = Vector2.Distance(knight.position, slime.position);

        // check if either player is near the edge of the camera
        if (PlayerNearEdge()){
            // have the camera change size -- the zooming 
            float newSize = Mathf.Clamp(distance + zoomBuffer, minZoom, maxZoom);
            // transition the camera size
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newSize, Time.deltaTime * zoomSpeed);
            // change the cameras y to keep the level at the bottom of the camera
            AnchorBottom(levelBottom);
        } 
        // if the players are not close enough together yet zoom in
        else if (cam.orthographicSize > minZoom){
            // have the camera change size -- the zooming 
            float newSize = Mathf.Clamp(distance + zoomBuffer, minZoom, maxZoom);
            // only change the camera size if the new size is smaller than the current one
            if (cam.orthographicSize > newSize){
                // transition the camera size
                cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newSize, Time.deltaTime * zoomSpeed);
                // change the cameras y to keep the level at the bottom of the camera
                AnchorBottom(levelBottom);
            }
        }
        
    }

    // when the player collides with the door object it will move them to the next room
    public void MoveToNewRoom(Transform newRoom){
        transform.position = new Vector3(newRoom.position.x, newRoom.position.y, transform.position.z);
    }

    // check if the players are near the edge of the camera -- only works for x axis right now
    private bool PlayerNearEdge(){
        // find the edges of the camera
        float cameraHalfWidth = cam.orthographicSize * cam.aspect;
        // not used yet
        // float cameraHalfHeight = cam.orthographicSize;

        // get the distance from the edge of the camera and the players
        float knightEdgeX = Mathf.Abs(knight.position.x - transform.position.x);
        float slimeEdgeX = Mathf.Abs(slime.position.x - transform.position.x);

        // the threshold to cross to start zooming 
        float widthThreshold = cameraHalfWidth - distFromEdge;
        // currently not used -- would be used for height zooming needs
        // float heightThreshold = cameraHalfWidth - distFromEdge;

        // check if either player is near the edge
        return knightEdgeX >= widthThreshold || slimeEdgeX >= widthThreshold;
    }

    // change the cameras y position after zooming to make sure the level is always at the bottom of the camera
    private void AnchorBottom(float levelBottom){
        // get the offset from the zoom -- o.5??
        float newY = levelBottom + (cam.orthographicSize - minZoom) * 0.5f;
        // change the camera position
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // have the camera move to the anchored position smoothly -- probbaly not wanted
        // Vector3 anchoredPos = new Vector3(transform.position.x, newY, transform.position.z);
        // Vector3.SmoothDamp(transform.position, anchoredPos, ref velocity, cameraSpeed);
    }

    // find the bottom of the level 
    private float CalculateLevelBottom(){
        // find the bottom of the level
        return transform.position.y - cam.orthographicSize;
    }
}
