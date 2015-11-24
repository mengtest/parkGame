using UnityEngine;
using System.Collections;

public class IABumper : MonoBehaviour 
{

	void OnTriggerEnter(Collider c)
	{
		transform.parent.GetComponent<IA> ().bumperOnTrigger = true;
			
	}

	void OnTriggerExit()
	{
		transform.parent.GetComponent<IA> ().bumperOnTrigger = false;
	}

}