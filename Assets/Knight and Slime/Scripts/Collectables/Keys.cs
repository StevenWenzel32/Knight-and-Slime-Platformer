using Unity.VisualScripting;
using UnityEngine;

public class Keys : Collectible2D
{
    // get key counter
    private KeyCounter keyCounter;

    protected override void Awake(){
        base.Awake();
        // looks for the first object in the hiearchy 
        keyCounter = FindObjectOfType<KeyCounter>();
    }

    // unique actions when collected
    protected override void Collect() {
        base.Collect();
        // up the key count
        keyCounter.UpKeyCount();
    }
}
