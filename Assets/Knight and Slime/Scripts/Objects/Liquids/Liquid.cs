using System.Runtime.CompilerServices;
using UnityEngine;

public class Liquid : MonoBehaviour
{
    // for shared collect effects for all objects
    public GameObject onAbsorbEffect;
    // collect sound used for all collectables - eventually have each object have its own sound
    public AudioClip onAbsorbSound;

    // private vars
    // the slime player
    protected Slime slime;

    protected virtual void Awake()
    {
        // looks for the first object in the hiearchy 
        slime = FindObjectOfType<Slime>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
    }

    // absorb the liquid
    public virtual void Absorb(GameObject absorber){
        // check if the Slime has already absorbed a liquid
        if (slime.GetLiquidAbsorbed()){
            // turn off all liquid granted abilties
            ClearAbilties();
        }

        // mark the slime has having absorbed a liquid
        slime.SetLiquidAbsorbed(true);
        
        // play absorb/eating animation 

        // change the slimes color
        ChangeColor(absorber);

        // play absorb sound
//        SoundManager.instance.PlaySound(onAbsorbSound);
        // play the collect partiacale effect 
//           Instantiate(onAbsrobEffect, transform.position, transform.rotation);
    }

    protected void ChangeColor(GameObject absorber){
        // get the liquids color
        Color liquidColor = gameObject.GetComponent<SpriteRenderer>().color;
        // set the opacity to solid 
        liquidColor.a = 1;
        // set the slimes color to the liquid color
        absorber.GetComponent<SpriteRenderer>().color = liquidColor;
    }

    // turns off/clears all the slimes liquid granted abilties
    protected void ClearAbilties(){
        slime.SetClimbWalls(false);
        slime.SetSuperJump(false);
    }
}


