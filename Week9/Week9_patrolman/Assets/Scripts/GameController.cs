using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour, SceneController, IUserAction
{
    PlayerController playerController;      //玩家控制者
    PatrolActionManager patrolActionManager;    //巡逻兵动作管理器
    PatrolFactory patrolFactory;                //巡逻兵工厂
    PatrolEventManager patrolEventManager;      //事件发布者（抓获成功和逃跑成功）

    GameObject[] walls;
    GameObject[] patrols;
    GameObject player;

    int score = 0;
    string gameState = "";

    void Awake()
    {
        SSDirector director = SSDirector.getInstance();
        director.setFPS(60);
        director.currentSceneController = this;
        director.currentSceneController.LoadResources();
    }

    void Start () {
		
	}
	
	void Update () {

	}

    //加载地图
    public void LoadMap()
    {
        walls = new GameObject[6];
        walls[0] = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Walls/Wall4_3"), new Vector3(2.5f, 0.5f, 2.5f), Quaternion.AngleAxis(180, Vector3.up));
        walls[1] = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Walls/Wall4_3"), new Vector3(7.5f, 0.5f, 2.5f), Quaternion.AngleAxis(90, Vector3.up));
        walls[2] = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Walls/Wall4_2_1"), new Vector3(12.5f, 0.5f, 2.5f), Quaternion.AngleAxis(180, Vector3.up));
        walls[3] = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Walls/Wall4_2_1"), new Vector3(2.5f, 0.5f, 7.5f), Quaternion.identity);
        walls[4] = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Walls/Wall4_3"), new Vector3(7.5f, 0.5f, 7.5f), Quaternion.AngleAxis(-90, Vector3.up));
        walls[5] = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Walls/Wall4_2_1"), new Vector3(12.5f, 0.5f, 7.5f), Quaternion.AngleAxis(90, Vector3.up));
    }

    //加载角色：玩家和巡逻兵
    public void LoadCharacters()
    {
        GameObject player = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Player"), new Vector3(2.5f, 0, -1), Quaternion.identity);
        playerController = player.AddComponent<PlayerController>();

        patrols = new GameObject[6];
        patrols[0] = patrolFactory.GetPatrol(new Vector3(1, 0.5f, 1), Quaternion.identity);
        patrols[1] = patrolFactory.GetPatrol(new Vector3(6, 0.5f, 4), Quaternion.AngleAxis(45, Vector3.up));
        patrols[2] = patrolFactory.GetPatrol(new Vector3(13, 0.5f, 2), Quaternion.AngleAxis(-60, Vector3.up));
        patrols[3] = patrolFactory.GetPatrol(new Vector3(4, 0.5f, 9), Quaternion.AngleAxis(180, Vector3.up));
        patrols[4] = patrolFactory.GetPatrol(new Vector3(8, 0.5f, 6), Quaternion.AngleAxis(90, Vector3.up));
        patrols[5] = patrolFactory.GetPatrol(new Vector3(11, 0.5f, 9), Quaternion.identity);
    }

    //加载资源：工厂，地图，角色，事件发布者
    public void LoadResources()
    {
        patrolFactory = PatrolFactory.getInstance();
        patrolActionManager = gameObject.AddComponent<PatrolActionManager>();

        LoadMap();
        LoadCharacters();

        foreach(GameObject patrol in patrols)
        {
            int angle = Random.Range(0, 120);
            patrolActionManager.RunAction(patrol, new PatrolAction(angle), patrolActionManager);
        }

        patrolEventManager = gameObject.AddComponent<PatrolEventManager>();
        PatrolEventManager.instance = patrolEventManager;
    }

    //返回游戏状态
    public string GameProcessing()
    {
        return gameState;
    }
    
    //注册事件处理程序
    public void OnEnable()
    {
        PatrolEventManager.OnHuntAction += hunt;
        PatrolEventManager.OnFleeAction += flee;
    }

    //注销事件处理程序
    public void OnDisable()
    {
        PatrolEventManager.OnHuntAction -= hunt;
        PatrolEventManager.OnFleeAction -= flee;
    }

    //玩家成功逃脱后的处理：加分
    void flee()
    {
        score += 5;
    }

    //玩家被抓到后的处理：结束游戏
    void hunt()
    {
        gameState = "Game over!";
        recycleAll();
    }

    //移动玩家
    public void movePlayer(float hor, float ver)
    {
        playerController.movePlayer(hor, ver);
    }

    //返回分数
    public int GetScore()
    {
        return score;
    }

    //回收动作
    public void recycleAll()
    {
        patrolActionManager.clear();
    }
}
