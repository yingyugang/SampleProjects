using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SwitchButton : MonoBehaviour
{
	public GameObject grey;

	virtual public void SetActive (bool isActive)
	{
		grey.SetActive (!isActive);
	}
}
