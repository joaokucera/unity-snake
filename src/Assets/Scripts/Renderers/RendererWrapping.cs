using UnityEngine;

namespace Snake
{
	[AddComponentMenu("SNAKE/Renderer Wrapping")]
	public class RendererWrapping : RendererBehaviour 
	{
		private Camera m_mainCamera;

		void Start()
		{
			m_mainCamera = Camera.main;
		}

		public override void OnBecameInvisible ()
		{
			if (m_mainCamera == null) 
			{
				return;
			}

			var position = transform.position;

			if (position.x < -m_mainCamera.orthographicSize * m_mainCamera.aspect || position.x > m_mainCamera.orthographicSize * m_mainCamera.aspect)
			{
				position.x *= -1;
			}
			if (position.y < -m_mainCamera.orthographicSize || position.y > m_mainCamera.orthographicSize)
			{
				position.y *= -1;
			}

			transform.position = position;
		}
	}
}