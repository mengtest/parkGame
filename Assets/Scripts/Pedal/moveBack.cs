using UnityEngine;
using System.Collections;

public class moveBack : MonoBehaviour {


	public carPhysic car;

	void Awake () {
		//transform.guiTexture.pixelInset = new Rect (0, 0, (int)Screen.width / 6, (int)Screen.height / 3);
		transform.guiTexture.pixelInset = resizeGUI.size(new Rect (0, 0, 160, 200));

		//find first car
		car = GameObject.Find ("Car0").GetComponent<carPhysic>();
	}

	void Update()
	{
		if (Input.touchCount > 0) {
			if (transform.guiTexture.HitTest (Input.touches [0].position))
				car.moveBack = true;
			else
				car.moveBack = false;
		}
		else
			car.moveBack = false;

	}
	
		
}

