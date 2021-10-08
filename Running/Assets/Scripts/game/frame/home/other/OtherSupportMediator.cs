using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;

public class OtherSupportMediator : ActivityMediator
{
	private OtherSupport otherSupport;
	public SupportScrollItem Instantiation;

	protected override void CreateActions ()
	{
		otherSupport = viewWithDefaultAction as OtherSupport;
		PopupLoader popupLoader = page.popupLoader;
		unityActionArray = new UnityAction[] {
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se02_ng);
				showWindow (0);
			}
		};
	}

	protected override void InitData ()
	{
		otherSupport = viewWithDefaultAction as OtherSupport;
		List<HelpCSVStructure> list = MasterCSV.helpCSV.Distinct (new HelpCSVStructureComparer ()).ToList ();
		int length = list.Count;
		Instantiator instantiator = Instantiator.GetInstance ();
		for (int i = 0; i < length; i++) {
			SupportScrollItem supportScrollItem = instantiator.Instantiate (Instantiation, Vector2.zero, Vector3.one, otherSupport.container);
			supportScrollItem.titleField.text = list [i].menu_name;
			supportScrollItem.unityAction = (RogerScrollItem rogerScrollItem) => {
				page.popupLoader.Popup (PopupEnum.GameSupport, null, new List<object> {
					(rogerScrollItem as SupportScrollItem).titleField.text
				});
			};
			supportScrollItem.gameObject.SetActive (true);
		}
	}
}

class HelpCSVStructureComparer : IEqualityComparer<HelpCSVStructure>
{
	public bool Equals (HelpCSVStructure helpCSVStructure1, HelpCSVStructure helpCSVStructure2)
	{
		if (helpCSVStructure1 == null) {
			return helpCSVStructure2 == null;
		}
		return helpCSVStructure1.menu_name == helpCSVStructure2.menu_name;
	}

	public int GetHashCode (HelpCSVStructure p)
	{
		if (p == null) {
			return 0;
		}
		return p.menu_name.GetHashCode ();
	}
}