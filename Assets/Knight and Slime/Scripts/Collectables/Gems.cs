using Unity.VisualScripting;
using UnityEngine;

public class Gems : Collectible2D
{
    // score value if wanted
    public float scoreValue;
    // get gem counter
    private GemCounter gemCounter;

    protected override void Awake(){
        base.Awake();
        // looks for the first object in the hiearchy 
        gemCounter = FindObjectOfType<GemCounter>();
    }

    // when collected
    protected override void OnTriggerEnter2D(Collider2D other) {
        // check for hit with anything in player layer
        if (other.gameObject.layer == LayerMask.NameToLayer("Knight") || other.gameObject.layer == LayerMask.NameToLayer("Slime")) {
            // disable collectable
            gameObject.SetActive(false);
            // play sound
            SoundManager.instance.PlaySound(onCollectSound);
            // Instantiate the particle effect
 //           Instantiate(onCollectEffect, transform.position, transform.rotation);
            // up the gem count
            gemCounter.UpGemCount();
        }
        
    }
}
