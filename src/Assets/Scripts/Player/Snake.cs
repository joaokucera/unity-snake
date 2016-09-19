using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Snake
{
	[AddComponentMenu("SNAKE/Snake")]
	[RequireComponent (typeof (BoxCollider2D))]
	[RequireComponent (typeof (Rigidbody2D))]
	public class Snake : MonoBehaviour 
	{
		private readonly string m_itemTag = "Item";
		private readonly float m_fasterMoveSpeed = .1f;

		private IInputController m_inputController;
		private Vector2 m_direction;
		private float m_startMoveSpeed;
		private List<Transform> m_tail;
		private Coroutine m_moveCoroutine;
		private Item m_pickedUpItem;

		[SerializeField] private TailSpawner m_tailSpawner;
		[Range(0f, 2f)] [SerializeField] private float m_moveSpeed;
		[Range(0f, 1f)] [SerializeField] private float m_incrementSpeed;
		[Range(0f, 5f)] [SerializeField] private float m_bonusSlowdown;

		public static SnakeData Data;

		private void Awake ()
		{
			Data = new SnakeData ();
		}

		private void Start ()
		{
			m_inputController = InputFactory.GetCurrentInputController ();
			m_direction = Vector2.right;
			m_startMoveSpeed = m_moveSpeed;
			m_tail = new List<Transform> ();

			m_moveCoroutine = StartCoroutine (Move ());
		}

		private void Update ()
		{
			m_direction = m_inputController.GetDirectionValue (m_direction);
		}

		private void OnTriggerEnter2D (Collider2D collider)
		{
			if (collider.CompareTag (m_itemTag))
			{
				SoundManager.Instance.PlaySoundEffect ("Item Pickup");

				m_pickedUpItem = collider.GetComponent <Item> ();
				m_pickedUpItem.Hide ();
			}
			else 
			{
				Stop ();

				GameManager.Instance.GameOver ();
			}
		}
			
		private IEnumerator Move ()
		{
			var sfxMove = new string[] { "Move1 Blip", "Move2 Blip" };

			while (true)
			{
				yield return new WaitForSeconds (m_moveSpeed);

				var currentPosition = transform.localPosition;

				SoundManager.Instance.PlaySoundEffect (sfxMove [Random.Range (0, sfxMove.Length)]);
				transform.Translate (m_direction);

				if (m_pickedUpItem != null)
				{
					ExecuteItemBehaviour (currentPosition);

					m_pickedUpItem = null;
				}
				else if (m_tail.Count > 0) 
				{
					m_tail.Last ().localPosition = currentPosition;

					m_tail.Insert (0, m_tail.Last());
					m_tail.RemoveAt (m_tail.Count - 1);
				}
			}
		}

		private void ExecuteItemBehaviour (Vector2 currentPosition)
		{
			switch (m_pickedUpItem.Kind)
			{
				case ItemKind.Number:
					IncreaseTailAndSpeed (currentPosition);
					break;
				case ItemKind.Bonus:
					SlowdownSpeedTemporarily ();
					break;
				case ItemKind.Reverse:
					ReverseSnakeHead ();
					break;
			}
		}

		private void IncreaseTailAndSpeed (Vector2 currentPosition)
		{
			int value = (m_pickedUpItem as NumberItem).Value;
			GameUI.Instance.UpdateLength (value);

			for (int i = 0; i < value; i++)
			{
				var newTail = m_tailSpawner.Spawn (currentPosition);
				m_tail.Insert (0, newTail);
			}

			if (!GameUI.Instance.IsBonusItemImageEnabled)
			{
				m_moveSpeed -= m_incrementSpeed;
				m_moveSpeed = Mathf.Clamp (m_moveSpeed, m_fasterMoveSpeed, m_startMoveSpeed);
			}
		}

		private void SlowdownSpeedTemporarily ()
		{
			GameUI.Instance.UpdateBonusImage (true);

			var currentMoveSpeed = m_moveSpeed;
			m_moveSpeed = m_startMoveSpeed;

			StartCoroutine (RestoreSpeed (currentMoveSpeed));
		}

		private IEnumerator RestoreSpeed (float lastMoveSpeed)
		{
			yield return new WaitForSeconds (m_bonusSlowdown);

			GameUI.Instance.UpdateBonusImage (false);

			m_moveSpeed = lastMoveSpeed;
		}

		private void ReverseSnakeHead ()
		{
			if (m_tail.Count == 0) 
			{
				return;
			}

			var lastTail = m_tail.Last ();
			var currentPosition = transform.localPosition;

			transform.localPosition = lastTail.localPosition;
			lastTail.localPosition = currentPosition;

			m_tail.Reverse ();

			if (m_direction == Vector2.right)
			{
				m_direction = Vector2.left;
			}
			else if (m_direction == Vector2.left)
			{
				m_direction = Vector2.right;
			}
			else if (m_direction == Vector2.up)
			{
				m_direction = Vector2.down;
			}
			else if (m_direction == Vector2.down)
			{
				m_direction = Vector2.up;
			}
		}

		private void Stop ()
		{
			StopCoroutine (m_moveCoroutine);
		}
	}
}