using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class menu : MonoBehaviour {


	public bool mainMenu=true, lvl=false, config=false;

	// parameters for player
	private bool vibrate, accelerometerHelp;

	// array to know if lvl is already deblock or not
	private int[] lvlInfo;

	// informations about player
	private Dictionary<string, string> profile =  new Dictionary<string, string>();

	// list of friends' player
	private List<object> friends = new List<object>();

	// list of profiles pictures
	private List<Texture> Pictures = new List<Texture>();

	private string name="LEVEL 1";

	#region Facebook

	void OnInit() {
		enabled = true;
		if(FB.IsLoggedIn)
			OnLoggedIn();

	}

	void OnHide(bool display) {
		if (!display)                                                                        
		{                                                                                        
			// pause the game - we will need to hide                                             
			Time.timeScale = 0;                                                                  
		}                                                                                        
		else                                                                                     
		{                                                                                        
			// start the game back up - we're getting focus again                                
			Time.timeScale = 1;                                                                  
		}     
	}


	private void LogInCallBack(FBResult result){
		if(FB.IsLoggedIn)
			OnLoggedIn();
	}

	private void OnLoggedIn() {
		FB.API("/me?fields=first_name,friends.limit(10).fields(first_name,id)", Facebook.HttpMethod.GET, InfoCallBack); 
	}
	
	private void InfoCallBack(FBResult result) {                                                                                               
		if (result.Error != null)                                                                                                  
		{                                                                                                                          
			FbDebug.Error(result.Error);                                                                                           
			// Let's just try again                                                                                                
			FB.API("/me?fields=first_name,friends.limit(10).fields(first_name,id)", Facebook.HttpMethod.GET, InfoCallBack);     
			return;                                                                                                                
		}                                                                                                                          
		
		profile = Util.DeserializeJSONProfile(result.Text); 
		friends = Util.DeserializeJSONFriends(result.Text);

		RecupFriendPicture (friends);
	
	}

	private void RecupFriendPicture(List<object> l) {

		foreach(var obj in l) {
			var dic1 = ((Dictionary<string, object>)obj);
			FB.API (Util.GetPictureURL((string)dic1["id"], 100,100), Facebook.HttpMethod.GET, PictureFriendCallBack);
		}

	}

	private void PictureFriendCallBack(FBResult result) {
		Pictures.Add (result.Texture);
	}

	private void InvitationCallBack(FBResult response) {
		Debug.Log(response.Text);
	}

	#endregion

	void Awake() {
		enabled = false;
		FB.Init (OnInit, OnHide);
	}


	void Start() {
		// check for parameters
		vibrate = PlayerPrefs.GetInt ("vibrationActivate") == 1;
		accelerometerHelp = PlayerPrefs.GetInt ("accelerometerHelp") == 1;

		lvlInfo = PlayerPrefsX.GetIntArray ("lvlInfo", 0, 50);
		PlayerPrefsX.SetIntArray ("lvlInfo", lvlInfo);

	}

	void Update() {

	}

	void OnGUI() {

		#region mainMenu
		if(mainMenu) 
		{
			if(GUI.Button(resizeGUI.size(new Rect(200, 100, 400, 100)), "Play")) 
			{
				lvl=true;
				mainMenu=false;
			}

			if(GUI.Button(resizeGUI.size(new Rect(200, 250, 400, 100)), "Config")) 
			{
				config=true;
				mainMenu=false;
			}
			if(GUI.Button(resizeGUI.size(new Rect(200, 400, 400, 100)), "Quit")) 
			{
				Application.Quit();
			}


			if(!FB.IsLoggedIn) 
			{
				if(GUI.Button(resizeGUI.size(new Rect(15, 15, 100, 100)), "Connect with Facebook")) 
				{
					FB.Login("read_friendlists,publish_actions", LogInCallBack);
				}

			}
			else 
			{
				if(GUI.Button(resizeGUI.size(new Rect(15, 15, 100, 100)), "Disconnect")) 
					FB.Logout();


				if(GUI.Button(resizeGUI.size(new Rect(15,485,100,100)), "Send")) 
				{
					FB.Feed(
						link: "http://www.google.fr",
						linkName: "Great Game!",
						linkCaption: "Come on and play!",
						linkDescription: "Yeah",
						callback: InvitationCallBack
						);
				}


				GUI.contentColor=Color.black;


				// say welcome
				GUI.Label (resizeGUI.size (new Rect (350, 15, 150, 50)), "Welcome "+profile["first_name"]);
			}

		}
		#endregion

		#region lvl
		if(lvl) 
		{
			if(GUI.Button (resizeGUI.size(new Rect(350, 15, 100, 100)), name))
				Application.LoadLevel(1);

			GUI.DrawTexture(resizeGUI.size(new Rect (500, 15, 100, 100)), Pictures[0]);



			if(GUI.Button(resizeGUI.size(new Rect(350, 150, 100,100)), "Level 2"))
				Debug.Log ("coucou");
		



			if(GUI.Button (resizeGUI.size(new Rect(15, 15, 100, 100)), "Return")) 
			{
				mainMenu = true;
				lvl = false;
			}
		}
		#endregion

		#region config
		if(config) 
		{
			if(GUI.Button(resizeGUI.size(new Rect(15, 15, 100, 100)), "Return")) 
			{
				mainMenu = true;
				config = false;
			}

			vibrate = GUI.Toggle(resizeGUI.size(new Rect(350, 300, 100, 100)), vibrate, "  Active vibrations");
			accelerometerHelp = GUI.Toggle(resizeGUI.size(new Rect(350, 400, 100, 100)), accelerometerHelp, "  Active help for rotation");
		

			if(vibrate)
				PlayerPrefs.SetInt("vibrationActivate", 1);
			else
				PlayerPrefs.SetInt("vibrationActivate", 0);

			if(accelerometerHelp)
				PlayerPrefs.SetInt("accelerometerHelp", 1);
			else
				PlayerPrefs.SetInt("accelerometerHelp", 0);
		}
		#endregion

	} // end of OnGUI()


}
