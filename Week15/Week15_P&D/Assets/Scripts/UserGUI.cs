using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserGUI : MonoBehaviour {

	private UserAction action;
    
	void Start () {
		action = SSDirector.getInstance ().currentSceneController as UserAction;
	}
	
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				action.Click (hit.collider.gameObject);
			}
		}
	}

	void OnGUI() {
		float width = Screen.width / 6;
		float height = Screen.height / 12;

        if (action.GameProcessing() == "noInput") return;

        

        if (action.GameProcessing() == "canGo")
        {
            if (GUI.Button(new Rect(Screen.width / 2 + width, 100, width, height), "Go!"))
            {
                action.Go();
            }

            if (GUI.Button(new Rect(Screen.width / 2, 100, width, height), "All up bank"))
            {
                action.AllUpBank();
            }
        }

        else if (action.GameProcessing() == "canNext")
        {
            if (GUI.Button(new Rect(Screen.width / 2 - width, 100, width, height), "Next"))
            {
                action.Next();
            }

            if (GUI.Button(new Rect(Screen.width / 2, 100, width, height), "All up bank"))
            {
                action.AllUpBank();
            }
        }

        else {
            string result = action.GameProcessing();

            if (GUI.Button (new Rect (Screen.width / 2 - width / 2, 100, width, height), "You " + result + "!\nClick here to restart:)")) {
                SceneManager.LoadScene("Scene", LoadSceneMode.Single);
			}
			if (GUI.Button (new Rect (Screen.width / 2 - width / 2, 200, width, height), "Quit")) {
				Application.Quit ();
			}
		}
	}
}
