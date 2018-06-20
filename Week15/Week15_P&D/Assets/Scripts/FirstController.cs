using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Networking;
//using NUnit.Compatibility;

public class FirstController : MonoBehaviour, SceneController, UserAction {
    
    public bool isMoving()
    {
        return actionManager.isMoving;
    }
    public string result;
    public string process;

    private const float notStraightSpeed = 9.0f;
    private const float straightSpeed = 6.0f;
    
	GameObject bank0;
	GameObject bank1;
	BankController controlBank0;
	BankController controlBank1;

	GameObject boat;
	BoatController controlBoat;

	GameObject[] priests;
	GameObject[] devils;

    //记录游戏过程中变化的船上角色和两边的魔鬼和牧师的人数对比
	GameObject onBoat0 = null;
	GameObject onBoat1 = null;
	int beginningPriests = 3;
	int beginningDevils = 3;
	int endingPriests = 0;
	int endingDevils = 0;

    public CCActionManager actionManager;
    public AIGameController AIController;
    
	void Awake () {
		SSDirector director = SSDirector.getInstance ();
		director.setFPS (60);
		director.currentSceneController = this;
		director.currentSceneController.LoadResources ();
	}

	public void LoadResources () {

		bank0 = Instantiate<GameObject> (Resources.Load<GameObject> ("Prefabs/Bank"), new Vector3(10.25f,1,0), Quaternion.identity);
		bank0.name = "bank0";
		controlBank0 = bank0.AddComponent<BankController> ();

		bank1 = Instantiate<GameObject> (Resources.Load<GameObject> ("Prefabs/Bank"), new Vector3(-10.25f,1,0), Quaternion.identity);
		bank1.name = "bank1";
		controlBank1 = bank1.AddComponent<BankController> ();

		boat = Instantiate<GameObject> (Resources.Load<GameObject> ("Prefabs/Boat"), new Vector3(3.75f,0.75f,0), Quaternion.identity);
		boat.name = "boat";
		controlBoat = boat.AddComponent<BoatController> ();
		boat.AddComponent<Move> ();


		priests = new GameObject[3];

		devils = new GameObject[3];

		for (int i = 0; i < 3; i++) {
			priests[i] = Instantiate<GameObject> (Resources.Load<GameObject> ("Prefabs/Priest"), new Vector3(6.5f + 1.5f * i,2,0), Quaternion.identity);
			priests [i].name = "priest";
		}
		for (int i = 0; i < 3; i++) {
			devils[i] = Instantiate<GameObject> (Resources.Load<GameObject> ("Prefabs/Devil"), new Vector3(11.0f + 1.5f * i,2,0), Quaternion.identity);
			devils [i].name = "devil";
		}

        //挂载动作管理器
        actionManager = gameObject.AddComponent<CCActionManager>();
        //挂载AI管理
        AIController = gameObject.AddComponent<AIGameController>();
    }
    
	void Start () {
		result = "";
	}
	
	void Update () {
		if (!isMoving()) {
			if (beginningDevils > beginningPriests && beginningPriests != 0 || endingDevils > endingPriests && endingPriests != 0) {
				result = "lose";
			}
			else if(controlBank1.allOnBank ()) result = "win";
		}
	}
    


	public void Click (GameObject Clicked)
    {
        if (isMoving())
			return;
           
        if (Clicked.name == "priest" || Clicked.name == "devil")
        {
            if (Clicked == onBoat0 || Clicked == onBoat1) {
				controlBoat.downBoat (Clicked.transform.position);
				if (Clicked.transform.position.x > 0) {
                    actionManager.moveRoleBetweenBankAndBoat(Clicked, controlBank0.upBank(), notStraightSpeed);
                } else {
                    actionManager.moveRoleBetweenBankAndBoat(Clicked, controlBank1.upBank(), notStraightSpeed);
                }
				if (Clicked == onBoat0)
					onBoat0 = null;
				else if (Clicked == onBoat1)
					onBoat1 = null;
			}
			else if (controlBoat.isAvailable ())
            {
                if (Clicked.transform.position.x > 0 && boat.transform.position.x > 0) {
					controlBank0.downBank (Clicked.transform.position);
                    actionManager.moveRoleBetweenBankAndBoat(Clicked, controlBoat.upBoat(), notStraightSpeed);
                    if (onBoat0 == null)
						onBoat0 = Clicked;
					else
						onBoat1 = Clicked;
				}
				else if (Clicked.transform.position.x < 0 && boat.transform.position.x < 0) {
					controlBank1.downBank (Clicked.transform.position);
                    actionManager.moveRoleBetweenBankAndBoat(Clicked, controlBoat.upBoat(), notStraightSpeed);
                    if (onBoat0 == null)
						onBoat0 = Clicked;
					else
						onBoat1 = Clicked;
				}
			}
		}
	}

