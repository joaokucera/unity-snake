using UnityEngine;
using System.Collections;

/// <summary>
/// Stores game state and game information
/// </summary>

public class GameManager : MonoBehaviour 
{
	// ---------------------------------------------------------------------------------------------------
	// Start()
	// ---------------------------------------------------------------------------------------------------
	// Unity method, called at game start automatically
	// ---------------------------------------------------------------------------------------------------
	void Start () 
	{
		// build our SnakeGame object
		SnakePlayer.Instance.Initialize();
		
		// build our Food object
		Food.Instance.Initialize();
		
		// build our Snake object
		SnakeMove.Instance.Initialize();
	}	
}
