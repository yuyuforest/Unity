using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundController : MonoBehaviour, SceneController, IUserAction {

    DiskFactory diskFactory;
    DiskActionManager diskActionManager;
    ScoreRecorder scoreRecorder;
    GameObject explosion;   //爆炸效果
    GameObject willBoom;    //将要爆炸的飞碟

    int round = 0;
    int trial = 0;
    int sendCounter = 0;          //计数发射飞碟数
    int timeCounter = 0;        //计时器
    int roundTimeLimit = 95;    //每局的trials之间的时间
    const int trialLimit = 5;   //每局限5个trial
    const int roundLimit = 5;   //限5局

    const float xMaxPos = 12.0f;        //飞碟发射的最大x位置
    const float xMinPos = 1.0f;         //飞碟发射的最小x位置
    const float zPos = -5.0f;           //飞碟发射的z位置
    const float shootMaxAngle = 50;     //飞碟发射的最大仰角（单位：°）
    const float shootMinAngle = 30;     //飞碟发射的最小仰角（单位：°）
    
    //飞碟发射的最大/小的x/z方向的速度
    float xMaxSpeed = 8.0f;             
    float xMinSpeed = 6.0f;
    float zMaxSpeed = 5.0f;
    float zMinSpeed = 3.0f;

    string state = "WillBegin";  //游戏状态，""表示正在进行，"Stop"表示一局完成后的暂停，"Over"表示游戏结束

    void Awake()
    {
        SSDirector director = SSDirector.getInstance();
        director.setFPS(60);
        director.currentSceneController = this;
        director.currentSceneController.LoadResources();
        diskFactory = new DiskFactory();
    }

    void Start () {
        diskFactory = gameObject.AddComponent<DiskFactory>();
        diskActionManager = gameObject.AddComponent<DiskActionManager>();
        diskActionManager.diskFactory = diskFactory;
        diskFactory.diskActionManager = diskActionManager;
        scoreRecorder = gameObject.AddComponent<ScoreRecorder>();
	}
	
	void Update () {
        if (!state.Equals("")) return;
        if (trial == trialLimit)
        {
            if (diskFactory.recycleCounter.Equals(sendCounter)) //只有飞碟都回收了，才能允许显示Stop按钮
            {
                if (round == roundLimit)
                {
                    state = "Over";
                }
                else
                {
                    state = "Stop";
                    xMaxSpeed += 1.0f;
                    xMinSpeed += 1.0f;
                    zMaxSpeed += 1.0f;
                    zMinSpeed += 1.0f;
                }
            }
            return;
        }
        timeCounter++;
        if (timeCounter.Equals(roundTimeLimit))
        {
            sendDisk();
            if(round > 1)   //若为第2/3局，一个trial内发射2个飞碟
            {
                sendDisk();
            }
            if (round > 3)   //若为第4、5局，一个trial内发射3个飞碟
            {
                sendDisk();
            }
            timeCounter = 0;
            trial++;
        }


	}

    public void sendDisk()  //发射飞碟
    {
        GameObject go = diskFactory.GetDisk();
        DiskAction da = DiskAction.GetDiskAction();
        
        SetDiskActionParameters(da);
        if (round > 3 && Random.Range(0, 1.0f) > 0.6f)  //在第4/5局随机设置特殊飞碟
            go.GetComponent<DiskData>().setBonus();
        diskActionManager.sendDisk(go, da);
        sendCounter++;
    }

    public void LoadResources()
    {

    }

    public void SetDiskActionParameters(DiskAction da)  //设置飞碟的速度、发射仰角等参数
    {
        da.xSpeed = Random.Range(xMinSpeed, xMaxSpeed);
        da.zSpeed = Random.Range(0, 0.1f) * da.xSpeed;
        da.startingPoint = new Vector3(Random.Range(xMinPos, xMaxPos), 0, zPos);
        double angle = (double)Random.Range(shootMinAngle, shootMaxAngle);
        da.yUpSpeed = da.xSpeed * (float)(System.Math.Tan(angle * System.Math.PI / 180));
        if (Random.Range(-1.0f, 1.0f) < 0)  //决定飞碟从左边还是右边发射
        {
            da.startingPoint.x = -da.startingPoint.x;
        }
        else
        {
            da.xSpeed = -da.xSpeed;
        }
        da.yDownSpeed = da.yUpSpeed * 0.1f;
    }

    //返回游戏状态
    public string GameProcessing()
    {
        return state;
    }

    public void Click(GameObject clicked)
    {
        //若点击飞碟，获得的会是子对象，需要获取父对象（即整个飞碟）
        if (clicked.transform.parent != null) willBoom = clicked.transform.parent.gameObject;
        else return;
        explosion = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Explosion"));
        explosion.transform.position = willBoom.transform.position;
        willBoom.SetActive(false);
        //Debug.Log("willboom " + willBoom.name);

        scoreRecorder.AddScore(willBoom.GetComponent<DiskData>().score);
        Invoke("BoomAndDisappear", 0.3f);   //爆炸延时显示0.3秒
    }

    private void BoomAndDisappear()     //爆炸消失
    {
        explosion.SetActive(false);
        Destroy(explosion);
    }

    public void BeginNewRound()     //开始新一局
    {
        state = "";
        timeCounter = 0;
        round++;
        trial = 0;
        roundTimeLimit -= 3;    //减少一局之内的trial的间隔
    }


    public int GetRound()
    {
        return round;
    }

    public int GetTrial()
    {
        return trial;
    }

    public int GetScore()
    {
        return scoreRecorder.GetScore();
    }
}
