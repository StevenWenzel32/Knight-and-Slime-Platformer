using UnityEngine;

public class Collectible2D : MonoBehaviour
{
    // shared rotation speed defintiley want to keep this here
    public float rotationSpeed = 0.5f;
    // for shared collect effects for all objects
    public GameObject onCollectEffect;
    // for shared absorb effects for all objects
    public GameObject onAbsorbEffect;
    // collect sound used for all collectables - eventually have each object have its own sound
    public AudioClip onCollectSound;
    // for objects that can be absorbed  - eventually have each object have its own sound
    public AudioClip onAbsorbSound;
    // if the slime can eat/absorb it 
    public bool absorbable = false;

    protected virtual void Awake()
    {
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        transform.Rotate(0, rotationSpeed, 0);
    }

    // what to do when it's collected by a player
    public virtual void Collect(GameObject collector){
        // disable the object
        gameObject.SetActive(false);
        // play collect sound
        SoundManager.instance.PlaySound(onCollectSound);
        // play the collect partiacale effect 
//           Instantiate(onCollectEffect, transform.position, transform.rotation);
    }

    // what to do when it's absorbed by the slime
    public virtual void Absorb(GameObject collector){
        // disable the object
        gameObject.SetActive(false);
        // play absorb sound
//        SoundManager.instance.PlaySound(onAbsorbSound);
        // play the collect partiacale effect 
//           Instantiate(onAbsorbEffect, transform.position, transform.rotation);
    }
}


