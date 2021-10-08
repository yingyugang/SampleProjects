using UnityEngine;
using UnityEngine.UI;

namespace Game.Common.Debugging
{
	public class FPS : MonoBehaviour
	{
		public Text textField;
		private float updateInterval = 0.5f;
		private float lastInterval;
		private int frameCount = 0;

		private void Update ()
		{
			++frameCount;
			if (Time.realtimeSinceStartup > lastInterval + updateInterval) {
				string strInfo = "";
				strInfo = string.Format ("FPS: {0}", Mathf.Round (frameCount / (Time.realtimeSinceStartup - lastInterval)));
				frameCount = 0;
				lastInterval = Time.realtimeSinceStartup;
				textField.text = strInfo;
			}
		}
	}
}