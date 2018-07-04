using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class UserGUI : NetworkBehaviour {

	private IUserAction action;
    private GUIStyle style;

    [SyncVar]
    private string state = "";
    [SyncVar]
    private int score = 0;

    public override void OnStartLocalPlayer()
    {
        action = SSDirector.getInstance().currentSceneController as IUserAction;
        style = new GUIStyle();
        style.fontSize = 20;
        style.normal.textColor = Color.white;
    }

    void Start () {
		action = SSDirector.getInstance ().currentSceneController as IUserAction;
        style = new GUIStyle();
        style.fontSize = 20;
        style.normal.textColor = Color.white;
	}
	
	void Update () {
        CmdGetInfo();
        //Debug.Log("client invoke");
    }
    
    [Command]
    void CmdGetInfo()
    {
        state = action.GameProcessing();
        score = action.GetScore();
        //Debug.Log("server " + state + " " + score);
    }

	void OnGUI() {
        if (!isLocalPlayer) return;
		float width = Screen.width / 6;
		float height = Screen.height / 12;

        if (!state.Equals(""))
        {
            GUI.Label(new Rect(0, 50, width, height), "Game over!", style);
        }

        GUI.Label(new Rect(0, 0, 50, 50), "Score : " + score, style);
    }
}
