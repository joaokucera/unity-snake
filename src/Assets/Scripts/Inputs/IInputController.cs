using System.Collections;
using UnityEngine;

namespace Snake
{
	public interface IInputController
	{
		Vector2 GetDirectionValue (Vector2 currectDirection);
	}
}