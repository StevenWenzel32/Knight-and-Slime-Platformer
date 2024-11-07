using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    // make a singleton
    // only have one instance ever, can get else where but can only set here -- singleton
    public static ScoreCounter instance {get; private set;}

    [Header ("Gems")]
    public int gemValue;

    [Header ("Time (in secs)")]
    public Timer timer;
    public int timeModifier;
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
        playerTime = timer.timeToDisplay;
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
        StarRating();
        FinalScore();
    }

    // finish later -- not needed for class
    public void StarRating(){
        if (playerTime <= threeStarTime){
            // give three stars
            stars = 3;
//            LevelSelectManager.instance.UpdateStars(stars);
        } else if (playerTime <= twoStarTime){
            // give two stars
            stars = 2;
//            LevelSelectManager.instance.UpdateStars(stars);
        } else {
            // give one star
            stars = 1;
//            LevelSelectManager.instance.UpdateStars(stars);
        }
    }

    // adds time to score
    public void FinalScore(){
        // calc score from time
        if (playerTime <= threeStarTime){
            int bonus = (int)((threeStarTime - playerTime) * timeModifier);
            // check if the bonus is positive
            if (bonus > 0){
                score += bonus;
            }
        }
    }
}
