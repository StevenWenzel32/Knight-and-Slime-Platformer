using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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

    // private vars
    private bool promptDisplayed = false;
    private string promptKeyword = "ctrl";

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
    public virtual void Absorb(GameObject absorber){
        // ref the slime
        Slime slime = absorber.GetComponent<Slime>();
        // check if the slime has space to absorb another item
        if (slime.CheckItemSpace()){
            // disable the object
            gameObject.SetActive(false);
            // ref the object as a collectable 
            Collectible2D item = gameObject.GetComponent<Collectible2D>();
            // put object in slimes inventory
            slime.itemsAbsorbed.Add(item);
            // up the slimes item count
            slime.UpItemsAbsorbed();
            slime.AddToInventoryUI(item);
            // use the object tag to display the object inside the slime
            ChangeDisplayInSlime(absorber, gameObject.tag, true);
            // play absorb sound
    //        SoundManager.instance.PlaySound(onAbsorbSound);
            // play the collect partiacale effect 
    //           Instantiate(onAbsorbEffect, transform.position, transform.rotation);
        }
    }

    // turn on the icon in the slime with the same tag given
    public void ChangeDisplayInSlime(GameObject absorber, string tag, bool state){
        Debug.Log("Inside display func");
        // search through the children
        foreach (Transform child in absorber.transform.Find("Inventory").transform){
            // if child has the tag
            if (child.CompareTag(tag)){
                Debug.Log("found the object");
                // turn the icon on
                child.gameObject.SetActive(state);
                break;
            }
        }
    }

    // toggle the prompt for this object
    public void TogglePrompt(GameObject absorber){
        // toggle
        promptDisplayed = !promptDisplayed;
        // search through the children in the prompt container
        foreach (Transform child in absorber.transform.Find("Prompts").transform){
            if (child.CompareTag(promptKeyword)){
                Debug.Log("found the prompt");
                // turn the prompt on
                child.gameObject.SetActive(promptDisplayed);
                break;
            }
        }
    }
}


