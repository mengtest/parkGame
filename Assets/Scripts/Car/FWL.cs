using UnityEngine;
using System.Collections;

public class FWL : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.name = "FWL";
		collider.isTrigger = true;


		transform.localPosition = new Vector3 (0.255f, 0, 0.255f);
		transform.localScale = new Vector3 (0.505f, 0.2f, 0.505f);
	}


	void OnTriggerEnter()
	{
		transform.parent.GetComponent<carPhysic> ().FWL = true;
		
	}
	
	void OnTriggerExit()
	{
		transform.parent.GetComponent<carPhysic> ().FWL = false;
	}
}
