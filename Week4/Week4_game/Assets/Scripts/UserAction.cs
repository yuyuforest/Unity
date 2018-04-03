using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface UserAction {

	// Use this for initialization
	string GameOver();
	void Click (GameObject clicked);
	void Go ();
	bool isShoreshide ();
}
