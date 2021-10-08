using UnityEngine;
using System.Collections;

namespace Swimming
{
	public class ImmortalItem : Item 
	{

		protected override void ItemEffect()
		{
			Swimmer.Instance.GetImmortalItem();
		}
	}
}