using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour {

    private IUserAction action;

    private Rect windRect = new Rect(0, 0, 50, 50);
    private Rect arrowNumRect = new Rect(0, 50, 50, 50);
    private Rect scoreRect = new Rect(0, 100, 50, 50);
    private const string tip = "按空格键->取新箭 按方向键->移动箭\n移动鼠标->旋转箭 点击鼠标->射箭";

    void Start()
    {
        action = SSDirector.getInstance().currentSceneController as IUserAction;
    }

    void Update()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        float deltaX = Input.GetAxis("Mouse X");//获取鼠标在X轴上的增量
        float deltaY = Input.GetAxis("Mouse Y");//获取鼠标在Y轴上的增量
        action.moveArrow(deltaX, deltaY, hor * 0.01f, ver * 0.01f);//移动弓箭


        if (Input.GetKeyDown(KeyCode.Space))
        {
            action.getArrow();
        }
        if (Input.GetMouseButtonDown(0))
        {
            action.sendArrow();
        }
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 20;
        style.normal.textColor = Color.white;

        string wind = action.GetWind();
        GUI.Label(windRect, wind, style);
        string arrow = "" + action.GetSendCounter() + "/" + action.GetMaxArrowNum();
        GUI.Label(arrowNumRect, "Arrows used: " + arrow, style);
        GUI.Label(scoreRect, "Score: " + action.GetScore(), style);


        if (action.GameProcessing().Equals("Over"))
        {
            GUI.Label(new Rect(300, 0, 200, 100), "Game Over!", style);
        }
        else
        {
            GUI.Label(new Rect(300, 0, 200, 100), tip, style);
        }
    }
}
