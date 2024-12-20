using UnityEngine;

public class Removable : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    // turn off the object
    public void ToggleRemovable(){
        // if object is active 
        bool active = gameObject.activeSelf;
        // toggle it 
        gameObject.SetActive(!active);
    }
}
