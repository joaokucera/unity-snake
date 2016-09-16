using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour {
	// public fields
	public Rect foodPos = new Rect(0,0,20,20);
	
	// private fields
	private static Food instance = null;
	private static int[] initXPos = new int[] {22,42,62,82,102,122,142,162,182,202,222,242,262,282,302,322,342,362,382,402,422,442,462,482,502,522,542,562,582,602,622,642,662,682,702,722,742,762,782,802,822,842,862,882,902,922,942,962,982};
	private static int[] initYPos = new int[] {94,114,134,154,174,194,214,234,254,274,294,314,334,354,374,394,414,434,454,474,494,514,534,554,574,594,614,634,654};
	private Texture2D foodTexture;
	private AudioClip foodPickup;
	
	// ---------------------------------------------------------------------------------------------------
	// constructor field: Instance
	// --------------------------------------------------------------------------------------------------- 
	// Creates an instance of Food if one does not exists
	// ---------------------------------------------------------------------------------------------------
	public static Food Instance
	{
		get 
		{
			if (instance == null)
			{
				instance = new GameObject("Food").AddComponent<Food>();
			}
			
			return instance;
		}
	}
	
	// ---------------------------------------------------------------------------------------------------
	// Unity method: OnApplicationQuit()
	// --------------------------------------------------------------------------------------------------- 
	// Called when you quit the application or stop the editor player
	// ---------------------------------------------------------------------------------------------------
	public void OnApplicationQuit()
	{
		DestroyInstance();
	}
	
	// ---------------------------------------------------------------------------------------------------
	// DestroyInstance()
	// --------------------------------------------------------------------------------------------------- 
	// Destroys the Food instance
	// ---------------------------------------------------------------------------------------------------
	
	public void DestroyInstance()
	{
		print("Food Instance destroyed");
		
		instance = null;
	}
	
	// ---------------------------------------------------------------------------------------------------
	// UpdateFood()
	// --------------------------------------------------------------------------------------------------- 
	// Updates the Food position
	// ---------------------------------------------------------------------------------------------------
	
	public void UpdateFood()
	{
		print("Food updated");
		
		// play our food pickup sound
		GetComponent<AudioSource>().Play();
		
		// initialize pixelInset random positions
		int ranX = Random.Range(0, initXPos.Length);
		int ranY = Random.Range(0, initYPos.Length);
		
		// assign a random position to the pixelInset
		foodPos = new Rect(initXPos[ranX],initYPos[ranY],20,20);
	}
	
	// ---------------------------------------------------------------------------------------------------
	// OnGUI()
	// --------------------------------------------------------------------------------------------------- 
	// Unity method for handling GUI rendering, used for rendering the Food texture
	// ---------------------------------------------------------------------------------------------------
	void OnGUI()
	{
		if (Food.Instance != null)
		{
			GUI.DrawTexture(foodPos, foodTexture);
		}
	}
	
	
	// ---------------------------------------------------------------------------------------------------
	// Initialize()
	// --------------------------------------------------------------------------------------------------- 
	// Initializes Food
	// ---------------------------------------------------------------------------------------------------
	public void Initialize()
	{
		print("Food initialized");
		
		// add our AudioSource component
		if (!gameObject.GetComponent<AudioSource>())
		{
			// load in our clips
			foodPickup = Resources.Load("Sounds/Food Pickup") as AudioClip;
			
			gameObject.AddComponent<AudioSource>();
			
			// initialize some audio properties
			GetComponent<AudioSource>().playOnAwake = false;
			GetComponent<AudioSource>().loop = false;
			GetComponent<AudioSource>().clip = foodPickup;
		}
		
		// make sure our localScale is correct for a GUItexture
		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;
		transform.localScale = Vector3.one;
		
		// we create a texture for our GUITexture mainTexture
		foodTexture = TextureHelper.CreateTexture(20,20,Color.red);
		
		// set a random position
		// initialize pixelInset random positions
		int ranX = Random.Range(0, initXPos.Length);
		int ranY = Random.Range(0, initYPos.Length);
		
		// assign a random position to the pixelInset
		foodPos = new Rect(initXPos[ranX],initYPos[ranY],20,20);
	}
}
