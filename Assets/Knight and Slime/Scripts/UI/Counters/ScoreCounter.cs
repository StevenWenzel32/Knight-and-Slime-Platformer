using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    // make a singleton
    // only have one instance ever, can get else where but can only set here -- singleton
    public static ScoreCounter instance {get; private set;}

    [Header ("Gems")]
    public int gemValue;

    [Header ("Time Score Nums")]
    // the point pool that the time is subtracted from
    public int basePts = 500;
    // what the time conversion is multipled by 
    public int multiplier = 1;


    [Header ("Time (in secs)")]
    public Timer timer;
    public float threeStarTime;
    public float twoStarTime;

    // private vars
    private TMP_Text text;

    // vars for filling in levelInfo at level complete
    public int score = 0;
    public float playerTime = 0;
    public int stars = 0;


    private void Awake()
    {
        // display the current score
        text = GetComponent<TMP_Text>();
        DisplayScore();
        // singleton stuff
        if (instance == null){
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void DisplayScore(){
        text.text = "Score: " + score;
    }

    // live gem score update in game -- linear growth currently but may change to expontial to motivate players to get them all
    public void UpdateScore(){
        // calc score from gems
        score += gemValue;
        DisplayScore();
    }

    // to be called by level manager when level is completed
    public void EndOfLevelCalc(){
        // grab the time
        playerTime = timer.timeToDisplay;
        StarRating();
        FinalScore();
    }

    // finish later -- not needed for class
    public void StarRating(){
        if (playerTime <= threeStarTime){
            // give three stars
            stars = 3;
        } else if (playerTime <= twoStarTime){
            // give two stars
            stars = 2;
        } else {
            // give one star
            stars = 1;
        }
    }

    // calc bonus points based on time
    public void FinalScore(){
        // grab the players time and convert into a num
        float timeConversion = playerTime;
        Debug.Log("time = " + playerTime);
        // calc the bonus
        int bonus = (int)(basePts - (timeConversion * 1));
        // check if the bonus is positive
        if (bonus > 0){
            score += bonus;
        }
    }
}
