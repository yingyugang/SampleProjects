using UnityEngine.Events;
using UnityEngine.UI;

public class GachaDetail : ViewWithDefaultAction
{
	public Text detailTitle;
	public Text detailDesc;
	public Image image;

	protected override void AddActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				Send (0);
			}
		};
	}

	public void Show (string detail_title, string detail_desc)
	{
		detailTitle.text = detail_title;
		detailDesc.text = detail_desc;
	}
}
