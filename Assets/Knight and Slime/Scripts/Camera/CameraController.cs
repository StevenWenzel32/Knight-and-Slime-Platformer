using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

// make sure that there is a camera before running the code
[RequireComponent(typeof(Camera))]
public class Controller : MonoBehaviour
{
    [Header ("Camera Moving")]
    // how fast the camera gets to the new position
    public float dampTime = 0.2f;
    // buffer around the players and the edge of the screen
    public float screenBuffer = 2f;

    [Header ("Camera Zooming")]
    // min zoom size
    public float minZoom = 11.3f;
    // max zoom size
    public float maxZoom = 25f;
    // how fast the zoom changes -- used in the damping 
    public float zoomDampTime = 0.4f;

    [Header ("Players")]
    // holds the player objects
    public List<Transform> targets;
    // extra adjustment added to the y position of the camera when based on players
    private float characterHeight = 2.5f;
    // extra adjustment added to the y position of the camera when based on ground
    public float groundHeight = 11.1f;

    // extra adjustment added to the x position of the camera when based on players -- value has not been found yet
    private float characterWidth = 2.5f;
    // extra adjustment added to the x position of the camera when based on ground
    public float wallWidth = 100f;

    // the bounds/box to hold the targets
    private Bounds bounds;
    private Camera cam;
    // default/ 0 velocity for moving the zoom
    private Vector3 velocity = Vector3.zero;
    // smooth damp velocity
    private float zoomVelocity = 0f;

    void Awake(){
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
        MoveCamera();
        // zoom the camera based on the bounds width
        ZoomCamera();
    }
    
    // create a bounds (which is basically a box) that cotains all the targets
    private void MakeBounds(){
        if (targets.Count == 1){
            bounds = new Bounds(targets[0].position, Vector3.zero);
            return;
        }

        // make a bounds object that holds all the targets -- basically a box
        bounds = new Bounds(targets[0].position, Vector3.zero);

        // add in the rest of the targets to the bounds box
        for (int i = 0; i < targets.Count; i++){
            bounds.Encapsulate(targets[i].position);
        }
    }

    // move the camera to the center point of the bounds box that holds all the players
    // returns the newPositon 
    private void MoveCamera(){
        // the container for tracking the targets
        MakeBounds();
        // follow players
        // calculate the midpoint between all targets/players
        Vector3 centerPoint = bounds.center;
        Vector3 newPosition = centerPoint;
        // make sure the z axis is fixed since we are in 2d
        newPosition.z = transform.position.z;
        // get the bottom of the level = min y position
        float levelBottom = CalcBottomScene();
        // get the left edge of the level = min x position -- might try to add in later *****
        float levelLeftEdge = CalcLeftEdgeScene();
        // adjust the y position to respect the bottom of the level
        newPosition.y = Mathf.Max(centerPoint.y, levelBottom);
        // adjust the x position to respect the left edge of the level
        newPosition.x = Mathf.Max(centerPoint.x, levelLeftEdge);

        // set the position of the camera
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, dampTime);
    }

    // zoom in on the targets based on the greatest distance between them -- which is the width of bounds 
    private void ZoomCamera(){
        // the width of the bounds box -- max distance between all targets + buffer
        float width = bounds.size.x + screenBuffer;
        // find the height of the bounds box
        float height = bounds.size.y + screenBuffer;
        // compare width and height
        float max = Mathf.Max(width, height); 
        // keep the zoom in the set bounds
        float targetZoom = Mathf.Clamp(max, minZoom, maxZoom);
        // change the orthographicSize of the camera -- the zooming
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, targetZoom, ref zoomVelocity, zoomDampTime);
    }

    // find the bottom most part of the level and the bottom position for the camera based on it
    private float CalcBottomScene(){
        // add the camera size and the groundHeight
        float cameraBottom = cam.orthographicSize - groundHeight;
        return cameraBottom;
    }

    // find the left most part of the level and the left position for the camera based on it
    private float CalcLeftEdgeScene(){
        // add the camera size and the wallHeight
        float cameraBottom = (cam.orthographicSize * cam.aspect) + wallWidth;
        return cameraBottom;
    }

    // find the bottom most character and the bottom position for the camera based on it
    private float CalcBottomCharacter(){
        // find the lowest target y
        float boundsBottom = bounds.min.y;
        // add the camera size and the characterHeight
        float cameraBottom = boundsBottom + cam.orthographicSize - characterHeight;
        return cameraBottom;
    }

    // find the left most character and the left position for the camera based on it -- not calabrated
    private float CalcLeftEdgeCharacter(){
        // find the lowest target x
        float boundsLeftEdge = bounds.min.x;
        // add the camera size and the characterWidth
        float cameraBottom = -boundsLeftEdge - (cam.orthographicSize * - cam.aspect) - characterWidth;
        return cameraBottom;
    }

    // when the player collides with the door object it will move them to the next room
    public void MoveToNewRoom(Transform newRoom){
        transform.position = new Vector3(newRoom.position.x, newRoom.position.y, transform.position.z);
    }
}
