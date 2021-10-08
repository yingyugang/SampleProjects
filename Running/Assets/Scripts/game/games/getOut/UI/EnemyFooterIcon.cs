using UnityEngine;
using System.Collections;

public class EnemyFooterIcon : MonoBehaviour {
	public GameObject ImageIcon;
	public GameObject ImageQuestionMark;
	public void SetImageIcon()
	{
		ImageQuestionMark.SetActive(false);
		ImageIcon.SetActive(true);
	}
}
