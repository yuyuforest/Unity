## 四、游戏对象与图形基础

### 1. 操作与总结

#### （1）参考 Fantasy Skybox FREE 构建自己的游戏场景 

因为在Assets Store找不到Fantasy Skybox FREE，所以使用了SkySerie Freebie中的6SizedFluffball。效果见下面的gif。（水面的构造使用了unity内置资源。）



#### （2）写一个简单的总结，总结游戏对象的使用

- 游戏对象是游戏场景中操作的基本对象。
- 可以通过鼠标在Unity编辑器里添加游戏对象，也可以在代码里实例化。
- 每个游戏对象都必有一个transform组件，记录游戏对象的大小、位置、旋转度等基本信息。
- 游戏对象可以添加多种组件，为游戏对象添加不同的特性或效果。如：添加纹理、碰撞器等等。
- 可以在Unity编辑器里的Inspector修改游戏对象组件。
- 可以把游戏对象制作成预制，存储起来，以便以后重复使用。



### 2. 编程实践：牧师与魔鬼——动作分离版

灰色正方体代表魔鬼，白色球体代表牧师。

![Week5_game](Week5_game_2_1.gif)



使用了老师提供的动作分离架构。

该架构的概览如下：

```c#
public interface ISSActionCallback;
//动作与动作管理者之间的接口，可以被动作（SSAction及其子类）包含
//可以被动作管理者实现，实现事件调度(接受所管理的动作的通知，比如动作已完成，或动作要开始了，同时可传递某些参数)

public class SSAction : ScriptableObject;
//动作基类
//包括了gameobject（来绑定到某个游戏对象）和callback（该动作的管理者）
//SSAction类对象可通过调用callback.SSActionEvent(...)来告诉管理者该动作的状态，让管理者进行相关处理
//比如CCMoveToAction，在Update里，到达目的地后调用callback.SSActionEvent()，告诉管理者动作结束

public class CCMoveToAction : SSAction;
//继承了SSAction动作基类，定义一个更具体的动作类型
//包括目的地和速度参数

public class CCSequenceAction : SSAction, ISSActionCallback;
//组合动作，用于执行一组动作
//可以作为动作类型，也可以视作一个动作管理器（实现了ISSActionCallback接口）
//它是被它从SSAction继承的callback所管理的动作，也是它内部列表包含的动作的管理器

public class SSActionManager : MonoBehaviour;
//动作管理器基类
//检查动作的运行/销毁状态，在作为MonoBehaviour类自带的Update()里调用所管理的每个要执行的动作内部定义的Update()，执行动作

public class CCActionManager : SSActionManager, ISSActionCallback;
//对SSActionManager根据实际用途的具体化，是一个针对特定场景的动作管理器
//因此需要实现ISSActionCallback接口，明确对动作对象开始或结束后做什么处理
```



以本次要实现的牧师和魔鬼游戏为例分析该架构的具体应用：

- FirstController对象包含了一个CCActionManager类对象
- CCActionManager包含一个CCMoveToAction类的对象，用于实现船和角色在两个河岸之间的简单直线移动
- CCActionManager包含一个CCSequenceAction类的对象，用于实现角色在船和河岸之间的折线移动
- CCActionManager向CCSequenceAction提供三个动作，构建动作列表：上升（rise），在空中水平移动（horizontal），下降着陆（fall）
- FirstController会获取可去往的目的地和速度，传递给CCActionManager调用相关移动函数



这六个类的具体代码如下：

```c#
public enum SSActionEventType : int { Started, Completed }

//动作与动作管理者之间的接口，可以被动作（SSAction及其子类）包含
//可以被动作管理者实现，实现事件调度(通知动作已完成，或动作要开始了，同时可传递某些参数)
public interface ISSActionCallback
{
    void SSActionEvent(SSAction source, 
        SSActionEventType events = SSActionEventType.Completed,
        int intParam = 0, string strParam = null, Object objectParam = null);
}
```



```c#
//动作基类
public class SSAction : ScriptableObject {

    public bool enable = true;          //是否可以进行该动作
    public bool destroy = false;        //是否要销毁动作，SSActionManager及其子类会检查这个参数，从而决定是否update
    public GameObject gameobject;       //该动作的对象
    public Transform transform;         //动作对象的transform属性
    public ISSActionCallback callback;  //相当于该动作的管理者（ActionManager或者SequenceAction）
    //SSAction类对象可通过调用callback.SSActionEvent(...)来告诉管理者该动作的状态，让管理者进行相关处理
    //比如CCMoveToAction，在Update里，到达目的地后调用callback.SSActionEvent()，告诉管理者动作结束

    //使用protected防止开发者自己new一个动作
    protected SSAction();
    public virtual void Start();
    public virtual void Update();
}
```



