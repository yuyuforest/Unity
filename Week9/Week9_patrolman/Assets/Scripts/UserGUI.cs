using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserGUI : MonoBehaviour {

	private IUserAction action;
    private GUIStyle style;
    
	void Start () {
		action = SSDirector.getInstance ().currentSceneController as IUserAction;
        style = new GUIStyle();
        style.fontSize = 20;
        style.normal.textColor = Color.white;
	}
	
	void Update () {

	}

	void OnGUI() {
		float width = Screen.width / 6;
		float height = Screen.height / 12;

        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        action.movePlayer(hor * 0.02f, ver * 0.02f);

        if (!action.GameProcessing().Equals(""))
        {
            GUI.Label(new Rect(0, 50, width, height), "Game over!", style);
        }

        GUI.Label(new Rect(0, 0, 50, 50), "Score : " + action.GetScore(), style);
    }
}
