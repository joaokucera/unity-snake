using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Snake
{
	[AddComponentMenu("SNAKE/Menu Manager")]
	public class MenuManager : MonoBehaviour 
	{
		[SerializeField] private Button m_playButton;
		[SerializeField] private Text m_highScoreText;

		private void Start ()
		{
			m_playButton.onClick.AddListener (PlayGame);

			UpdateHighScoreText ();
		}

		private void Update ()
		{
			if (Input.GetKeyDown (KeyCode.Escape)) 
			{
				Application.Quit();
			}
		}

		private void PlayGame ()
		{
			SoundManager.Instance.PlaySoundEffect ("Button Click");

			SceneManager.LoadScene ("Game");
		}

		private void UpdateHighScoreText ()
		{
			var highScore = PlayerPrefs.GetInt ("Highscore");

			m_highScoreText.text = string.Format ("HIGH SCORE: <color=#20C020FF>{0}</color>", highScore);
		}
	}
}