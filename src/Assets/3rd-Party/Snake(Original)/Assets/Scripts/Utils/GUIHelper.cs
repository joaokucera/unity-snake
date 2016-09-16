using UnityEngine;
using System.Collections;

public class GUIHelper : MonoBehaviour 
{
	// method to create a GUIText object in the game
	public static GUIText CreateGetGUIText(Vector2 offset, string strText, float layer)
	{
		// over load to add a name to the gameObject created
		return CreateGetGUIText(offset, "GUITextObject", strText, layer);
	}
	
	// method to create a GUIText object in the game
	public static GUIText CreateGetGUIText(Vector2 offset, string name, string strText, float layer)
	{
		// we need a new game object to hold the component
		GameObject guiTextObject = new GameObject(name);
		
		// set some gameObject properties
		guiTextObject.transform.position = new Vector3(0, 0, layer);
		guiTextObject.transform.rotation = Quaternion.identity;
		guiTextObject.transform.localScale = Vector3.one;
		
		// add the GUIText component for display
		GUIText guiDisplayText = guiTextObject.AddComponent<GUIText>();
		
		// we set the position to the Vector2 offset passed
		guiDisplayText.pixelOffset = offset;
		
		// we set the text to the string strText passed
		guiDisplayText.text = strText;
		
		// finally we return the GUIText component for game manipulation
		return guiDisplayText;
	}
	
	// method to create a GUITexture object in game
	public static void CreateGUITexture(Rect coorindates, Color colTexture, float layer)
	{
		// over load to add a name to the gameObject created
		CreateGUITexture(coorindates, colTexture, "GUITextureOBject", layer);
	}
	
	public static void CreateGUITexture(Rect coorindates, Color colTexture, string name, float layer)
	{
		// we need a new game object to hold the component
		GameObject guiTextureObject = new GameObject(name);
		
		// set some gameObject properties
		guiTextureObject.transform.position = new Vector3(0, 0, layer);
		guiTextureObject.transform.rotation = Quaternion.identity;
		guiTextureObject.transform.localScale = new Vector3(0.01f, 0.01f, 1.0f);
		
		// add our GUITexture Component
		GUITexture guiDisplayTexture = guiTextureObject.AddComponent<GUITexture>();
		
		// create a simple 1x1 black texture
		Texture2D guiTexture = TextureHelper.Create1x1Texture(colTexture); 
		
		// set some GUITexture properties
		guiDisplayTexture.texture = guiTexture;
		guiDisplayTexture.pixelInset = coorindates;
	}
	
	// method to create a GUITexture object in game
	public static GUITexture CreateGetGUITexture(Rect coorindates, Color colTexture, float layer)
	{
		// over load to add a name to the gameObject created
		return CreateGetGUITexture(coorindates, colTexture, "GUITextureOBject", layer);
	}
	
	public static GUITexture CreateGetGUITexture(Rect coorindates, Color colTexture, string name, float layer)
	{
		// we need a new game object to hold the component
		GameObject guiTextureObject = new GameObject(name);
		
		// set some gameObject properties
		guiTextureObject.transform.position = new Vector3(0, 0, layer);
		guiTextureObject.transform.rotation = Quaternion.identity;
		guiTextureObject.transform.localScale = new Vector3(0.01f, 0.01f, 1.0f);
		
		// add our GUITexture Component
		GUITexture guiDisplayTexture = guiTextureObject.AddComponent<GUITexture>();
		
		// create a simple 1x1 black texture
		Texture2D guiTexture = TextureHelper.Create1x1Texture(colTexture); 
		
		// set some GUITexture properties
		guiDisplayTexture.texture = guiTexture;
		guiDisplayTexture.pixelInset = coorindates;
		
		// return our GUITexture
		return guiDisplayTexture;
	}
}
