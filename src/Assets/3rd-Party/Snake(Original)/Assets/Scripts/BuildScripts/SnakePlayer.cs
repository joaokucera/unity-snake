using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SnakePlayer : MonoBehaviour
{
	// private static instance of class
	private static SnakePlayer instance = null;
	
	// private fields
	private List<Rect> snakePos = new List<Rect>();
	private List<Texture2D> snakeIcon = new List<Texture2D>();
	private int snakeLength = 2;
	private float moveDelay = 0.5f;
	private AudioClip move1;
	private AudioClip move2;
	private AudioClip death;
	
	// direction enum for clarification
	public enum Direction
	{
		UP,
		DOWN,
		LEFT,
		RIGHT
	}
		
	// ---------------------------------------------------------------------------------------------------
	// constructor field: Instance
	// --------------------------------------------------------------------------------------------------- 
	// Creates an instance of Snake if one does not exists
	// ---------------------------------------------------------------------------------------------------
	public static SnakePlayer Instance
	{
		get 
		{
			if (instance == null)
			{
				instance = new GameObject("Snake").AddComponent<SnakePlayer>();
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
	// Destroys the ScreenField instance
	// ---------------------------------------------------------------------------------------------------
	
	public void DestroyInstance()
	{
		print("Snake Instance destroyed");
		
		instance = null;
	}
	
	// ---------------------------------------------------------------------------------------------------
	// Start()
	// ---------------------------------------------------------------------------------------------------
	// Unity MonoBehavior call, runs auto when object is created, used for initialization
	// ---------------------------------------------------------------------------------------------------
	void Start ()
	{
		// start our SnakeUpdate loop
		StartCoroutine(UpdateSnake());
	}
	
	// ---------------------------------------------------------------------------------------------------
	// UpdateSnake()
	// ---------------------------------------------------------------------------------------------------
	// Our main player loop, this is what runs the logic for the snake, movement, food, adding segments
	// ---------------------------------------------------------------------------------------------------
	IEnumerator UpdateSnake ()
	{
		while(true)
		{
			// handle multi key presses
			if (InputHelper.GetStandardMoveMultiInputKeys())
			{
				Debug.Log ("We are pressing multiple keys for direction");
				yield return null;
				continue;				
			}
			
			// are we moving up								
			if (InputHelper.GetStandardMoveUpDirection())
			{
				yield return StartCoroutine(MoveSnake(Direction.UP));
				//Debug.Log ("We are moving UP");
			}
			
			// are we moving left
			if (InputHelper.GetStandardMoveLeftDirection())
			{
				yield return StartCoroutine(MoveSnake(Direction.LEFT));
				//Debug.Log ("We are moving LEFT");
			}
			
			// are we moving down
			if (InputHelper.GetStandardMoveDownDirection())
			{
				yield return StartCoroutine(MoveSnake(Direction.DOWN));
				//Debug.Log ("We are moving DOWN");
			}
			
			if (InputHelper.GetStandardMoveRightDirection())
			{
				yield return StartCoroutine(MoveSnake(Direction.RIGHT));
				//Debug.Log ("We are moving RIGHT");
			}	
			
			// here we check for snake collision (it can only collide with itself)
			if (SnakeCollidedWithSelf() == true)
			{
				break;
			}
			
			yield return new WaitForSeconds(moveDelay);		
		}
		
		// play our death sound
		GetComponent<AudioSource>().clip = death;
		GetComponent<AudioSource>().Play();
		
		// we are hit
		yield return StartCoroutine(ScreenHelper.FlashDeathScreen(6, 0.1f, new Color(1, 0, 0, 0.5f)));
		
		// we reduce number of lives
		SnakeGame.Instance.UpdateLives(-1);
		
		// check for playable lives left
		if (SnakeGame.Instance.gameLives == 0)
		{
			// reload the level, resetting the game
//			Application.LoadLevel("Uni2DSnake");
		}
		else
		{
			// here we handle resetting the snake after a collision
			Initialize();
		
			// we have to call Start manually because this object is already Instantiated
			Start();
		}		
	}
	
	// ---------------------------------------------------------------------------------------------------
	// MoveSnake()
	// ---------------------------------------------------------------------------------------------------
	// Moves the snake texture (pixel movement / update snake texture Rect)
	// ---------------------------------------------------------------------------------------------------
	public IEnumerator MoveSnake(Direction moveDirection)
	{
		// define a temp List of Rects to our current snakes List of Rects
		List<Rect> tempRects = new List<Rect>();
		Rect segmentRect = new Rect(0,0,0,0);
		
		// initialize
		for (int i = 0; i < snakePos.Count; i++)
		{
			tempRects.Add(snakePos[i]);
		}
		
		switch(moveDirection)
		{
			case Direction.UP:
				if (snakePos[0].y > 94)
				{
					// we can move up
					snakePos[0] = new Rect(snakePos[0].x, snakePos[0].y - 20, snakePos[0].width, snakePos[0].height);
					
					// now update the rest of our body
					UpdateMovePosition(tempRects);
					
					// check for food
					if (CheckForFood() == true)
					{
						// check for valid build segment position and add a segment
						
						// create a temporary check position (this one is below the last segment in snakePos[])
						segmentRect = CheckForValidDownPosition();
						if (segmentRect.x != 0)
						{
							// we build another segment passing the Rect as an argument
							BuildSnakeSegment(segmentRect);
							
							// increment our snake length
							snakeLength++;
							
							// decrement our moveDelay
							moveDelay = Mathf.Max(0.05f, moveDelay - 0.01f); 
							
							// give control back to our calling method
							yield break;
						}
						
						// create a temporary check position (this one is to the left the last segment in snakePos[])
						segmentRect = CheckForValidLeftPosition();
						if (segmentRect.x != 0)
						{
							// we build another segment passing the Rect as an argument
							BuildSnakeSegment(segmentRect);
							
							// increment our snake length
							snakeLength++;
							
							// decrement our moveDelay
							moveDelay = Mathf.Max(0.05f, moveDelay - 0.01f); 
							
							// give control back to our calling method
							yield break;
						}
						
						// create a temporary check position (this one is to the right the last segment in snakePos[])
						segmentRect = CheckForValidRightPosition();
						if (segmentRect.x != 0)
						{
							// we build another segment passing the Rect as an argument
							BuildSnakeSegment(segmentRect);
							
							// increment our snake length
							snakeLength++;
							
							// decrement our moveDelay
							moveDelay = Mathf.Max(0.05f, moveDelay - 0.01f); 
						}	
						
						// no need to check Up, because we are pressing the Up key, we do not want a segment above us		
					}
					
					// toggle the audio clip and play
					GetComponent<AudioSource>().clip = (GetComponent<AudioSource>().clip == move1) ? move2 : move1;
					GetComponent<AudioSource>().Play();					
				}
				break;
			case Direction.LEFT:
				if (snakePos[0].x > 22)
				{
					// we can move left
					snakePos[0] = new Rect(snakePos[0].x - 20, snakePos[0].y, snakePos[0].width, snakePos[0].height);
					
					// now update the rest of our body
					UpdateMovePosition(tempRects);
					
					// check for food
					if (CheckForFood() == true)
					{
						// check for valid build segment position and add a segment
						
						// create a temporary check position (this one is to the right the last segment in snakePos[])
						segmentRect = CheckForValidRightPosition();
						if (segmentRect.x != 0)
						{
							// we build another segment passing the Rect as an argument
							BuildSnakeSegment(segmentRect);
							
							// increment our snake length
							snakeLength++;
							
							// decrement our moveDelay
							moveDelay = Mathf.Max(0.05f, moveDelay - 0.01f); 
							
							// give control back to our calling method
							yield break;
						}	
						
						// create a temporary check position (this one is above the last segment in snakePos[])
						segmentRect = CheckForValidUpPosition();
						if (segmentRect.x != 0)
						{
							// we build another segment passing the Rect as an argument
							BuildSnakeSegment(segmentRect);
							
							// increment our snake length
							snakeLength++;
							
							// decrement our moveDelay
							moveDelay = Mathf.Max(0.05f, moveDelay - 0.01f); 
							
							// give control back to our calling method
							yield break;
						}
						
						// create a temporary check position (this one is below the last segment in snakePos[])
						segmentRect = CheckForValidDownPosition();
						if (segmentRect.x != 0)
						{
							// we build another segment passing the Rect as an argument
							BuildSnakeSegment(segmentRect);
							
							// increment our snake length
							snakeLength++;
							
							// decrement our moveDelay
							moveDelay = Mathf.Max(0.05f, moveDelay - 0.01f); 
							
							// give control back to our calling method
							yield break;
						}
						
						// no need to check Left, because we are pressing the Left key, we do not want a segment ahead of us		
					}
					
					// toggle the audio clip and play
					GetComponent<AudioSource>().clip = (GetComponent<AudioSource>().clip == move1) ? move2 : move1;
					GetComponent<AudioSource>().Play();
				}
				break;
			case Direction.DOWN:
				if (snakePos[0].y < 654)
				{
					// we can move down
					snakePos[0] = new Rect(snakePos[0].x, snakePos[0].y + 20, snakePos[0].width, snakePos[0].height);
					
					// now update the rest of our body
					UpdateMovePosition(tempRects);
					
					// check for food
					if (CheckForFood() == true)
					{
						// check for valid build segment position and add a segment
						
						// create a temporary check position (this one is above the last segment in snakePos[])
						segmentRect = CheckForValidUpPosition();
						if (segmentRect.x != 0)
						{
							// we build another segment passing the Rect as an argument
							BuildSnakeSegment(segmentRect);
							
							// increment our snake length
							snakeLength++;
							
							// decrement our moveDelay
							moveDelay = Mathf.Max(0.05f, moveDelay - 0.01f); 
							
							// give control back to our calling method
							yield break;
						}
						
						// create a temporary check position (this one is to the left the last segment in snakePos[])
						segmentRect = CheckForValidLeftPosition();
						if (segmentRect.x != 0)
						{
							// we build another segment passing the Rect as an argument
							BuildSnakeSegment(segmentRect);
							
							// increment our snake length
							snakeLength++;
							
							// decrement our moveDelay
							moveDelay = Mathf.Max(0.05f, moveDelay - 0.01f); 
							
							// give control back to our calling method
							yield break;
						}
						
						// create a temporary check position (this one is to the right the last segment in snakePos[])
						segmentRect = CheckForValidRightPosition();
						if (segmentRect.x != 0)
						{
							// we build another segment passing the Rect as an argument
							BuildSnakeSegment(segmentRect);
							
							// increment our snake length
							snakeLength++;
							
							// decrement our moveDelay
							moveDelay = Mathf.Max(0.05f, moveDelay - 0.01f); 
						}	
						
						// no need to check Down, because we are pressing the Down key, we do not want a segment below us		
					}
					
					// toggle the audio clip and play
					GetComponent<AudioSource>().clip = (GetComponent<AudioSource>().clip == move1) ? move2 : move1;
					GetComponent<AudioSource>().Play();
				}
				break;
			case Direction.RIGHT:
				if (snakePos[0].x < 982)
				{
					// we can move right
					snakePos[0] = new Rect(snakePos[0].x + 20, snakePos[0].y, snakePos[0].width, snakePos[0].height);
					
					// now update the rest of our body
					UpdateMovePosition(tempRects);
					
					// check for food
					if (CheckForFood() == true)
					{
						// check for valid build segment position and add a segment
						
						// create a temporary check position (this one is left of the last segment in snakePos[])
						segmentRect = CheckForValidLeftPosition();
						if (segmentRect.x != 0)
						{
							// we build another segment passing the Rect as an argument
							BuildSnakeSegment(segmentRect);
							
							// increment our snake length
							snakeLength++;
							
							// decrement our moveDelay
							moveDelay = Mathf.Max(0.05f, moveDelay - 0.01f); 
							
							// give control back to our calling method
							yield break;
						}
						
						// create a temporary check position (this one is to the left the last segment in snakePos[])
						segmentRect = CheckForValidUpPosition();
						if (segmentRect.x != 0)
						{
							// we build another segment passing the Rect as an argument
							BuildSnakeSegment(segmentRect);
							
							// increment our snake length
							snakeLength++;
							
							// decrement our moveDelay
							moveDelay = Mathf.Max(0.05f, moveDelay - 0.01f); 
							
							// give control back to our calling method
							yield break;
						}
						
						// create a temporary check position (this one is below the last segment in snakePos[])
						segmentRect = CheckForValidDownPosition();
						if (segmentRect.x != 0)
						{
							// we build another segment passing the Rect as an argument
							BuildSnakeSegment(segmentRect);
							
							// increment our snake length
							snakeLength++;
							
							// decrement our moveDelay
							moveDelay = Mathf.Max(0.05f, moveDelay - 0.01f); 
						}	
						
						// no need to check Right, because we are pressing the Right key, we do not want a segment ahead of us		
					}
					
					// toggle the audio clip and play
					GetComponent<AudioSource>().clip = (GetComponent<AudioSource>().clip == move1) ? move2 : move1;
					GetComponent<AudioSource>().Play();
				}
				break;
		}
		
		yield return null;
	}
	
	// ---------------------------------------------------------------------------------------------------
	// UpdateMovePosition()
	// ---------------------------------------------------------------------------------------------------
	// Updates the snakePos list of Rect's to the new Rect positions after a move
	// ---------------------------------------------------------------------------------------------------
	private void UpdateMovePosition(List<Rect> tmpRects)
	{
		// update our snakePos Rect with the tmpRect positions
		for (int i = 0; i < tmpRects.Count - 1; i++)
		{
			// exe. size of 3, assign 1,2,3 to 0,1,2 - snakePos[0] is already assigned
			snakePos[i+1] = tmpRects[i];
		} 
	}
	
	// ---------------------------------------------------------------------------------------------------
	// CheckForFood()
	// ---------------------------------------------------------------------------------------------------
	// Checks if the first snake segment is same position as the food on the game field, returns bool
	// ---------------------------------------------------------------------------------------------------
	private bool CheckForFood()
	{
		if(Food.Instance != null)
		{
			Rect foodRect = Food.Instance.foodPos;
			if (snakePos[0].Contains(new Vector2(foodRect.x,foodRect.y)))
			{
				
				Debug.Log ("We hit the food");
				
				// we re-position the food
				Food.Instance.UpdateFood();
				
				// we add to our score
				SnakeGame.Instance.UpdateScore(1);
				
				return true;
			}
		}
		
		return false;
	}
	
	// ---------------------------------------------------------------------------------------------------
	// CheckForValidDownPosition()
	// ---------------------------------------------------------------------------------------------------
	// Checks if the last snake segment is not in the lowest position on the game field, returns Rect
	// ---------------------------------------------------------------------------------------------------
	private Rect CheckForValidDownPosition()
	{
		if (snakePos[snakePos.Count-1].y != 654)
		{
			return new Rect(snakePos[snakePos.Count-1].x, snakePos[snakePos.Count-1].y - 20, 20, 20);
		}
		
		return new Rect(0,0,0,0);
	}
	
	// ---------------------------------------------------------------------------------------------------
	// CheckForValidUpPosition()
	// ---------------------------------------------------------------------------------------------------
	// Checks if the last snake segment is not in the highest position on the game field, returns Rect
	// ---------------------------------------------------------------------------------------------------
	private Rect CheckForValidUpPosition()
	{
		if (snakePos[snakePos.Count-1].y != 94)
		{
			return new Rect(snakePos[snakePos.Count-1].x, snakePos[snakePos.Count-1].y + 20, 20, 20);
		}
		
		return new Rect(0,0,0,0);
	}
	
	// ---------------------------------------------------------------------------------------------------
	// CheckForValidLeftPosition()
	// ---------------------------------------------------------------------------------------------------
	// Checks if the last snake segment is not in the far left position on the game field, returns Rect
	// ---------------------------------------------------------------------------------------------------
	private Rect CheckForValidLeftPosition()
	{
		if (snakePos[snakePos.Count-1].x != 22)
		{
			return new Rect(snakePos[snakePos.Count-1].x - 20, snakePos[snakePos.Count-1].y, 20, 20);
		}
		
		return new Rect(0,0,0,0);
	}
	
	// ---------------------------------------------------------------------------------------------------
	// CheckForValidRightPosition()
	// ---------------------------------------------------------------------------------------------------
	// Checks if the last snake segment is not in the far right position on the game field, returns Rect
	// ---------------------------------------------------------------------------------------------------
	private Rect CheckForValidRightPosition()
	{
		if (snakePos[snakePos.Count-1].x != 982)
		{
			return new Rect(snakePos[snakePos.Count-1].x + 20, snakePos[snakePos.Count-1].y, 20, 20);
		}
		
		return new Rect(0,0,0,0);
	}
	
	// ---------------------------------------------------------------------------------------------------
	// BuildSnakeSegment()
	// ---------------------------------------------------------------------------------------------------
	// Adds a snake segment, pass the Rect to apply the position to
	// ---------------------------------------------------------------------------------------------------
	private void BuildSnakeSegment(Rect rctPos)
	{
		// define our snake head and tail texture
		snakeIcon.Add(TextureHelper.CreateTexture(20, 20, Color.green));
		
		// define our snake head and tail GUI Rect
		snakePos.Add(rctPos);
	}
	
	// ---------------------------------------------------------------------------------------------------
	// SnakeCollidedWithSelf()
	// ---------------------------------------------------------------------------------------------------
	// Checks if the snake has hit any part of its body, returns true/false
	// ---------------------------------------------------------------------------------------------------
	private bool SnakeCollidedWithSelf()
	{
		bool didCollide = false;
		
		if (snakePos.Count <= 4)
		{
			return false;
		}
		
		for (int i = 0; i < snakePos.Count; i++)
		{
			if (i > 0)
			{
				if (snakePos[0].x == snakePos[snakePos.Count - i].x && snakePos[0].y == snakePos[snakePos.Count - i].y)
				{
					// we have collided
					didCollide = true;
					
					break;
				}
			}			
		}
		
		return didCollide;
	}
	
	// ---------------------------------------------------------------------------------------------------
	// ResetSnake()
	// ---------------------------------------------------------------------------------------------------
	// Handles resetting snake back to original
	// ---------------------------------------------------------------------------------------------------
	
	// handles displaying the Snake
	void OnGUI()
	{
		for (int i = 0; i < snakeLength; i++)
		{
			GUI.DrawTexture(snakePos[i], snakeIcon[i]);
		}
	}
	
	// ---------------------------------------------------------------------------------------------------
	// Initialize()
	// --------------------------------------------------------------------------------------------------- 
	// Initializes Snake
	// ---------------------------------------------------------------------------------------------------
	public void Initialize()
	{
		print("Snake initialized");
		
		// clear our Lists
		snakePos.Clear();
		snakeIcon.Clear();
		
		// initialize our length to start length
		snakeLength = 2;
		
		// intialize our moveDelay
		moveDelay = 0.5f;
		
		// add our AudioSource component
		if (!gameObject.GetComponent<AudioSource>())
		{
			// load in our clips
			move1 = Resources.Load("Sounds/Move1 Blip") as AudioClip;
			move2 = Resources.Load("Sounds/Move2 Blip") as AudioClip;
			death = Resources.Load("Sounds/Death") as AudioClip;
			
			gameObject.AddComponent<AudioSource>();
			
			// initialize some audio properties
			GetComponent<AudioSource>().playOnAwake = false;
			GetComponent<AudioSource>().loop = false;
			GetComponent<AudioSource>().clip = move1;
		}
		
		// make sure our localScale is correct for a GUItexture
		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;
		transform.localScale = Vector3.one;
		
		// define our snake head and tail texture
		snakeIcon.Add(TextureHelper.CreateTexture(20, 20, Color.green));
		snakeIcon.Add(TextureHelper.CreateTexture(20, 20, Color.green));
		
		// define our snake head and tail GUI Rect
		snakePos.Add(new Rect(Screen.width * 0.5f - 10, Screen.height * 0.5f - 10, snakeIcon[0].width, snakeIcon[0].height));
		snakePos.Add(new Rect(Screen.width * 0.5f - 10 + 20, Screen.height * 0.5f - 10, snakeIcon[1].width, snakeIcon[1].height));
	}
}

