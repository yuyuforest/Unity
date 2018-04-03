using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Networking;
//using NUnit.Compatibility;

public class FirstController : MonoBehaviour, SceneController, UserAction {

	public bool shoreshide { get; set; }
	public string result { get; set;}

	GameObject river;

	GameObject bank0;
	GameObject bank1;
	BankController controlBank0;
	BankController controlBank1;

	GameObject boat;
	BoatController controlBoat;

	GameObject[] priests;
	GameObject[] devils;
	RoleController[] controlRole;

	GameObject onBoat0 = null;
	GameObject onBoat1 = null;

	int beginningPriests = 3;
	int beginningDevils = 3;
	int endingPriests = 0;
	int endingDevils = 0;

	void Awake () {
		SSDirector director = SSDirector.getInstance ();
		director.setFPS (60);
		director.currentSceneController = this;
		director.currentSceneController.LoadResources ();
	}

	public void LoadResources () {
		river = Instantiate<GameObject> (Resources.Load<GameObject> ("Prefabs/River"), Vector3.zero, Quaternion.identity);
		river.name = "river";

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
			priests[i] = Instantiate<GameObject> (Resources.Load<GameObject> ("Prefabs/Priest"), new Vector3(6.75f + 1.5f * i,2,0), Quaternion.identity);
			priests [i].name = "priest";
			priests[i].AddComponent<Move> ();
			//priests [i].AddComponent<MeshCollider> ();
		}
		for (int i = 0; i < 3; i++) {
			devils[i] = Instantiate<GameObject> (Resources.Load<GameObject> ("Prefabs/Devil"), new Vector3(11.25f + 1.5f * i,2,0), Quaternion.identity);
			devils [i].name = "devil";
			devils[i].AddComponent<Move> ();
			//devils [i].AddComponent<MeshCollider> ();
		}

	}

	// Use this for initialization
	void Start () {
		shoreshide = true;
		result = "";
	}
	
	// Update is called once per frame
	void Update () {
		if (!boat.GetComponent<Move> ().isMoving ()) {
			shoreshide = true;
			if (beginningDevils > beginningPriests && beginningPriests != 0 || endingDevils > endingPriests && endingPriests != 0) {
				result = "lose";
			}
			else if(controlBank1.allOnBank ()) result = "win";
		}
	}

	public string GameOver() {
		return result;
	}


	public void Click (GameObject Clicked) {
		if (!shoreshide || Clicked.transform.position.y != 2.0f &&  Clicked.transform.position.y != 1.5f)
			return;
		
		if (Clicked.name == "priest" || Clicked.name == "devil") {
			if (Clicked == onBoat0 || Clicked == onBoat1) {
				controlBoat.downBoat (Clicked.transform.position);
				if (Clicked.transform.position.x > 0) {
					Clicked.GetComponent<Move> ().goTo (controlBank0.upBank ());
				} else {
					Clicked.GetComponent<Move> ().goTo (controlBank1.upBank ());
				}
				if (Clicked == onBoat0)
					onBoat0 = null;
				else if (Clicked == onBoat1)
					onBoat1 = null;
			}
			else if (controlBoat.isAvailable ()) {
				if (Clicked.transform.position.x > 0 && boat.transform.position.x > 0) {
					controlBank0.downBank (Clicked.transform.position);
					Clicked.GetComponent<Move> ().goTo (controlBoat.upBoat ());
					if (onBoat0 == null)
						onBoat0 = Clicked;
					else
						onBoat1 = Clicked;
				}
				else if (Clicked.transform.position.x < 0 && boat.transform.position.x < 0) {
					controlBank1.downBank (Clicked.transform.position);
					Clicked.GetComponent<Move> ().goTo (controlBoat.upBoat ());
					if (onBoat0 == null)
						onBoat0 = Clicked;
					else
						onBoat1 = Clicked;
				}
			}
		}
	}

	public void Go () {
		if (onBoat0 == null && onBoat1 == null)
			return;
		if (onBoat0 != null && onBoat0.GetComponent<Move> ().isMoving ())
			return;
		if (onBoat1 != null && onBoat1.GetComponent<Move> ().isMoving ())
			return;
		
		shoreshide = false;
		float boatx = boat.transform.position.x;

		boat.GetComponent<Move> ().goTo (new Vector3(0 - boat.transform.position.x, boat.transform.position.y, boat.transform.position.z));
		float deltax = -2 * boat.transform.position.x;
		if(onBoat0 != null) onBoat0.GetComponent<Move> ().goTo (onBoat0.transform.position + new Vector3(deltax, 0, 0));
		if(onBoat1 != null) onBoat1.GetComponent<Move> ().goTo (onBoat1.transform.position + new Vector3(deltax, 0, 0));

		if (onBoat0 != null) {
			if (onBoat0.name == "priest") {
				if (boatx > 0) {
					beginningPriests -= 1;
					endingPriests += 1;
				} else {
					endingPriests -= 1;
					beginningPriests += 1;
				}
			} else {
				if (boatx > 0) {
					beginningDevils -= 1;
					endingDevils += 1;
				} else {
					endingDevils -= 1;
					beginningDevils += 1;
				}
			}
		}
		if (onBoat1 != null) {
			if (onBoat1.name == "priest") {
				if (boatx > 0) {
					beginningPriests -= 1;
					endingPriests += 1;
				} else {
					endingPriests -= 1;
					beginningPriests += 1;
				}
			} else {
				if (boatx > 0) {
					beginningDevils -= 1;
					endingDevils += 1;
				} else {
					endingDevils -= 1;
					beginningDevils += 1;
				}
			}
		}
	}


	public bool isShoreshide (){
		return shoreshide;
	}
}
