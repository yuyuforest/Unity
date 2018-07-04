using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolAction : SSAction {
    
    //每次遇上区域边界后的旋转角度
    public int rotateAngle;

    //检测巡逻兵状态，为PatrolAction提供改变的依据
    private PatrolController patrolController;

    public PatrolAction(int angle)
    {
        rotateAngle = angle;
    }
    
	public override void Start () {
        gameobject.transform.Rotate(gameobject.transform.up, rotateAngle);
        patrolController = gameobject.GetComponent<PatrolController>();
	}
	
	public override void Update () {
        if (destroy)
        {
            return;
        }
        if (patrolController.borderIsFront) //遇上边界则转向
        {
            gameobject.transform.Rotate(gameobject.transform.up, rotateAngle);
            patrolController.borderIsFront = true;
        }
        else if (patrolController.playerIsFront)    //遇上玩家，不移动，举枪攻击玩家
        {
            return;
        }
        else if (patrolController.isHunting)    //正在追击，则目标是玩家，要向着玩家移动
        {
            gameobject.transform.LookAt(patrolController.player);
            gameobject.transform.position = Vector3.MoveTowards(gameobject.transform.position, patrolController.player.position, 1.0f * Time.deltaTime);
        }
        else
        {   //以上情况都不是，进行普通的巡逻
            gameobject.transform.Translate(Vector3.forward * 1.0f * Time.deltaTime);
        }
	}
}
