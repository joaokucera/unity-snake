using UnityEngine;

namespace Snake
{
	public class SnakeData
	{
		private readonly string m_highscoreKey = "Highscore";

		public int HighScore { get; private set; }
		public int CurrentLength { get; private set; }

		public SnakeData ()
		{
			HighScore = PlayerPrefs.GetInt (m_highscoreKey);

			CurrentLength = 0;
		}

		public void AddLength (int newLength)
		{
			CurrentLength += newLength;
		}

		public void CheckHighScore ()
		{
			if (CurrentLength > HighScore)
			{
				HighScore = CurrentLength;

				PlayerPrefs.SetInt (m_highscoreKey, HighScore);
			}
		}
	}
}