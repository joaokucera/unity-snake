using UnityEngine;

namespace Snake
{
	public class InputTouchController : IInputController
	{
		public InputTouchController ()
		{
			GameUI.Instance.ShowTouchControls ();
		}

		#region IInputController implementation

		public Vector2 GetDirectionValue (Vector2 currentDirection)
		{
			if (GameUI.Instance.IsClickButtonRight)
			{
				currentDirection = Vector2.right;
			}
			else if (GameUI.Instance.IsClickButtonLeft)
			{
				currentDirection = Vector2.left;
			}
			else if (GameUI.Instance.IsClickButtonUp)
			{
				currentDirection = Vector2.up;
			}
			else if (GameUI.Instance.IsClickButtonDown)
			{
				currentDirection = Vector2.down;
			}

			return currentDirection;
		}

		#endregion
	}

}