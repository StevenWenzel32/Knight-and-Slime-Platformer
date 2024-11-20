using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    [Header ("Video")]
    public static VideoManager instance;
    public VideoPlayer videoPlayer;
    public Canvas videoCanvas;

    [Header ("Music")]
    // current bgm
    public AudioSource bgm;
    // music to play during video clip
    public AudioSource videoMusic;
    // duration of the cross fade
    public float crossfadeDuration = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null){
            instance = this;
        } else {
            Destroy(gameObject);
        }
        // start with the video canvas hidden
        videoCanvas.enabled = false;     
        // subscribe to event 
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    public void PlayVideo(){
        // start cross fading the music
        //StartCoroutine(CrossfadeMusic(bgm, videoMusic));
        // just pause the music for now
        if (bgm.isPlaying)
        {
            bgm.Stop(); // Pauses the background music
        }
        videoMusic.Play();

        // show the canvas
        videoCanvas.enabled = true;
        // start playing the video
        videoPlayer.Play();
    }

    // coroutine to cross fade the music
    private IEnumerator CrossfadeMusic(AudioSource from, AudioSource to){
        float time = 0f;

        // check if there is music to transition to
        if (to != null){
            // make sure the new music starts at a low volume
            to.volume = 0f;
            to.Play();
        }
        
        // fade out the current and fade in the new
        while (time < crossfadeDuration){
            // update the time
            time += Time.deltaTime;
            // update the normalized time
            float normalizedTime = time / crossfadeDuration;

            // check for current music
            if (from != null){
                // fade out
                from.volume = Mathf.Lerp(1f, 0f, normalizedTime);
            }
            if (to != null){
                // fade in 
                to.volume = Mathf.Lerp(0f, 1f, normalizedTime);
            }
            yield return null;
        }
        // stop the bgm after fade out
        from.Pause();
    }

    public bool IsPlaying()
    {
        return videoPlayer.isPlaying;
    }

    private void OnVideoEnd(VideoPlayer vp){
        // start cross fading the music to bgm
        //StartCoroutine(CrossfadeMusic(videoMusic, bgm));
        if (videoMusic.isPlaying)
        {
            videoMusic.Pause(); // Pause video music
        }
        bgm.Play();

        // hide the canvas
        videoCanvas.enabled = false;
    }
}