```c#
//继承了SSAction动作基类，定义一个更具体的动作类型
public class CCMoveToAction : SSAction {

    public Vector3 target;        //目的地
    public float speed;           //速度

    //防止开发者自己new一个动作
    protected CCMoveToAction()
    {

    }

    public override void Start()
    {
        //通知动作管理者，动作即将开始
        this.callback.SSActionEvent(this, SSActionEventType.Started);
    }

    //该动作具体的运动实现
    public override void Update()
    {
        //运动代码
        this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);

        //到达目的地
        if (this.transform.position == target)
        {
            this.destroy = true;                    //动作要被销毁
            this.callback.SSActionEvent(this);      //通知动作管理者该动作结束
        }
        else this.callback.SSActionEvent(this, SSActionEventType.Started);
    }


    //开发者只能使用该函数来新建一个动作
    public static CCMoveToAction GetSSAction(Vector3 target, float speed)
    {
        CCMoveToAction action = ScriptableObject.CreateInstance<CCMoveToAction>();  //让unity自己创建动作类，确保内存正确回收
        action.target = target;
        action.speed = speed;
        return action;
    }
}
```



```CCSequenceAction```是```CCActionManager```的动作，又是它自己包含的```List<SSAction>```中每个动作的动作管理器。

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//组合动作，用于执行一组动作
//可以作为动作类型，也可以视作一个动作管理器（实现了ISSActionCallback接口）
public class CCSequenceAction : SSAction, ISSActionCallback {

    public List<SSAction> sequence;
    public int repeat = -1; //表示重复的次数，如果为-1，则无限循环执行动作
    public int start = 0;   //作为索引来获取sequence中需要执行的动作，这个类的update方法会调用sequence[start].Update()

    //开发者通过该函数来新建一个CCSequenceAction
    public static CCSequenceAction GetSSAcition(int repeat, int start, List<SSAction> sequence, ISSActionCallback callback = null)
    {
        CCSequenceAction action = ScriptableObject.CreateInstance<CCSequenceAction>();
        action.repeat = repeat;
        action.sequence = sequence;
        action.start = start;
        action.callback = callback;
        return action;
    }

    //执行start指示的队列中的动作
    public override void Update()
    {
        if (sequence.Count == 0 || repeat == 0)
        {
            return;
        }
        if (start < sequence.Count)
        {
            sequence[start].Update();
            this.callback.SSActionEvent(this, SSActionEventType.Started);
        }
    }

    //动作类的对象source调用callback.SSActionEvent()，CCSequenceAction要根据source的状态来决定进行什么处理
    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Completed,
        int intParam = 0, string strParam = null, Object objectParam = null)
    {
        if (events == SSActionEventType.Started) return;
        source.destroy = false; //可能还要进行循环，暂时不销毁（但该动作可能在通知管理器前就将destroy置true了，比如CCMoveToAction,所以要重置false）
        this.start++;           //events默认为动作结束，所以start往后推一位，选定下一个动作
        if (this.start >= sequence.Count)
        {
            if (repeat > 0) repeat--;
            this.start = 0;     //start归零，repeat减1，表示又执行了一次循环
            if (repeat == 0)    //重复次数已用完
            {
                this.destroy = true;                //该组合动作需要销毁
            }
        }
    }

    public override void Start()
    {
        this.callback.SSActionEvent(this, SSActionEventType.Started);
        foreach (SSAction action in sequence)
        {
            action.gameobject = this.gameobject;
            action.transform = this.transform;
            action.callback = this;
            action.Start();
        }
    }

    void OnDestroy()
    {
        
    }
}
```



```c#
//动作管理器基类
//检查动作的运行/销毁状态，在作为MonoBehaviour类自带的Update()里调用所管理的每个要执行的动作内部定义的Update()，执行动作
public class SSActionManager : MonoBehaviour {
    private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();    //要执行的动作
    private List<SSAction> waitingAdd = new List<SSAction>();     //等待加入actions队列的动作
    private List<int> waitingDelete = new List<int>();  //存放从actions队列里移出的动作，等待删除

    protected void Start () {
		
	}
	
	protected void Update () {
        //将waitingAdd里的所有动作移入actions
        foreach (SSAction ac in waitingAdd)
            actions[ac.GetInstanceID()] = ac;
        waitingAdd.Clear();

        //检查actions里的动作，如需要销毁则移入waitingDelete，否则调用其Update方法，执行该动作
        foreach (KeyValuePair<int, SSAction> kv in actions)
        {
            SSAction ac = kv.Value;
            if (ac.destroy)
            {
                ac.callback.SSActionEvent(ac);      //通知ac的动作管理器ac已结束
                waitingDelete.Add(ac.GetInstanceID());
            }
            else if (ac.enable)
            {
                ac.Update();
            }
        }

        //逐个回收动作对象，正确管理内存
        foreach (int key in waitingDelete)
        {
            SSAction ac = actions[key];
            actions.Remove(key);
            DestroyObject(ac);
        }
        waitingDelete.Clear();
    }
    
