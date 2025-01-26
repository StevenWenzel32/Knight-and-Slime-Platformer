using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class KnightController : PlayerControllerBase
{
    // private vars
    Knight knight;

    protected override void Start(){
        base.Start();
        // ref the knight 
        knight = GetComponent<Knight>();
    }

    // very basic player until knight unique abilties are added in
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        // call the base collsion
        base.OnCollisionEnter2D(collision);
        // Debug.Log($"Knight collided with: {collision.gameObject.name}");

        // ref the door
        Door door = collision.gameObject.GetComponent<Door>();
        // check if its a door
        if (door != null){
            // check if Knight has a key
            if (knight.GetHasKey()){
                // open the door
                door.ToggleDoor();
                // use up key
                KeyCounter.instance.DownKeyCount();
            }
        }
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        Collectible2D collectible = other.GetComponent<Collectible2D>();
        // collectables for the knight
        if (collectible != null){
            // call the objects collect func
            collectible.Collect(gameObject);

            // for keys
            if (collectible.CompareTag("Key")){
                // mark that the knight has a key
                knight.SetHasKey(true);
            }
        }
    }
}
