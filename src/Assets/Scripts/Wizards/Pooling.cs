using System.Collections.Generic;
using UnityEngine;

namespace Snake
{
	public abstract class Pooling<T> : MonoBehaviour where T : Component
	{
		private List<T> m_pool = new List<T>();

		[Header("Pool Settings")]
		[SerializeField] private T m_prefab;
		[SerializeField] private Transform m_parent;
		[SerializeField] private int m_poolSize = 1;
		[SerializeField] private bool m_poolCanGrow = false;

		public virtual void Awake()
		{
			if (m_prefab == null)
			{
				Debug.LogError("Has not been defined a prefab!");
			}

			GeneratePool();
		}

		protected T GetFromPool (bool active = true)
		{
			for (int i = 0; i < m_pool.Count; i++)
			{
				T obj = m_pool[i];

				if (!obj.gameObject.activeInHierarchy)
				{
					obj.gameObject.SetActive (active);

					return obj;
				}
			}

			if (m_poolCanGrow)
			{
				var obj = CreateNew ();

				obj.gameObject.SetActive (active);

				return obj;
			}

			return null;
		}

		private void GeneratePool ()
		{
			for (int i = 0; i < m_poolSize; i++)
			{
				CreateNew ();
			}
		}

		private T CreateNew ()
		{
			var item = Instantiate (m_prefab, Vector2.zero, Quaternion.identity, m_parent) as T;
			item.gameObject.SetActive (false);

			m_pool.Add(item);

			return item;
		}
	}
}