using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankController : MonoBehaviour {

	private bool[] seatsOccupied;
	private static float[] endingseats = { -14.0f, -12.5f, -11.0f, -9.5f, -8.0f, -6.5f };
	private static float[] beginningseats = { 6.5f, 8.0f, 9.5f, 11.0f, 12.5f, 14.0f };

	void Awake() {
		seatsOccupied = new bool[6];
		for (int i = 0; i < 6; i++) {
			seatsOccupied[i] = false;
		}
	}
    
	void Start () {
		if (this.name == "bank0") {
			for (int i = 0; i < 6; i++) {
				seatsOccupied [i] = true;
			}
		}
	}
	
	void Update () {
		
	}

	public Vector3 upBank() {
		Vector3 seatpos = Vector3.zero;

		for (int i = 0; i < 6; i++) {
			if (!seatsOccupied [i]) {
				seatsOccupied [i] = true;
				float xpos = 0;
				if (transform.position.x > 0) {
					xpos = beginningseats [i];
				}
				else if (transform.position.x < 0) {
					xpos = endingseats [i];
				}
				seatpos = new Vector3 (xpos, 2, 0);
				break;
			}
		}
		return seatpos;
	}

	public void downBank(Vector3 seat) {
		float xpos = seat.x;
		if (transform.position.x > 0) {
			for (int i = 0; i < 6; i++) {
				if (beginningseats [i] == xpos) {
					seatsOccupied [i] = false;
                    
                    break;
				}
			}
		}
		else if (transform.position.x < 0) {
			for (int i = 0; i < 6; i++) {
				if (endingseats [i] == xpos) {
					seatsOccupied [i] = false;
                    break;
                }
			}
		}
	}

	public bool allOnBank(){
		for (int i = 0; i < 6; i++) {
			if(seatsOccupied[i] == false) return false;
		}
		return true;
	}
}
