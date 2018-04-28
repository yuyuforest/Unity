using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserAction
{
    string GameProcessing();
    int GetScore();
    void getArrow();
    void moveArrow(float deltaX, float deltaY, float hor, float ver);
    void sendArrow();
    int GetSendCounter();
    int GetMaxArrowNum();
    string GetWind();
}
