using UnityEngine;

namespace Snake
{
	[RequireComponent(typeof(SpriteRenderer))]
	public abstract class RendererBehaviour : MonoBehaviour
	{
		protected SpriteRenderer SpriteRenderer { get; private set; }

		private void Awake ()
		{
			SpriteRenderer = GetComponent<SpriteRenderer> ();
		}

		public abstract void OnBecameInvisible ();
	}
}