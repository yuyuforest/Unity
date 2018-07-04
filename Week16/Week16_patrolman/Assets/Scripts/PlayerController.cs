using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
    
    private Animator playerAnimator;
    private Transform player;
    //遇到的其他物体/角色
    private Transform obstacle = null;
    //以什么方向遇上障碍物
    private Vector3 obstacleDirection = Vector3.zero;

    private IUserAction action;

    //是否死亡
    private bool isDead = false;
    private bool isRunning = false;
    [SyncVar]
    private bool over = false;  //游戏是否结束

    private static int count = 0;
    private int localID = 0;

	void Start () {
        action = SSDirector.getInstance().currentSceneController as IUserAction;
    }

    public override void OnStartLocalPlayer()
    {
        if (count == 2)
        {
            gameObject.SetActive(false);
            localID = 3;
            return;
        }
        localID = ++count;
        player = gameObject.transform;
        playerAnimator = gameObject.GetComponent<Animator>();
        if (player.position.z > 5) player.SetPositionAndRotation(player.position, Quaternion.AngleAxis(180, Vector3.up));
    }

    private void OnDestroy()
    {
        if (localID <= count)
        {
            count--;
            localID = 2;
        }
    }

    private void OnPlayerDisconnected(NetworkPlayer player)
    {
        if (localID <= count)
        {
            count--;
            localID = 2;
        }
    }

    private void OnDisconnectedFromServer(NetworkDisconnection info)
    {
        if (localID <= count)
        {
            count--;
            localID = 2;
        }
    }

    void Update ()
    {
        if (!isLocalPlayer) return;

        CmdGetInfo();

        //if (playerAnimator != null)
        playerAnimator.SetBool("IsRunning", isRunning);
        playerAnimator.SetBool("IsDead", isDead);

        if (over) return;

        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        movePlayer(hor * 0.03f, ver * 0.03f);
    }

    [Command]
    void CmdGetInfo()
    {
        if (!action.GameProcessing().Equals("")) over = true;
        //Debug.Log("server " + over);
    }

    //是否正对着障碍物
    private bool IsObstacleFront()
    {
        return obstacleDirection == player.forward;
    }

    //检测是否遇上障碍
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("wall"))
        {
            obstacleDirection = player.forward;
        }
        else if (other.tag.Equals("PatrolBody"))    //遇上巡逻兵，死亡
        {
            isDead = true;
            //playerAnimator.SetBool("IsDead", true);
        }
    }

    //离开障碍
    private void OnTriggerExit(Collider other)
    {
        obstacleDirection = Vector3.zero;
    }

    //移动玩家
    public void movePlayer(float hor, float ver)
    {
        if (isDead) return;
        if (ver != 0)
        {
            //如果玩家不是在奔跑，则调用奔跑的动画
            //if (!playerAnimator.GetBool("IsRunning")) playerAnimator.SetBool("IsRunning", true);
            if (playerAnimator != null && !playerAnimator.GetBool("IsRunning")) isRunning = true;

            if (ver > 0)
            {
                player.forward = Vector3.forward;
                if (IsObstacleFront()) return;  //如果玩家遇上了障碍，则不移动
                player.Translate(new Vector3(0, 0, ver));
            }
            else
            {
                player.forward = Vector3.back;
                if (IsObstacleFront()) return;
                player.Translate(new Vector3(0, 0, -ver));
            }
        }
        else if (hor != 0)
        {
            //if (!playerAnimator.GetBool("IsRunning")) playerAnimator.SetBool("IsRunning", true);
            if (!playerAnimator.GetBool("IsRunning")) isRunning = true;
            if (hor > 0)
            {
                player.forward = Vector3.right;
                if (IsObstacleFront()) return;
                player.Translate(new Vector3(0, 0, hor));
            }
            else
            {
                player.forward = Vector3.left;
                if (IsObstacleFront()) return;
                player.Translate(new Vector3(0, 0, -hor));
            }
        }
        //else playerAnimator.SetBool("IsRunning", false);    //没有移动，则不奔跑
        else isRunning = false;
    }
}
