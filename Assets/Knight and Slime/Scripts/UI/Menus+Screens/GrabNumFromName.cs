using TMPro;
using UnityEngine;

public class GrabNumFromName : MonoBehaviour
{
    public TMP_Text text;
    void Awake()
    {
        // grab the object # from the object name
        int num = ParseName(gameObject.name);
        text.text = "" + num;
    }

    public int ParseName(string objectName){
        string[] splitName = objectName.Split(" ");
        return int.Parse(splitName[1]);
    }
}