	public void Go () {                     
        if (actionManager.isMoving) return; //有正在移动的物体则不能Go
        if (onBoat0 == null && onBoat1 == null) //船上无人则不能Go
			return;
        
		float boatx = boat.transform.position.x;
        
        actionManager.moveBoatOrRole(boat, new Vector3(0 - boat.transform.position.x, boat.transform.position.y, boat.transform.position.z), straightSpeed);

        float deltax = -2 * boatx;
		if(onBoat0 != null)
            actionManager.moveBoatOrRole(onBoat0, onBoat0.transform.position + new Vector3(deltax, 0, 0), straightSpeed);
        if (onBoat1 != null)
            actionManager.moveBoatOrRole(onBoat1, onBoat1.transform.position + new Vector3(deltax, 0, 0), straightSpeed);

        int deltaPr = 0, deltaDe = 0;
        if (onBoat0 != null) {
			if (onBoat0.name == "priest") {
				if (boatx > 0) {
                    deltaPr -= 1;
				} else {
                    deltaPr += 1;
				}
			} else {
				if (boatx > 0) {
                    deltaDe -= 1;
				} else {
                    deltaDe += 1;
				}
			}
		}
		if (onBoat1 != null) {
			if (onBoat1.name == "priest") {
				if (boatx > 0) {
                    deltaPr -= 1;
                } else {
                    deltaPr += 1;
                }
			} else {
				if (boatx > 0) {
                    deltaDe -= 1;
                } else {
                    deltaDe += 1;
                }
			}
		}
        AIController.change(deltaPr, deltaDe);
	}

    public string GameProcessing()
    {
        if (isMoving()) return "noInput";   //有在移动的游戏对象，则通知GUI不能有用户动作
        if(result == "")                            //没有在移动的游戏对象，且游戏结果未决定
        {
            if (onBoat0 == null && onBoat1 == null) return "canNext";    //船上为空，则通知GUI可以让用户选择自动进行下一步
            else return "canGo";            //船上不为空，通知GUI可以让用户开船
        }
        return result;                //通知GUI游戏结束
    }

    public void AllUpBank()
    {
        if (onBoat0 != null)
        {
            controlBoat.downBoat(onBoat0.transform.position);
            if (onBoat0.transform.position.x > 0)
            {
                actionManager.moveRoleBetweenBankAndBoat(onBoat0, controlBank0.upBank(), notStraightSpeed);
            }
            else
            {
                actionManager.moveRoleBetweenBankAndBoat(onBoat0, controlBank1.upBank(), notStraightSpeed);
            }
            onBoat0 = null;
        }
        if (onBoat1 != null)
        {
            controlBoat.downBoat(onBoat1.transform.position);
            if (onBoat1.transform.position.x > 0)
            {
                actionManager.moveRoleBetweenBankAndBoat(onBoat1, controlBank0.upBank(), notStraightSpeed);
            }
            else
            {
                actionManager.moveRoleBetweenBankAndBoat(onBoat1, controlBank1.upBank(), notStraightSpeed);
            }
            onBoat1 = null;
        }
    }

    public void Next()
    {
        Debug.Log("current next");
        State next = AIController.Next();
        State current = AIController.current;
        Debug.Log("(" + current.beginningPriests + " " + current.beginningDevils + " " + current.boatBeginning + ")");
        Debug.Log("next (" + next.beginningPriests + " " + next.beginningDevils + " " + next.boatBeginning + ")");
        if (current.boatBeginning)
        {
            //船在起点岸，要从起点岸上挑人下去
            int pr = current.beginningPriests - next.beginningPriests;
            int de = current.beginningDevils - next.beginningDevils;
            for (int i = 0; i < 3 && pr != 0; i++)
            {
                if (priests[i].transform.position.x > 0)
                {
                    controlBank0.downBank(priests[i].transform.position);
                    actionManager.moveRoleBetweenBankAndBoat(priests[i], controlBoat.upBoat(), notStraightSpeed);
                    if (onBoat0 == null) onBoat0 = priests[i];
                    else onBoat1 = priests[i];
                    pr--;
                }
            }
            for (int i = 0; i < 3 && de != 0; i++)
            {
                if (devils[i].transform.position.x > 0)
                {
                    controlBank0.downBank(devils[i].transform.position);
                    actionManager.moveRoleBetweenBankAndBoat(devils[i], controlBoat.upBoat(), notStraightSpeed);
                    if (onBoat0 == null) onBoat0 = devils[i];
                    else onBoat1 = devils[i];
                    de--;
                }
            }
        }
        else
        {
            //船在终点岸，要从终点岸上挑人下去
            int pr = current.endingPriests - next.endingPriests;
            int de = current.endingDevils - next.endingDevils;
            Debug.Log("(" + current.beginningPriests + " " + current.beginningDevils + " " + current.boatBeginning + ")");
            Debug.Log("next (" + next.beginningPriests + " " + next.beginningDevils + " " + next.boatBeginning + ")");
            Debug.Log("pr " + pr + " de " + de);
            for (int i = 0; i < 3 && pr > 0; i++)
            {
                if (priests[i].transform.position.x < 0)
                {
                    controlBank1.downBank(priests[i].transform.position);
                    actionManager.moveRoleBetweenBankAndBoat(priests[i], controlBoat.upBoat(), notStraightSpeed);
                    if (onBoat0 == null) onBoat0 = priests[i];
                    else onBoat1 = priests[i];
                    pr--;
                }
            }
            for (int i = 0; i < 3 && de > 0; i++)
            {
                if (devils[i].transform.position.x < 0)
                {
                    controlBank1.downBank(devils[i].transform.position);
                    actionManager.moveRoleBetweenBankAndBoat(devils[i], controlBoat.upBoat(), notStraightSpeed);
                    if (onBoat0 == null) onBoat0 = devils[i];
                    else onBoat1 = devils[i];
                    de--;
                }
            }
        }
    }
}
