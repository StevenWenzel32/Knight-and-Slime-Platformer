using UnityEngine;

public class Collectible2D : MonoBehaviour
{
    public float rotationSpeed = 0.5f;
    public GameObject onCollectEffect;
    public AudioClip onCollectSound;

    protected virtual void Awake()
    {
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        transform.Rotate(0, rotationSpeed, 0);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other) {
         // Check if the other object is in player layer
        if (other.gameObject.layer == LayerMask.NameToLayer("Slime") || other.gameObject.layer == LayerMask.NameToLayer("Slime")) {
            // disable collectable
            gameObject.SetActive(false);
            // play sound
            SoundManager.instance.PlaySound(onCollectSound);
            // Instantiate the particle effect
 //           Instantiate(onCollectEffect, transform.position, transform.rotation);
        }
    }
}


