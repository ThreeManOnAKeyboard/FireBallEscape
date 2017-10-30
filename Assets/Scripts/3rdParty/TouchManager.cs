using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _3rdParty
{
	public class TouchManager : MonoBehaviour
	{
		public static TouchManager instance;

		// Use this for initialization
		private void Awake()
		{
			instance = this;
		}

		// Verifies if touch is pointed over UI an object
		public bool IsPointerOverUIObject(int touchIndex)
		{
			PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
			eventDataCurrentPosition.position = new Vector2(Input.GetTouch(touchIndex).position.x, Input.GetTouch(touchIndex).position.y);
			List<RaycastResult> results = new List<RaycastResult>();
			EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
			return results.Count > 0;
		}
	}
}
