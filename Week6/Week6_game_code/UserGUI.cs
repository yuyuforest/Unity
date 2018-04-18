using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour {

    private IUserAction action;

    private Rect roundRect = new Rect(0, 0, 50, 50);
    private Rect trialRect = new Rect(0, 50, 50, 50);
    private Rect scoreRect = new Rect(0, 100, 50, 50);

    void Start()
    {
        action = SSDirector.getInstance().currentSceneController as IUserAction;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {

                //Debug.Log("click1");
                action.Click(hit.collider.gameObject);
            }
        }
    }

    private void OnGUI()
    {
        GUI.Label(roundRect, "Round " + action.GetRound());
        GUI.Label(trialRect, "Trial " + action.GetTrial());
        GUI.Label(scoreRect, "Score " + action.GetScore());

        if (action.GameProcessing().Equals("Over"))
        {
            GUI.Label(new Rect(200, 0, 200, 100), "Game Over!");
        }
        else if (action.GameProcessing().Equals("Stop"))
        {
            if(GUI.Button(new Rect(200, 0, 200, 100), "New Round"))
            {
                action.BeginNewRound();
            }
        }
        else if (action.GameProcessing().Equals("WillBegin"))
        {
            if (GUI.Button(new Rect(200, 0, 200, 100), "Begin"))
            {
                action.BeginNewRound();
            }
        }
    }
}
