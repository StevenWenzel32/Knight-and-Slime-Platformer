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
        absorbable = true;
    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }

    // unique actions when collected
    public override void Collect(GameObject collector) {
        base.Collect(collector);
        // up the key count
        keyCounter.UpKeyCount();
    }
}
