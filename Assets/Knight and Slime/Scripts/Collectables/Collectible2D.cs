using UnityEngine;

public class Collectible2D : MonoBehaviour
{

    public float rotationSpeed = 0.5f;
    public GameObject onCollectEffect;
    public AudioClip onCollectSound;
    protected MenuManager menuManager;

    protected virtual void Awake()
    {
        // looks for the first object in the hiearchy 
        menuManager = FindObjectOfType<MenuManager>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        transform.Rotate(0, rotationSpeed, 0);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other) {
         // Check if the other object has a Player tag
        if (other.CompareTag("Player")) {
            // disable collectable
            gameObject.SetActive(false);
            // play sound
            SoundManager.instance.PlaySound(onCollectSound);
            // Instantiate the particle effect
 //           Instantiate(onCollectEffect, transform.position, transform.rotation);
        }
    }
}


