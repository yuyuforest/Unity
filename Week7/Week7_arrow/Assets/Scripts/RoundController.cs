using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundController : MonoBehaviour, SceneController, IUserAction {
    ArrowFactory arrowFactory;
    PhysicalActionManager actionManager;
    ScoreRecorder scoreRecorder;
    GameObject currentArrow = null;    //已选取的并且将要射出的箭
    GameObject target = null;
    GameObject windTrigger = null;      //生成风
    WindController windController = null;

    int sendCounter = 0;        //计数射箭数
    private const int maxArrowNum = 10;
   

    string state = "WillBegin";  //游戏状态，""表示正在进行，"Over"表示游戏结束

    void Awake()
    {
        SSDirector director = SSDirector.getInstance();
        director.setFPS(60);
        director.currentSceneController = this;
        director.currentSceneController.LoadResources();
    }

    void Start () {
	}

    void FixedUpdate()
    {
        
    }
	
	void Update () {
        
	}

    public void LoadResources()
    {
        arrowFactory = gameObject.AddComponent<ArrowFactory>();
        scoreRecorder = gameObject.AddComponent<ScoreRecorder>();
        actionManager = gameObject.AddComponent<PhysicalActionManager>();
        windTrigger = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/WindTrigger"), new Vector3(0, 3, -15), Quaternion.identity);
        windController = windTrigger.GetComponent<WindController>();

        target = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Target"), Vector3.zero, Quaternion.identity);
        Transform[] all = target.GetComponentsInChildren<Transform>();
        foreach(Transform child in all)
        {
            Debug.Log("child " + child.gameObject.name);
            //获取的子物体也包括父物体自己
            if(child.gameObject.name != "Target(Clone)") child.gameObject.GetComponent<TargetController>().recorder = scoreRecorder;
        }
    }

    //返回游戏状态
    public string GameProcessing()
    {
        return state;
    }

    public int GetScore()
    {
        return scoreRecorder.GetScore();
    }

    public int GetSendCounter()
    {
        return sendCounter;
    }

    public int GetMaxArrowNum()
    {
        return maxArrowNum;
    }

    public string GetWind()
    {
        return windController.getWindDescription();
    }

    public void getArrow()
    {
        if (sendCounter >= maxArrowNum) return;
        if (currentArrow != null) return;
        currentArrow = arrowFactory.GetArrow();
        windController.resetWind();
    }

    public void sendArrow()
    {
        if (currentArrow == null)
        {
            return;
        }
        else {
            currentArrow.GetComponent<Rigidbody>().isKinematic = false;
            PhysicalArrowAction shootAction = PhysicalArrowAction.GetPhysicalArrowAction();
            actionManager.RunAction(currentArrow, shootAction, actionManager);
            currentArrow = null;
            sendCounter++;
            if (sendCounter == maxArrowNum) state = "Over";
        }
    }

    public void moveArrow(float deltaX, float deltaY, float hor, float ver)
    {
        if (currentArrow == null) return;
        currentArrow.transform.Translate(new Vector3(hor, ver, 0));
        currentArrow.transform.Rotate(Vector3.left, deltaY);
        currentArrow.transform.Rotate(Vector3.up, deltaX);
    }
}
