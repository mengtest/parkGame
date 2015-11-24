using UnityEngine;
using System.Collections;

public class FollowCar : MonoBehaviour {

	public GameObject car;

	//private float width, height;



	void Awake() {
		//height = camera.orthographicSize /*2*/;
		//width = height * camera.aspect;
		//ground = GameObject.Find ("Ground");
		car = GameObject.Find ("Car0");

	}


	void Update () {

	//	if(car.transform.position.x + width < ground.transform.position.x + ground.transform.localScale.x/2 && car.transform.position.x - width > ground.transform.position.x - ground.transform.localScale.x/2) {
			//if(car.transform.position.z + height < ground.transform.position.z + ground.transform.localScale.z/2 && car.transform.position.z - height > ground.transform.position.z - ground.transform.localScale.z/2)
				transform.position = new Vector3 (car.transform.position.x, transform.position.y, car.transform.position.z);
	//	}	

	}


}
