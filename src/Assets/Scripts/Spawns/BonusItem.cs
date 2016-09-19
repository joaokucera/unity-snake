using System.Collections;
using UnityEngine;

namespace Snake
{
	public class BonusItem : Item
	{
		#region implemented abstract members of Item

		public override ItemKind Kind 
		{
			get 
			{
				return ItemKind.Bonus;
			}
		}

		#endregion
	}
}