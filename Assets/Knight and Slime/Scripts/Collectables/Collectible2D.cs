using UnityEngine;

public class Collectible2D : MonoBehaviour
{
    // shared rotation speed defintiley want to keep this here
    public float rotationSpeed = 0.5f;
    // for shared collect effects for all objects
    public GameObject onCollectEffect;
    // collect sound used for all collectables - eventually have each object have its own sound
    public AudioClip onCollectSound;

    protected virtual void Awake()
    {
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        transform.Rotate(0, rotationSpeed, 0);
    }

    public virtual void Collect(GameObject collector){
        // disable the object
        gameObject.SetActive(false);
        // play collect sound
        SoundManager.instance.PlaySound(onCollectSound);
        // play the collect partiacale effect 
//           Instantiate(onCollectEffect, transform.position, transform.rotation);
    }
}


