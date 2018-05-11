using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserAction {
	string GameProcessing();
    void movePlayer(float hor, float ver);
    int GetScore();
}
