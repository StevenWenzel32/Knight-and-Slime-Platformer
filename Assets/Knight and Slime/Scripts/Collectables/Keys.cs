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

    // unique actions when collected
    public override void Collect(GameObject collector) {
        base.Collect(collector);
        // up the key count
        keyCounter.UpKeyCount();
    }

    public override void Absorb(GameObject absorber){
        // ref the slime
        Slime slime = absorber.GetComponent<Slime>();
        // check if the slime has space to absorb another item
        if (slime.CheckItemSpace()){
            base.Absorb(absorber);
            // display a key inside the slime
            
            // up the slimes item count
            slime.UpItemsAbsorbed();
            // up the slimes key count
            slime.UpKeyCount();
        }
        
    }
}
