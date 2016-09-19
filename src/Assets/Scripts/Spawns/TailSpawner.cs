using UnityEngine;

namespace Snake
{
	[AddComponentMenu("SNAKE/Tail Spawner")]
	public class TailSpawner : Pooling<Transform> 
	{
		public Transform Spawn (Vector2 position)
		{
			var tail = GetFromPool ();

			tail.transform.localPosition = position;

			return tail;
		}
	}
}