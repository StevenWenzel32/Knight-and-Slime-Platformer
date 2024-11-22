using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform previousRoom;
    public Transform nextRoom;
    public Controller cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.layer == LayerMask.NameToLayer("Knight") || collision.gameObject.layer == LayerMask.NameToLayer("Slime")){
            if (collision.transform.position.x < transform.position.x){
                cam.MoveToNewRoom(nextRoom);
            } else {
                cam.MoveToNewRoom(previousRoom);
            }
        }
    }
}
