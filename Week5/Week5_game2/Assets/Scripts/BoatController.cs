using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour {

	private bool[] seatsOccupied;

	public BoatController(){
		
		seatsOccupied = new bool[2];
		for (int i = 0; i < 2; i++) {
			seatsOccupied [i] = false;
		}
	}

	// Use this for initialization
	void Start () {
		//this.transform.position = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool isAvailable(){
		return !seatsOccupied [0] || !seatsOccupied [1];
	}

	public Vector3 upBoat() {
		float xfirst = this.transform.position.x + transform.localScale.x / 2 - 1;
		float xpos;
		for (int i = 0; i < 2; i++) {
			if (!seatsOccupied [i]) {
				seatsOccupied [i] = true;
				xpos = xfirst - 1.5f * i;
				return new Vector3 (xpos, 1.5f, 0);
			}
		}
		return Vector3.zero;

	}

	public void downBoat(Vector3 seat) {
		float xfirst = this.transform.position.x + transform.lossyScale.x / 2 - 1;
		float xpos = seat.x;
        Debug.Log(xfirst + " " + xpos);
		int pos = (int)((xfirst - xpos) / 1.5f);
		seatsOccupied [pos] = false;
	}

	public bool isOnBoat(Vector3 position){
		float x1 = this.transform.position.x - transform.lossyScale.x / 2;
		float x2 = this.transform.position.x + transform.lossyScale.x / 2;
		if (x1 <= position.x && position.x <= x2)
			return true;
		else
			return false;
	}
}
