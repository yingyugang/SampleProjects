using UnityEngine;
using System.Collections;

namespace Swimming
{

	public class Ending : MonoBehaviour 
	{
		[HideInInspector]
		public bool canCloseEnding = false;

		void OnEnable()
		{
			canCloseEnding = false;
		}

		public void SetCanCloseEnding()
		{
			canCloseEnding = true;
		}

		public void CloseEnding()
		{
			if (!canCloseEnding)
				return;
			
			GUIManager.Instance.CloseEnding();
		}

		public void CloseOpening()
		{
			GUIManager.Instance.CloseOpening();
		}
	}
}