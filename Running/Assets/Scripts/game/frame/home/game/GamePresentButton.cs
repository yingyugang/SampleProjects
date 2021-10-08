using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

public class GamePresentButton : MonoBehaviour
{
	public Animator animator;
	public GameObject item;
	public GameObject particle;
	public Image image;
	public Text text;

	private void OnEnable ()
	{
		animator.enabled = false;
		item.SetActive (false);
		particle.SetActive (false);
		image.gameObject.SetActive (false);
		text.gameObject.SetActive (false);
	}

	public void Play ()
	{
		ItemCSVStructure currentItemCSVStructure = MasterCSV.itemCSV.FirstOrDefault (result => result.id == ItemBonus.GetInstance.reward_id);
		image.sprite = AssetBundleResourcesLoader.itemIconDictionary [currentItemCSVStructure.image_resource];
		text.text = string.Format ("{0}{1}", LanguageJP.X, ItemBonus.GetInstance.num);
		animator.enabled = true;
	}
}
