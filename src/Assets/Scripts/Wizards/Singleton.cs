using UnityEngine;

namespace Snake
{
	public abstract class Singleton<T> : MonoBehaviour where T : Component
	{
		[SerializeField] private bool isPersistant;

		public static T Instance { get; private set; }

		public virtual void Awake ()
		{
			if (isPersistant)
			{
				if (Instance == null)
				{
					Instance = this as T;

					DontDestroyOnLoad (this);
				}
				else
				{
					Destroy (gameObject);
				}
			}
			else
			{
				Instance = this as T;
			}
		}
	}
}