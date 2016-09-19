using UnityEngine;
using UnityEngine.EventSystems;

namespace Snake
{
	[AddComponentMenu("SNAKE/Input Touch Pointer")]
	public class InputTouchPointer : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		public bool IsClicked { get; private set; }

		#region IPointerDownHandler implementation

		public void OnPointerDown (PointerEventData eventData)
		{
			IsClicked = true;
		}

		#endregion

		#region IPointerUpHandler implementation

		public void OnPointerUp (PointerEventData eventData)
		{
			IsClicked = false;
		}

		#endregion
	}
}