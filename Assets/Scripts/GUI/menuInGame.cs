using UnityEngine;
using System.Collections;

public class menuInGame : MonoBehaviour
{

    // true when game is in pause
    private bool isPause = false;


    public GUIStyle styleNbrLifes;

    public float startTime;
    private bool canPlay = false;


    void Start()
    {

        Time.timeScale = 1;

        startTime = Time.time + 3;

        GameObject.Find("Pedal").GetComponent<moveFoward>().enabled = false;
        GameObject.Find("Pedal2").GetComponent<moveBack>().enabled = false;

    }

    void Update()
    {


        // give 3sec of preparation for players
        if (canPlay == false && (Time.time - startTime) >= 0)
        {
            canPlay = true;
            GameObject.Find("Pedal").GetComponent<moveFoward>().enabled = true;
            GameObject.Find("Pedal2").GetComponent<moveBack>().enabled = true;

        }
    }


    void OnGUI()
    {

        GUI.Label(resizeGUI.size(new Rect(350, 15, 50, 50)), (Time.time - startTime).ToString("0.##"), styleNbrLifes);
        // lifes	
        GUI.Label(resizeGUI.size(new Rect(750, 15, 50, 50)), camera.GetComponent<lvl>().nbrLifes.ToString(), styleNbrLifes);

        // pause button
        if (GUI.Button(resizeGUI.size(new Rect(15, 15, 100, 100)), "Pause"))
        {
            isPause = !isPause;

            if (isPause)
                Time.timeScale = 0;
            else
                Time.timeScale = 1;
        }



        if (isPause)
        {
            if (GUI.Button(resizeGUI.size(new Rect(200, 15, 400, 100)), "Play"))
            {
                isPause = !isPause;
                Time.timeScale = 1;
            }
            if (GUI.Button(resizeGUI.size(new Rect(200, 130, 400, 100)), "Quit"))
            {
                Application.LoadLevel("menu");
            }
            if (GUI.Button(resizeGUI.size(new Rect(200, 245, 400, 100)), "Restart"))
            {
                Application.LoadLevel(Application.loadedLevel);
            }
        }

    }


}
