using System.Collections.Generic;
using UnityEngine;

namespace Snake
{
	[AddComponentMenu("SNAKE/Sound Manager")]
	[RequireComponent (typeof (AudioSource))]
	public class SoundManager : Singleton <SoundManager> 
	{
		private AudioSource m_sfxSource;
		private Dictionary<string, AudioClip> m_sfxDictionary;

		[Header("Sound Effects Collection")]
		[SerializeField] private AudioClip[] sfxClips;

		private void Start ()
		{
			m_sfxSource = GetComponent <AudioSource> ();

			if (m_sfxDictionary == null) 
			{
				CreateSoundDictionary ();
			}
		}

		public void PlaySoundEffect (string clipName)
		{
			AudioClip originalClip;

			if (m_sfxDictionary.TryGetValue (clipName, out originalClip))
			{
				MakeSoundEffect (originalClip);
			}
		}

		private void CreateSoundDictionary()
		{
			m_sfxDictionary = new Dictionary<string, AudioClip> ();

			for (int i = 0; i < sfxClips.Length; i++)
			{
				m_sfxDictionary.Add (sfxClips[i].name, sfxClips[i]);
			}
		}

		private void MakeSoundEffect (AudioClip originalClip)
		{
			m_sfxSource.PlayOneShot (originalClip);
		}
	}
}