using System.Collections;
using UnityEngine;

namespace Snake
{
	public class NumberItem : Item
	{
		public int Value { get; private set; }

		#region implemented abstract members of Item

		public override ItemKind Kind 
		{
			get 
			{
				return ItemKind.Number;
			}
		}

		#endregion

		private void OnBecameVisible ()
		{
			int minValue = 1;
			int maxValue = 3;

			Value = Random.Range (minValue, maxValue);

			GameUI.Instance.UpdateNumberItemValue (Value);	
		}

		public override void Hide ()
		{
			base.Hide ();

			GameManager.Instance.SpawnNumberItem ();
		}
	}
}