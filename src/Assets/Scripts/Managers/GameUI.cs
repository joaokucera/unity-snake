using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Snake
{
	[AddComponentMenu("SNAKE/Game UI")]
	public class GameUI : Singleton<GameUI>
	{
		[Header("In Game UI Components")]
		[SerializeField] private GameObject m_panelInGame;
		[SerializeField] private Text m_currentLengthText;
		[SerializeField] private Text m_previousHighScoreText;
		[SerializeField] private Text m_currentNumberItemValueText;
		[SerializeField] private Image m_bonusItemImage;
		[SerializeField] private Button m_retryButton;
		[SerializeField] private Button m_menuButton;

		[Header("Game Over UI Components")]
		[SerializeField] private GameObject m_panelGameOver;
		[SerializeField] private Text m_finalLengthText;
		[SerializeField] private Text m_newHighScoreText;

		[Header("Touch Controls UI Components")]
		[SerializeField] private GameObject m_panelTouchControls;
		[SerializeField] private InputTouchPointer m_buttonRight;
		[SerializeField] private InputTouchPointer m_buttonLeft;
		[SerializeField] private InputTouchPointer m_buttonUp;
		[SerializeField] private InputTouchPointer m_buttonDown;

		public bool IsBonusItemImageEnabled { get { return m_bonusItemImage.enabled; } }
		public bool IsClickButtonRight { get { return m_buttonRight.IsClicked; } }
		public bool IsClickButtonLeft { get { return m_buttonLeft.IsClicked; } }
		public bool IsClickButtonUp { get { return m_buttonUp.IsClicked; } }
		public bool IsClickButtonDown { get { return m_buttonDown.IsClicked; } }

		private void Start()
		{
			m_retryButton.onClick.AddListener (Retry);
			m_menuButton.onClick.AddListener (Menu);

			UpdateHighScoreText ();
		}

		private void Update()
		{
			if (Input.GetKeyDown (KeyCode.Escape)) 
			{
				Menu ();
			}
		}

		public void Retry ()
		{
			SoundManager.Instance.PlaySoundEffect ("Button Click");

			SceneManager.LoadScene ("Game");
		}

		public void Menu ()
		{
			SoundManager.Instance.PlaySoundEffect ("Button Click");

			SceneManager.LoadScene ("Menu");
		}

		public void ShowTouchControls ()
		{
			m_panelTouchControls.SetActive (true);
		}

		public void ShowGameOver ()
		{
			SoundManager.Instance.PlaySoundEffect ("Death");

			GameOver ();
		}

		public void UpdateBonusImage (bool enable)
		{
			m_bonusItemImage.enabled = enable;
		}

		public void UpdateNumberItemValue (int newValue)
		{
			m_currentNumberItemValueText.text = string.Format ("NUMBER: <color=#20C020FF>{0}</color>", newValue);
		}

		public void UpdateLength (int newLength)
		{
			StartCoroutine (UpdateLengthText (newLength));
		}

		private IEnumerator UpdateLengthText (int newScore)
		{
			var previousLength = Snake.Data.CurrentLength;

			Snake.Data.AddLength (newScore);

			var wait = new WaitForSeconds (.1f);

			while (previousLength < Snake.Data.CurrentLength)
			{
				previousLength++;

				m_currentLengthText.text = string.Format ("LENGTH: <color=#20C020FF>{0}</color>", previousLength);

				yield return wait;
			}

			m_currentLengthText.text = string.Format ("LENGTH: <color=#20C020FF>{0}</color>", Snake.Data.CurrentLength);
		}

		private void UpdateHighScoreText ()
		{
			m_previousHighScoreText.text = string.Format ("HIGH SCORE: <color=#20C020FF>{0}</color>", Snake.Data.HighScore);
		}

		private void GameOver ()
		{
			Snake.Data.CheckHighScore ();

			m_panelInGame.SetActive (false);
			m_panelGameOver.SetActive (true);

			m_finalLengthText.text = string.Format ("FINAL LENGTH: <color=#FFC000FF>{0}</color>", Snake.Data.CurrentLength);
			m_newHighScoreText.text = string.Format ("HIGH SCORE: <color=#FFC000FF>{0}</color>", Snake.Data.HighScore);
		}
	}
}