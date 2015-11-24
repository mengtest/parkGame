using UnityEngine;
using System.Collections;

public class carPhysic : MonoBehaviour
{


    public float carWidth, carHeight;
    public float xPositionStart, yPositionStart, zPositionStart;


    public float speedTranslation;
    public float speedTranslationOnCollision;
    public float speedRotation;
    private float speedTranslationStart;

    private float translation = 0, rotation = 0;

    private float acceleration = 0;
    public float addToAccel;

    public bool FWL = true, FWR = false, BWL = false, BWR = false;
    // B:back F:front W:wheel L:left R:right

    private bool collisionIsPossible = true;
    public bool activeCollision = true;
    public float timeBetweenTwoCollisions;
    private float addTimeDelta;

    public bool moveFoward = false, moveBack = false;

    private float indRotation = 0;
    public float steeringLock = 0.4f;

    public float startDrag, dragOnCollision, angularDrag;

    public Transform target;
    private float angleCarTarget;
    public bool targetOk = false;

    private short vibrationActivate, accelerometerHelp;

    private GameObject camera;

    private Sprite scriptSprite;


    #region Script on child

    // <summary>
    // Add FWL, BWL scripts etc at the wheels of car
    // </summary>
    void addScriptsChildren()
    {


        foreach (Transform child in transform)
        {
            switch (child.name)
            {
                case "FWL":
                    child.gameObject.AddComponent("FWL");
                    break;
                case "FWR":
                    child.gameObject.AddComponent("FWR");
                    break;
                case "BWL":
                    child.gameObject.AddComponent("BWL");
                    break;
                case "BWR":
                    child.gameObject.AddComponent("BWR");
                    break;
            }

        }
    }
    #endregion

	void Awake() {
		//find camera
		camera = GameObject.Find ("Camera");
	}

    void Start()
    {
        //initialisation of car
        gameObject.AddComponent("Rigidbody");
        transform.localScale = new Vector3(carWidth, 0.5f, carHeight);
        transform.position = new Vector3(xPositionStart, yPositionStart, zPositionStart);

        //rigidbody specifications
        rigidbody.mass = 2;
        rigidbody.drag = startDrag;
        rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rigidbody.angularDrag = angularDrag;
        rigidbody.useGravity = false;


        speedTranslationStart = speedTranslation;

        //add scripts on wheels 
        addScriptsChildren();

        //check if user wants vibrations or not
        vibrationActivate = (short)PlayerPrefs.GetInt("vibrationActivate");

        //check if user wants help for accelerometer
        accelerometerHelp = (short)PlayerPrefs.GetInt("accelerometerHelp");

		 //find sprite script
        scriptSprite = GetComponentInChildren<Sprite>();
    }

    #region Accelerometer

    // <summary>
    // Check and control the value of accelerometer
    // </summary>
    private void useAccelerometer()
    {
        if (Input.acceleration.x >= steeringLock)
            indRotation = steeringLock;
        else if (Input.acceleration.x <= -steeringLock)
            indRotation = -steeringLock;
        else
            indRotation = Input.acceleration.x;
    }

    #endregion


    #region Progressive acceleration

    // <summary>
    // Add a coefficient to acceleration
    // </summary>
    private void onAcceleration()
    {

        if (moveFoward)
        {
            if (acceleration < 1)
                acceleration += addToAccel;
            else
                acceleration = 1;
        }
        else if (moveBack)
        {
            if (acceleration > -1)
                acceleration -= addToAccel;
            else
                acceleration = -1;

        }
        else
        {
            acceleration = 0;
        }

        //Debug.Log (acceleration); 

        translation = acceleration * speedTranslation /** Time.deltaTime*/;
    }
    #endregion


    #region Objectives

    // <summary>
    // Check if the car is on a objective -> True or not -> False
    // </summary>
    private bool checkObjective()
    {
        if (transform.position.x + transform.localScale.x / 2 < target.transform.position.x + target.transform.localScale.x / 2 && transform.position.x - transform.localScale.x / 2 > target.transform.position.x - target.transform.localScale.x / 2)
        {
            if (transform.position.z + transform.localScale.z / 2 < target.transform.position.z + target.transform.localScale.z / 2 && transform.position.z - transform.localScale.z / 2 > target.transform.position.z - target.transform.localScale.z / 2)
            {

                if (Quaternion.Angle(transform.rotation, target.transform.rotation) > 90)
                    angleCarTarget = Mathf.Abs(180 - Quaternion.Angle(transform.rotation, target.transform.rotation));
                else
                    angleCarTarget = Quaternion.Angle(transform.rotation, target.transform.rotation);

                if (angleCarTarget <= 5)
                    return true;
            }
        }
        return false;
    }
    #endregion


    void Update()
    {

        if (accelerometerHelp == 0)
            useAccelerometer();


        if (checkObjective())
            targetOk = true;


        // on collision disable the car's collider during timeBetweenTwoCollisions
        if (!collisionIsPossible && activeCollision)
        {
            addTimeDelta += Time.deltaTime;
            if (addTimeDelta >= timeBetweenTwoCollisions)
            {
                collisionIsPossible = true;
                scriptSprite.leave();
            }
        }
    }


    void FixedUpdate()
    {

        onAcceleration();

        if (translation > 0)
        {
            rotation = indRotation * speedRotation * acceleration;


            if ((FWL || FWR) && !(FWL && FWR))
            {
                if (FWL && rotation > 0)
                    transform.Rotate(0, rotation, 0);
                else if (FWR && rotation < 0)
                    transform.Rotate(0, rotation, 0);

            }
            else if (!(FWL && FWR))
            {
                transform.Rotate(0, rotation, 0);
            }

        } // IF TRANS > 0


        else if (translation < 0)
        {
            translation /= 1.5f;

            rotation = indRotation * speedRotation / 2 * acceleration;

            if ((BWL || BWR) && !(BWL && BWR))
            {
                if (BWL && rotation < 0)
                    transform.Rotate(0, rotation, 0);
                else if (BWR && rotation > 0)
                    transform.Rotate(0, rotation, 0);

            }
            else if (!(BWL && BWR))
            {
                transform.Rotate(0, rotation, 0);
            }


        } // ELSE IF TRAN < 0 

        rigidbody.AddRelativeForce(translation, 0, 0);
    }



    void OnCollisionEnter(Collision c)
    {

        if (c.collider.gameObject.name == "IA")
			Application.LoadLevel ("menu");
		else if (collisionIsPossible && activeCollision)
        {

            if (vibrationActivate == 1)
                Handheld.Vibrate();

            rigidbody.drag = dragOnCollision;
            speedTranslation = speedTranslationOnCollision;


            // remove one life
            camera.GetComponent<lvl>().nbrLifes--;

            // opacity
            scriptSprite.touch();


            //prevent next collision during few secondes
            collisionIsPossible = false;
            addTimeDelta = 0;
        }
    }

    void OnCollisionExit()
    {
        rigidbody.drag = startDrag;
        speedTranslation = speedTranslationStart;

    }



    #region Help for accelerometer

    // <summary>
    // Show accelerometer value but  in future the player will be able to play whith
    // </summary>
    void OnGUI()
    {
        if (accelerometerHelp == 1)
            indRotation = GUI.HorizontalSlider(resizeGUI.size(new Rect(15, 500, 100, 100)), indRotation, -steeringLock, steeringLock);
        //indRotation = GUI.HorizontalScrollbar(new Rect(15, 500, 100, 100), indRotation, 10, -steeringLock, steeringLock);
    }
    #endregion

}
