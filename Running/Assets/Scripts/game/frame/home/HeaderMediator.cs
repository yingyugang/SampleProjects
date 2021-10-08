using UnityEngine;
using System.Collections;
using System.Linq;

namespace home
{
	public class HeaderMediator : MonoBehaviour
	{
		public Header header;
		public ApMediator apMediator;
		public HeaderSpriteGetter headerSpriteGetter;
		public PopupLoader popupLoader;

		private void Start ()
		{
			UpdateName ();
			UpdateCoinAndMoney ();
			header.button.onClick.AddListener (OnClickHandler);
			header.lvField.text = Player.GetInstance.lv.ToString ();
		}

		private void OnDestroy()
		{
			header.button.onClick.RemoveListener (OnClickHandler);
		}

		private void OnClickHandler()
		{
			popupLoader.Popup (PopupEnum.ShowCoin);
		}

		public void SetCurrentHeaderImage ()
		{
			if (string.IsNullOrEmpty (Player.GetInstance.head_image)) {
				return;
			}
            CardCSVStructure cardCSVStructure = MasterCSV.cardCSV.FirstOrDefault (result => int.Parse (Player.GetInstance.head_image).ToString (LanguageJP.FOUR_MASK) == int.Parse (result.image_resource).ToString (LanguageJP.FOUR_MASK));
            GameConstant.currentHeadImageID = cardCSVStructure.id;

            headerSpriteGetter.GetSprite (GetResource);
		}

		private void GetResource (Sprite sprite, Sprite frame, Sprite rate)
		{
			header.headImageFrameField.sprite = frame;
			header.headImageField.sprite = sprite;
			header.headImageRateField.sprite = rate;
			header.headImageField.gameObject.SetActive (true);
		}

		public void SetHeaderImage (Sprite sprite, Sprite frame, Sprite rate)
		{
			header.headImageField.sprite = sprite;
			header.headImageFrameField.sprite = frame;
			header.headImageRateField.sprite = rate;
		}

		public void UpdateName ()
		{
			header.nameField.text = Player.GetInstance.name;
		}

		public void UpdateAp ()
		{
			apMediator.SetApWithServerData ();
		}

		public void UpdateCoinAndMoney ()
		{
			header.coinField.text = Player.GetInstance.free_ticket_num.ToString ();
			header.moneyField.text = Player.GetInstance.ticket_num.ToString ();
		}
	}
}