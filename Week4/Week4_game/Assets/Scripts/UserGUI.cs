using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserGUI : MonoBehaviour {

	private UserAction action;

	private bool noGo = false;

	// Use this for initialization
	void Start () {
		action = SSDirector.getInstance ().currentSceneController as UserAction;
	}
	
	// Update is called once per frame
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

		if (action.GameOver () != "") {
			noGo = true;
			if (GUI.Button (new Rect (Screen.width / 2 - width / 2, 100, width, height), "You " + action.GameOver () + "!\nClick here to restart:)")) {
				SceneManager.LoadScene (0);
			}
			if (GUI.Button (new Rect (Screen.width / 2 - width / 2, 200, width, height), "Quit")) {
				Application.Quit ();
			}
		}

		if (action.isShoreshide () && !noGo) {
			if (GUI.Button (new Rect (Screen.width / 2 - width / 2, 100, width, height), "Go!")) {
				action.Go ();
			}
		}
	}
}
