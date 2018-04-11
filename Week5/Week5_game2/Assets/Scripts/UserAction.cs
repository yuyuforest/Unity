using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface UserAction {
	string GameProcessing();
	void Click (GameObject clicked);
	void Go ();
}
