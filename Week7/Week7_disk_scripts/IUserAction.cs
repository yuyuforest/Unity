using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserAction
{
    string GameProcessing();
    void Click(GameObject clicked);
    int GetRound();
    int GetTrial();
    int GetScore();
    void BeginNewRound();
    void setActionManager(bool physical);
}