    //参数：游戏对象gameobject，动作对象action，动作管理器manager
    //将action的游戏对象设为gameobject，管理器设为manager
    public void RunAction(GameObject gameobject, SSAction action, ISSActionCallback manager)
    {
        action.gameobject = gameobject;
        action.transform = gameobject.transform;
        action.callback = manager;
        waitingAdd.Add(action);     //将action加入等待执行的队列
        action.Start();             //开始action
    }
}
```



- CCActionManager包含一个CCMoveToAction类的对象，用于实现船和角色在两个河岸之间的简单直线移动
- CCActionManager包含一个CCSequenceAction类的对象，用于实现角色在船和河岸之间的折线移动
- CCActionManager向CCSequenceAction提供三个动作，构建动作列表：上升（rise），在空中水平移动（horizontal），下降着陆（fall）

```c#
//对SSActionManager根据实际用途的具体化，是一个针对特定场景的动作管理器
//因此需要实现ISSActionCallback接口，明确对动作对象开始或结束后做什么处理
public class CCActionManager : SSActionManager, ISSActionCallback {

    public CCMoveToAction moveStraight;
    public CCSequenceAction notStraight;
    public CCMoveToAction rise;
    public CCMoveToAction horizontal;
    public CCMoveToAction fall;

    public bool isMoving = false;

    //new是方法覆盖，该方法不会多态，且需要通过base.Start()调用原方法
    protected new void Start()
    {
        
    }

    //关于new，同上
    protected new void Update()
    {
        base.Update();
    }

    public void SSActionEvent(SSAction source,
        SSActionEventType events = SSActionEventType.Completed,
        int intParam = 0, string strParam = null, Object objectParam = null)
    {
        //检测是否正在执行某个动作
        if (events == SSActionEventType.Completed)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }
    }

    public void moveBoatOrRole(GameObject gj, Vector3 target, float speed)
    {
        moveStraight = CCMoveToAction.GetSSAction(target, speed);
        this.RunAction(gj, moveStraight, this);
    }

    public void moveRoleBetweenBankAndBoat(GameObject gj, Vector3 target, float speed)
    {
        rise = CCMoveToAction.GetSSAction(new Vector3(gj.transform.position.x, 5, target.z), speed);
        horizontal = CCMoveToAction.GetSSAction(new Vector3(target.x, 5, target.z), speed);
        fall = CCMoveToAction.GetSSAction(target, speed);
        notStraight = CCSequenceAction.GetSSAcition(1, 0, new List<SSAction> {rise, horizontal, fall }, this);
        this.RunAction(gj, notStraight, this);
    }
}

```



扩展：

通过使用ISSActionCallback接口所定义的SSActionEvent方法，可以令动作对象们不断向动作管理器传递它们的状态（已开始运动/完成运动），从而CCActionManager可以获知是否有未完成的动作。

FirstController用isMoving方法来从CCActionManager处知道是否有未完成的动作，UserGUI可以由此判断是否需要显示“Go!”的按钮。



```c#
//CCActionManager类内
public void SSActionEvent(SSAction source,
        SSActionEventType events = SSActionEventType.Completed,
        int intParam = 0, string strParam = null, Object objectParam = null)
    {
        //检测是否正在执行某个动作
        if (events == SSActionEventType.Completed)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }
    }


//FirstController类内 
public bool isMoving()
    {
        return actionManager.isMoving;
    }

public string GameProcessing()
    {
        if (isMoving()) return "noInput";   //有在移动的游戏对象，则通知GUI不能有用户动作
        if(result == "")                            //没有在移动的游戏对象，且游戏结果未决定
        {
            if(onBoat0 == null && onBoat1 == null) return "noInput";    //船上为空，则通知GUI不能有用户动作
            else return "canGo";            //船上不为空，通知GUI可以让用户开船
        }
        return result;                //通知GUI游戏结束
    }


//UserGUI类内
	void OnGUI() {
		float width = Screen.width / 6;
		float height = Screen.height / 12;

        if (action.GameProcessing() == "noInput") return;


        if (action.GameProcessing() == "canGo")
        {
            if (GUI.Button(new Rect(Screen.width / 2 - width / 2, 100, width, height), "Go!"))
            {
                action.Go();
            }
        }

        else {
            string result = action.GameProcessing();

            if (GUI.Button (new Rect (Screen.width / 2 - width / 2, 100, width, height), "You " + result + "!\nClick here to restart:)")) {
                SceneManager.LoadScene("Scene", LoadSceneMode.Single);
			}
			if (GUI.Button (new Rect (Screen.width / 2 - width / 2, 200, width, height), "Quit")) {
				Application.Quit ();
			}
		}
	}
```























