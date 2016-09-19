using System.Collections;
using UnityEngine;

namespace Snake
{
	[AddComponentMenu("SNAKE/Game Manager")]
	public class GameManager : Singleton <GameManager> 
	{
		private readonly string m_spawnBonusItemMethodName = "SpawnBonusItem";
		private readonly string m_spawnReverseItemMethodName = "SpawnReverseItem";

		private float screenWidth;
		private float screenHeight;

		[SerializeField] private ItemSpawner m_numberItemSpawner;
		[Range(0f, 60f)] [SerializeField] private float m_bonusItemSpawnTime;
		[SerializeField] private ItemSpawner m_bonusItemSpawner;
		[Range(0f, 60f)] [SerializeField] private float m_reverseItemSpawnTime;
		[SerializeField] private ItemSpawner m_reverseItemSpawner;

		private void Start ()
		{
			var mainCamera = Camera.main;
			float percentageOffset = .9f;

			screenWidth = (mainCamera.orthographicSize * mainCamera.aspect) * percentageOffset;
			screenHeight = mainCamera.orthographicSize * percentageOffset;
		
			InvokeRepeating (m_spawnBonusItemMethodName, m_bonusItemSpawnTime, m_bonusItemSpawnTime);
			InvokeRepeating (m_spawnReverseItemMethodName, m_reverseItemSpawnTime, m_reverseItemSpawnTime);

			SpawnNumberItem ();
		}

		public void GameOver ()
		{
			Stop ();

			GameUI.Instance.ShowGameOver ();	
		}

		public void SpawnNumberItem ()
		{
			m_numberItemSpawner.Spawn (screenWidth, screenHeight);
		}

		private void SpawnBonusItem ()
		{
			m_bonusItemSpawner.Spawn (screenWidth, screenHeight);
		}

		private void SpawnReverseItem ()
		{
			m_reverseItemSpawner.Spawn (screenWidth, screenHeight);
		}

		private void Stop ()
		{
			CancelInvoke (m_spawnBonusItemMethodName);
			CancelInvoke (m_spawnReverseItemMethodName);
		}
	}
}