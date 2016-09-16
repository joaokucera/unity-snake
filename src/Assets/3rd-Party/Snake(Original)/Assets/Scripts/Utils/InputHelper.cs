using UnityEngine;
using System.Collections;

public class InputHelper
{
	// handle multi Input
	public static bool GetStandardMoveMultiInputKeys()
	{
		// check W
		if (Input.GetKey (KeyCode.W) && Input.GetKey (KeyCode.A)) { return true; }		
		if (Input.GetKey (KeyCode.W) && Input.GetKey (KeyCode.S)) { return true; }		
		if (Input.GetKey (KeyCode.W) && Input.GetKey (KeyCode.D)) { return true; }
		
		// check A
		if (Input.GetKey (KeyCode.A) && Input.GetKey (KeyCode.S)) { return true; }		
		if (Input.GetKey (KeyCode.A) && Input.GetKey (KeyCode.D)) { return true; }
		
		// check S
		if (Input.GetKey (KeyCode.S) && Input.GetKey (KeyCode.D)) { return true; }
		
		// D is resulted in the above checks
		
		// check UpArrow
		if (Input.GetKey (KeyCode.UpArrow) && Input.GetKey (KeyCode.LeftArrow))  { return true; }
		if (Input.GetKey (KeyCode.UpArrow) && Input.GetKey (KeyCode.DownArrow))  { return true; }
		if (Input.GetKey (KeyCode.UpArrow) && Input.GetKey (KeyCode.RightArrow)) { return true; }
		
		// check LeftArrow
		if (Input.GetKey (KeyCode.LeftArrow) && Input.GetKey (KeyCode.DownArrow))  { return true; }
		if (Input.GetKey (KeyCode.LeftArrow) && Input.GetKey (KeyCode.RightArrow)) { return true; }
		
		// check DownArrow
		if (Input.GetKey (KeyCode.DownArrow) && Input.GetKey (KeyCode.RightArrow)) { return true; }
		
		// RightArrow is resulted in the checks above
		
		return false;
	}
	
	// handle up direction
	public static bool GetStandardMoveUpDirection()
	{
		if (Input.GetKey (KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) { return true; }
		
		return false;
	}
	
	// handle left direction
	public static bool GetStandardMoveLeftDirection()
	{
		if (Input.GetKey (KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) { return true; }
		
		return false;
	} 
	
	// handle down direction
	public static bool GetStandardMoveDownDirection()
	{
		if (Input.GetKey (KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) { return true; }
		
		return false;
	} 
	
	// handle left direction
	public static bool GetStandardMoveRightDirection()
	{
		if (Input.GetKey (KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) { return true; }
		
		return false;
	} 
}

