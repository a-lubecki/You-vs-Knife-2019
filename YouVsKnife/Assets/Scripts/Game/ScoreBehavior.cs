/**
 * You vs Knife
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

using System;
using UnityEngine;


public class ScoreBehavior : MonoBehaviour {


    [SerializeField] private int score = 0;
    [SerializeField] private GameObject goScoreListener = null;
    private IScoreListener listener {
        get {
            return goScoreListener.GetComponent<IScoreListener>();
        }
    }


    public int getScore() {
        return score;
    }

    public void resetScore() {
        setScore(0);
    }

    public void incrementScore() {
        setScore(score + 1);
    }

    public void setScore(int score) {

        if (score < 0) {
            throw new ArgumentException();
        }

        if (score == this.score) {
            //no need to notify if same score
            return;
        }

        this.score = score;

        listener?.onScoreChanged(this, score);
    }

    public void saveCurrentScoreIfMax() {

        var maxScore = getMaxScore();

        if (score > maxScore) {
            setMaxScore(score);
        }
    }

    public int getMaxScore() {
        return GameSaver.instance.data.maxScore;
    }

    public void setMaxScore(int maxScore) {

        if (maxScore < 0) {
            throw new ArgumentException();
        }

        GameSaver.instance.data.maxScore = maxScore;

        listener?.onMaxScoreChanged(this, maxScore);
    }

}

public interface IScoreListener {

    void onScoreChanged(ScoreBehavior knife, int score);

    void onMaxScoreChanged(ScoreBehavior knife, int maxScore);
}
