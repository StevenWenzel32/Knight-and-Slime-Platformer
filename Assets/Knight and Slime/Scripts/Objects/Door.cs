using UnityEngine;

public class Door : MonoBehaviour
{
    [Header ("Lock")]
    // the lock over the door
    public GameObject doorLock;

    private Animator anim;
    // will be changed inside a switch or button
    public bool action = false;
    // for the collider
    private BoxCollider2D boxCollider;
    

    // Start is called before the first frame update
    void Start()
    {
        // grab the animator on the door object
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
 
    // Update is called once per frame
    void Update()
    {
        // remove the lock
        doorLock.SetActive(!action);
        // play the open and close animation based on the value of open, open will be changed by a script in a switch or button
        anim.SetBool("Open", action);
        boxCollider.enabled = !action;

    }

    public void ToggleDoor(){
        action = !action;
    }
}
