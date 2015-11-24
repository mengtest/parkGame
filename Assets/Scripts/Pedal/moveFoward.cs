using UnityEngine;
using System.Collections;

public class moveFoward : MonoBehaviour {


	public carPhysic car;

	void Awake() 
	{
		//transform.guiTexture.pixelInset = new Rect (0, 0, (int)Screen.width / 8, (int)Screen.height / 3);
		transform.guiTexture.pixelInset = resizeGUI.size(new Rect (0, 0, 80, 200));

		//find the first car
		car = GameObject.Find ("Car0").GetComponent<carPhysic>();
	}


	void Update()
	{
		if (Input.touchCount > 0) {
			if (transform.guiTexture.HitTest (Input.touches [0].position))
				car.moveFoward = true;
			else
				car.moveFoward = false;
		}
		else
			car.moveFoward = false;
		
	}



	// FUTUR PASSAGE POSSIBLE A REPEATBUTTON


	/*void OnMouseDrag()
	{
		GameObject.Find ("Car2").GetComponent<carPhysic> ().moveFoward = true;	
	}
	void OnMouseUp()
	{
		GameObject.Find ("Car2").GetComponent<carPhysic> ().moveFoward = false;
	}*/
}
