using System.Collections;
using UnityEngine;

namespace Snake
{
	[AddComponentMenu("SNAKE/Item Spawner")]
	public class ItemSpawner : Pooling <Item> 
	{	
		private Coroutine m_hideCoroutine;

		[Range(0f, 15f)] [SerializeField] private float m_hideCountdown;

		public void Spawn (float width, float height)
		{
			if (m_hideCoroutine != null) 
			{
				StopCoroutine (m_hideCoroutine);
			}

			int x = (int) Random.Range (0, width);
			int y = (int) Random.Range (0, height);

			var item = GetFromPool ();
			if (item != null) 
			{
				item.transform.localPosition = new Vector2 (x, y);

				m_hideCoroutine = StartCoroutine (Hide (item));
			}
		}

		private IEnumerator Hide (Item item)
		{
			yield return new WaitForSeconds (m_hideCountdown);

			item.Hide ();
		}
	}
}