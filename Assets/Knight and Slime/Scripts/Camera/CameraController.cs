using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

// make sure that there is a camera before running the code
[RequireComponent(typeof(Camera))]
public class Controller : MonoBehaviour
{
    [Header ("Camera Settings")]
    // how fast the camera follows the players
    public float smoothTime = 0.5f;
    // buffer around the players
    public UnityEngine.Vector3 offest;
    // default/ 0 velocity
    private UnityEngine.Vector3 velocity = UnityEngine.Vector3.zero;
    public float minZoom;
    public float maxZoom;
    // to limit the zoom size
    public float zoomLimiter;
    // magic number that keeps the level at the bottom of the screen
    public float characterHeight;
    // bounds for the max position placement for the camera
    public UnityEngine.Vector2 maxPositionBounds;

    [Header ("Players")]
    // holds the player objects
    public List<Transform> targets;

    // the bounds/box to hold the targets
    private Bounds bounds;
    private Camera cam;

    void Start(){
        cam = GetComponent<Camera>();
    }

    // late update is called after update allowing the camera to be set after the players have been updated
    // better for accuracy
    void LateUpdate()
    {
        // make sure the camera has something to follow
        if (targets.Count == 0){
            Debug.LogError("There's to targets for the camera to follow");
            return;
        }
        // move the camera based on the targets positions
        UnityEngine.Vector3 newPosition = MoveCamera();
        // zoom the camera based on the bounds width
        ZoomCamera(newPosition);
    }
    
    // create a bounds (which is basically a box) that cotains all the targets
    private void MakeBounds(){
        if (targets.Count == 1){
            bounds = new Bounds(targets[0].position, velocity);
            return;
        }

        // make a bounds object that holds all the targets -- basically a box
        bounds = new Bounds(targets[0].position, velocity);

        // add in the rest of the targets to the bounds box
        for (int i = 0; i < targets.Count; i++){
            bounds.Encapsulate(targets[i].position);
        }
    }

    // move the camera to the center point of the bounds box that holds all the players
    // returns the newPositon 
    private UnityEngine.Vector3 MoveCamera(){
        MakeBounds();
        // follow players
        // calculate the midpoint between all targets/players
        UnityEngine.Vector3 centerPoint = bounds.center;
        // add the offest buffer to the bounds
        UnityEngine.Vector3 newPosition = centerPoint + offest;
        //make sure the z axis is fixed since we are in 2d
        newPosition.z = transform.position.z;
        

        // clamp the camera position based on the bounds
    //    float cameraHalfHeight = cam.orthographicSize;
    //    newPosition.y = Mathf.Clamp(newPosition.y, cameraHalfHeight, maxPositionBounds.y - cameraHalfHeight);

        // set the position of the camera
       transform.position = UnityEngine.Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);

        return newPosition;
    }

    // zoom in on the targets based on the greatest distance between them -- which is the width of bounds 
    private void ZoomCamera(UnityEngine.Vector3 newPosition){
        // the width of the bounds box
        float width = bounds.size.x;
        // go between the max and min set of the zoom size
        // not super sure what zoomLimiter does???
        float newZoom = Mathf.Lerp(minZoom, maxZoom, width / zoomLimiter);
        // change the orthographicSize of the camera -- the zooming
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime);
        // find the bottom of the level by checking what the lowest y point within the bounds is
        // add the character height as an offset
        float levelBottom = bounds.min.y + characterHeight;
        // up data the camera position based on the levelBottom
        newPosition.y = levelBottom + cam.orthographicSize;
        // update the camera position
        transform.position = UnityEngine.Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    // when the player collides with the door object it will move them to the next room
    public void MoveToNewRoom(Transform newRoom){
        transform.position = new UnityEngine.Vector3(newRoom.position.x, newRoom.position.y, transform.position.z);
    }

    
}
