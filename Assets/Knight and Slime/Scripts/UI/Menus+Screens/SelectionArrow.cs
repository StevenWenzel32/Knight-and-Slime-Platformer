using UnityEngine;
using UnityEngine.UI;

public class SelectionArrow : MonoBehaviour
{
    // public
    public RectTransform[] options;
    // sound to play when the arrow moves
    public AudioClip changePosSound;
    // sound to play when an option is selected
    public AudioClip selectSound;

    // private
    private RectTransform rect;
    private int currentPos;

    private void Awake(){
        rect = GetComponent<RectTransform>();
    }

    private void Update(){
        // change the position of the arrow
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)){
            ChangePosition(-1);
        } else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)){
            ChangePosition(1);
        }

        // interacting with the options
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)){
            Interact();
        }
    }

    private void ChangePosition(int change){
        currentPos += change;

        if (change != 0){
            SoundManager.instance.PlaySound(changePosSound);
        }

        if (currentPos < 0){
            currentPos = options.Length - 1;
        } else if (currentPos > options.Length - 1){
            currentPos = 0;
        }

        // assign the y position of the curret option to the arrow
        rect.position = new Vector3(rect.position.x, options[currentPos].position.y, 0);
    }

    // for selecting or interacting with the options
    private void Interact(){
        SoundManager.instance.PlaySound(selectSound);
        // access the button componets
        options[currentPos].GetComponent<Button>().onClick.Invoke();
    }
}
