using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class lvl : MonoBehaviour
{


    private GameObject[] car = new GameObject[6];
    private GameObject[] target = new GameObject[6];
    private GameObject[] pedal = new GameObject[2];

    private bool lvlFinished = false;

    private List<float> ghostCar = new List<float>();

    // same number for targets
    public int nbrCars;
    public int numLvl;
    public int nbrLifes;
    private int[] lvlInfo;

    private short j = 0;

    private Rect windowEndLvl = resizeGUI.size(new Rect(200, 150, 400, 300));

    void Awake()
    {


        // find all cars in lvl
        for (short i = 0; i < nbrCars; i++)
        {
            car[i] = GameObject.Find("Car" + i.ToString());
            if (i > 0)
                car[i].GetComponent<carPhysic>().activeCollision = false;
        }

        // find all objectives in lvl
        for (short i = 0; i < nbrCars; i++)
        {
            target[i] = GameObject.Find("Target" + i.ToString());
            if (i > 0)
                target[i].GetComponent<MeshRenderer>().renderer.enabled = false;
        }


        pedal[0] = GameObject.Find("Pedal");
        pedal[1] = GameObject.Find("Pedal2");

        lvlInfo = PlayerPrefsX.GetIntArray("lvlInfo");

    }

    void Update()
    {


        if (FB.IsLoggedIn)
        {
            ghostCar.Add(j);
            ghostCar.Add(car[j].transform.position.x);
            ghostCar.Add(car[j].transform.position.z);
            ghostCar.Add(car[j].transform.rotation.y);
        }


        if (nbrLifes <= 0)
            Application.LoadLevel("menu");


        else if (car[j].GetComponent<carPhysic>().targetOk == true && j < nbrCars - 1)
        {

            car[j].GetComponent<carPhysic>().moveFoward = false;
            car[j].GetComponent<carPhysic>().moveBack = false;
            car[j].GetComponent<carPhysic>().activeCollision = false;

            target[j].GetComponent<MeshRenderer>().renderer.enabled = false;


            j++; // NEXT CAR

            pedal[0].GetComponent<moveFoward>().car = car[j].GetComponent<carPhysic>();
            pedal[1].GetComponent<moveBack>().car = car[j].GetComponent<carPhysic>();

            camera.GetComponent<FollowCar>().car = car[j];
            target[j].GetComponent<MeshRenderer>().renderer.enabled = true;
            car[j].GetComponent<carPhysic>().activeCollision = true;
        }

        else if (j == nbrCars - 1 && car[nbrCars - 1].GetComponent<carPhysic>().targetOk == true)
        {
            Time.timeScale = 0;

            // display windowEnd
            lvlFinished = true;


            // save that player finished the level
            lvlInfo[numLvl - 1] = 1;
            PlayerPrefsX.SetIntArray("lvlInfo", lvlInfo);

            // build the ghostCar list to send it

            if (FB.IsLoggedIn)
            {
				ghostCar = makeGhostCar(ghostCar);
            }

        }


    }


    #region makeGhostCar

    // <summary>
    // Prepare list of informations and positions to send it to server
    // </summary>
    private List<float> makeGhostCar(List<float> l)
    {
        l.Insert(0, float.Parse(FB.UserId));
        l.Insert(1, numLvl);
        l.Insert(2, Time.time - camera.GetComponent<menuInGame>().startTime);
        l.Insert(3, l.Count - 3);
		return l;
    }

    #endregion


    #region GUI
    void OnGUI()
    {
        if (lvlFinished)
        {
            windowEndLvl = GUI.Window(0, windowEndLvl, doWindow, "Lvl Finished");
        }
    }
    #endregion


    #region doWindow

    // <summary>
    // Window at the end of level
    // </summary>
    void doWindow(int id)
    {
        if (GUI.Button(resizeGUI.size(new Rect(150, 75, 100, 100)), "Return to menu"))
        {
            Application.LoadLevel("menu");
        }

        if (FB.IsLoggedIn)
        {
            if (GUI.Button(resizeGUI.size(new Rect(10, 75, 100, 100)), "Send ghost car"))
                Debug.Log("ok");
        }

        if (GUI.Button(resizeGUI.size(new Rect(290, 75, 100, 100)), "Restart"))
            Application.LoadLevel(Application.loadedLevel);

    }
    #endregion



}

