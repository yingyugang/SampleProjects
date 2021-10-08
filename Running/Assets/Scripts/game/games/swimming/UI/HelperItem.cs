using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Swimming
{
	public class HelperItem : MonoBehaviour 
	{
		public int count;
		public Image imgUnknown;
		public Image imgHelper;
		public Text txtCount;

		// Use this for initialization
		void Start () 
		{
			Reset();
		}
		
		public void SetCount(int value)
		{
			count = value;

			if (count >= 1)
			{
				imgHelper.gameObject.SetActive(true);
				imgUnknown.enabled = false;
			}

			if (count >= 2)
			{
				txtCount.gameObject.SetActive(true);
				txtCount.text = count.ToString();
			}
		}

		public void Reset()
		{
			count = 0;
			imgUnknown.enabled = true;
			imgHelper.gameObject.SetActive(false);
			txtCount.gameObject.SetActive(false);
		}
	}
}