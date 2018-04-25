### 视频

链接: https://pan.baidu.com/s/1y7VmpGzn9iUSC9bQLLAcIQ 

密码: dkmn

（只录制了物理学运动部分）



### 说明

- 上一版和本版飞碟游戏，因为使用了超过100M的资源，无法上传到github，所以只上传代码部分。


- 因为使用第一版飞碟游戏的自定义飞碟做物理学运动时，在爆炸、碰撞时出现了零件分离的情况，所以在本版改用了简单的圆柱体。



### 实现思路

#### 动作部分

新增了 ```PhysicalDiskAction``` 类，规定飞碟的物理学动作。（原有的 ```DiskAction``` 类规定飞碟的运动学动作。）

新增了 ```IActionParameter``` 接口，```DiskAction``` 和 ```PhysicalDiskAction```  都实现该接口，以便 ```RoundController``` 设置它们的参数。

```c#
public interface IActionParameter {
    void setParameter(Vector3 start, Vector3 velocity, float acceleration);
}

//PhysicalDiskAction的实现
public void setParameter(Vector3 start, Vector3 velocity, float acceleration)
{
    this.velocity = velocity;
    this.startingPoint = start;
    this.acceleration = new Vector3(0, - acceleration, 0);
}

//DiskAction的实现
public void setParameter(Vector3 start, Vector3 velocity, float acceleration)
{
    this.xSpeed = velocity.x;
    this.yUpSpeed = velocity.y;
    this.zSpeed = velocity.z;
    this.startingPoint = start;
    this.yDownSpeed = 0.5f * acceleration;
}

//RoundController中，在SetDiskActionParameters(IActionParameter)中调用接口的方法
public void sendDisk()  //发射飞碟
{
    GameObject go = diskFactory.GetDisk();
    IActionParameter action;

    if (physical)	//是物理学的设置，则使用物理学动作
      action = PhysicalDiskAction.GetPhysicalDiskAction() as IActionParameter;
    else
      action = DiskAction.GetDiskAction() as IActionParameter;

    //...

    SetDiskActionParameters(action);
    //...
}
public void SetDiskActionParameters(IActionParameter da)  //设置飞碟的速度、发射仰角等参数
{
    float xSpeed = Random.Range(xMinSpeed, xMaxSpeed);
    float zSpeed = Random.Range(0, 0.1f) * xSpeed;
    Vector3 startingPoint = new Vector3(Random.Range(xMinPos, xMaxPos), 0, zPos);
    double angle = (double)Random.Range(shootMinAngle, shootMaxAngle);
    float yUpSpeed = xSpeed * (float)(System.Math.Tan(angle * System.Math.PI / 180));
    if (Random.Range(-1.0f, 1.0f) < 0)  //决定飞碟从左边还是右边发射
    {
      	startingPoint.x = -startingPoint.x;
    }
    else
    {
      	xSpeed = -xSpeed;
    }
    float yDownSpeed = yUpSpeed * 0.2f;
    
  	//调用接口的方法给飞碟动作设置参数
    da.setParameter(startingPoint, new Vector3(xSpeed, yUpSpeed, zSpeed), yDownSpeed);
}
```



#### 动作管理器部分

新增了 ```PhysicalActionManager``` 类，管理飞碟的物理学动作。（原有的 ```DiskActionManager``` 类管理飞碟的运动学动作。）

新增了 ```IActionManager``` 接口，```DiskActioManager``` 和 ```PhysicalActionManager```  都实现该接口，以便 ```RoundController``` 调用它们以发射飞碟。

```c#
//接口定义
public interface IActionManager {
    void sendDisk(GameObject disk, IActionParameter da);
}

//PhysicalActionManager的实现
public void sendDisk(GameObject disk, IActionParameter da)
{
  	this.RunAction(disk, (PhysicalDiskAction)da, this);
}

//DiskActionManager的实现
public void sendDisk(GameObject disk, IActionParameter da)
{
  	this.RunAction(disk, (DiskAction)da, this);
}

//在RoundController中，会根据GUI的点击来决定使用哪种动作管理器
public void setActionManager(bool physical)
{
    this.physical = physical;
    if (physical)
    {
      twoActionsManager = gameObject.AddComponent<PhysicalActionManager>() as IActionManager;
    }
    else
    {
      twoActionsManager = gameObject.AddComponent<DiskActionManager>() as IActionManager;
    }
}

//GUI点击设置动作类型
private void OnGUI()
{
    //......
      if (GUI.Button(new Rect(200, 0, 300, 50), "Begin With Physical Action"))
      {
        action.setActionManager(true);
        action.BeginNewRound();
      }
      if (GUI.Button(new Rect(200, 70, 300, 50), "Begin With Kinematic Action"))
      {
        action.setActionManager(false);
        action.BeginNewRound();
      }
  	//......
}
```

