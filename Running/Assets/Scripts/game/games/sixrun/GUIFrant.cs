using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace SixRun{
	[ExecuteInEditMode]
	public class GUIFrant : SixRunSingleMono<GUIFrant> {

		public List<SixRunBtn> btns;
		public GameObject frameCanvas;
		public Text comboMax;
		public Text comboCur;

		public bool load;
		void Update(){
			if (load) {
				load = false;
				SixRunBtn[] srbs = GetComponentsInChildren<SixRunBtn> ();
				btns = new List<SixRunBtn> ();
				btns.AddRange (srbs);
			}
		}

	}
}
