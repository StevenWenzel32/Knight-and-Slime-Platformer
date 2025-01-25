using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class KnightController : PlayerControllerBase
{
    // very basic player until knight unique abilties are added in
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        // call the base collsion
        base.OnCollisionEnter2D(collision);
        // Debug.Log($"Knight collided with: {collision.gameObject.name}");
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
                GetComponent<Knight>().SetHasKey(true);
            }
        }
    }
}
