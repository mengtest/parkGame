using UnityEngine;
using System.Collections;

public class IA : MonoBehaviour
{
	
	
	public float carWidth, carHeight;
	public float xPositionStart, yPositionStart, zPositionStart;
	public float xPositionEnd;
	
	
	public float speedTranslation;
	public float drag;
	public float angularDrag;

	private Transform bumper;
	public bool bumperOnTrigger;

	private void makeBumper(Transform b) {
		b.transform.localPosition = new Vector3 (0.75f, 0, 0);
		b.transform.localScale = new Vector3 (0.5f, 1, 1);
		b.collider.isTrigger = true;
		b.renderer.enabled = false;
	}


	void Awake()
	{
		bumper = transform.FindChild ("Bumper");
	}

	void Start()
	{
		//initialisation of car
		gameObject.AddComponent("Rigidbody");
		transform.localScale = new Vector3(carWidth, 0.5f, carHeight);
		transform.position = new Vector3(xPositionStart, yPositionStart, zPositionStart);
		
		//rigidbody specifications
		rigidbody.mass = 2;
		rigidbody.drag = drag;
		rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX |  RigidbodyConstraints.FreezeRotationZ;
		rigidbody.angularDrag = angularDrag;
		rigidbody.useGravity = false;

		makeBumper (bumper);
	}

	void FixedUpdate()
	{
		if(!bumperOnTrigger)
			rigidbody.AddRelativeForce(speedTranslation, 0,0);

		if(transform.position.x >= xPositionEnd)
		{
			transform.position = new Vector3(xPositionStart, yPositionStart, zPositionStart);
			//transform.collider.isTrigger = false;
		}
	}

	/*void OnCollisionStay(Collision c)
	{
		if(c.collider.gameObject.name != "Car0")
			transform.collider.isTrigger=true;
	}*/




}

