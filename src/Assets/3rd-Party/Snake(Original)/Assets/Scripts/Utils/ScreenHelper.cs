using UnityEngine;
using System.Collections;

public class ScreenHelper : MonoBehaviour
{
	// ---------------------------------------------------------------------------------------------------
	// FlashDeathScreen()
	// --------------------------------------------------------------------------------------------------- 
	// Flashes the death screen for a number of times, for a delayed amount, with a specified color
	// ---------------------------------------------------------------------------------------------------
	public static IEnumerator FlashDeathScreen(int flashTimes, float flashDelay, Color flashColor)
	{		
		// create our flash screen texture
		GUITexture flashScreenTexture = GUIHelper.CreateGetGUITexture(new Rect(0, 0, 1024, 768), flashColor, 20);
		
		for (int i = 0; i < flashTimes; i++)
		{
			flashScreenTexture.color = flashColor;
			yield return new WaitForSeconds(flashDelay);
			flashScreenTexture.color = Color.clear;
			yield return new WaitForSeconds(flashDelay);
		}
		
		// remove our flash death screen object since we only need this in the game when we die
		Destroy(flashScreenTexture.gameObject);
	}
}