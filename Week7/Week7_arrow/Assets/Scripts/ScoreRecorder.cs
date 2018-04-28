using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreRecorder : MonoBehaviour {

    private int score = 0;
    
	void Start () {
		
	}
	
	void Update () {
		
	}

    public void AddScore(int delta)
    {
        score += delta;
    }

    public int GetScore()
    {
        return score;
    }
}
