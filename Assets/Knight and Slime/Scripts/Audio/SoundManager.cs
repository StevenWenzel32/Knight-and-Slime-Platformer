using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    // only have one instance ever, can get else where but can only set here -- singleton
    public static SoundManager instance {get; private set;}
    
    public AudioMixer audioMixer;

    private const string VolumeParameter = "MasterVolume";
    private const string VolumePrefKey = "musicVolume";

    private void Awake(){
        if (instance == null)
        {
            instance = this;
            // Persist SoundManager across scenes -- good for performance
            //DontDestroyOnLoad(gameObject);  
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // default to the max volume
        float savedVolume = PlayerPrefs.GetFloat(VolumePrefKey, 1.0f);
        SetVolume(savedVolume);
    }

     // SetVolume takes the value from the volume slider in the settings and changes the game volume accordingly
    public void SetVolume(float volume)
    {
        // then changes the mixers master volume to the new value
        audioMixer.SetFloat(VolumeParameter, (Mathf.Log10(volume) * 20));
    }

// UpdateSliderValue changes the text display of the sliders currant value
    public float GetVolume()
    {
        return PlayerPrefs.GetFloat(VolumePrefKey, 1.0f);
    }

    public void PlaySound(AudioClip sound){
        AudioSource source = GetComponent<AudioSource>();
        // play it only once
        source.PlayOneShot(sound);
    }
}
