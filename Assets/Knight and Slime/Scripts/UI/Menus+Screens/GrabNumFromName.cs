using TMPro;
using UnityEngine;

public class GrabNumFromName : MonoBehaviour
{
    public TMP_Text text;
    void Awake()
    {
        // display the object num in text
        text.text = "" + GetObjectNum();
    }

    public int GetObjectNum(){
        return ParseName(gameObject.name);
    }

    public int ParseName(string objectName){
        string[] splitName = objectName.Split(" ");
        return int.Parse(splitName[1]);
    }
}
