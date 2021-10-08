using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Downloading : MonoBehaviour
{
	public GameObject container;
	public Text progressText;
	public Slider progressSlider;
	public RogerScrollGrid rogerScrollGrid;
	private Dictionary<string,Sprite> dictionary;

	public void UpdateProgress (float f)
	{
		progressText.text = Mathf.Floor (f * 100) + "%";
		progressSlider.value = f;
	}

	public void Clear ()
	{
		progressText.text = "0%";
		progressSlider.value = 0;
	}
}
