using System.Collections;
using UnityEngine;

namespace Snake
{
	public class InputKeyboardController : IInputController 
	{
		private readonly string m_horizontalAxis = "Horizontal";
		private readonly string m_verticalAxis = "Vertical";

		#region IInputController implementation

		public Vector2 GetDirectionValue (Vector2 currentDirection)
		{
			var vertical = Input.GetAxis (m_verticalAxis);
			var horizontal = Input.GetAxis (m_horizontalAxis);

			if (horizontal > 0)
			{
				currentDirection = Vector2.right;
			}
			else if (horizontal < 0)
			{
				currentDirection = Vector2.left;
			}
			else if (vertical > 0)
			{
				currentDirection = Vector2.up;
			}
			else if (vertical < 0)
			{
				currentDirection = Vector2.down;
			}

			return currentDirection;
		}

		#endregion
	}
}