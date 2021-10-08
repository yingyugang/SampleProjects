using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;

public class OilKing_Animation : MonoBehaviour
{
	private static OilKing_Animation m_Instance;

	private float m_PosBeginY;
	private float m_PosEndY;
	private float m_Distance;
	private int m_PosPause;
	private bool m_CheckFirst;

	//pause when use util skill
	[HideInInspector]
	public bool checkStartUtilSkill;

	//animation util skill
	private float m_TimeUtilSkillShow = 0.5f;

	private Vector3 m_AimObjectThrow = new Vector3 (-8.28f, 4.25f, 0);

	public static OilKing_Animation Instance {
		get {
			if (m_Instance == null) {
				m_Instance = GameObject.FindObjectOfType<OilKing_Animation> ();
			}
			return m_Instance;
		}
	}

	void Awake ()
	{
		m_Instance = this;
	}

	void Start ()
	{
		m_CheckFirst = false;
		checkStartUtilSkill = false;
		m_PosPause = 0;
	}

	void Update ()
	{
		if (checkStartUtilSkill && !m_CheckFirst)
			StartCoroutine (UtilSkillAnimSecond (OilKingGamePlay.Instance.utilSkillTransform, 
			                                     OilKing_BlockManager.Instance.lstBlocks));
	}

	IEnumerator UtilSkillAnim (Transform utilImage)
	{
		OilKing_PlayUI.Instance.RaycastTargetButtons (false);
		m_PosBeginY = utilImage.position.y;
		m_PosEndY = OilKing_BlockManager.Instance.lstBlocks[OilKing_BlockManager.Instance.GetNumberBlocks() - 1]
		                                .transform.position.y;
		m_Distance = (m_PosBeginY - m_PosEndY) / (float)(OilKing_BlockManager.Instance.GetNumberBlocks() - 1);
		utilImage.DOScale (1.4f, .15f);
		yield return new WaitForSeconds (.15f);
		checkStartUtilSkill = true;
	}


	IEnumerator UtilSkillAnimSecond (Transform utilImage, List<OilKing_Block> lstBlocks)
	{
		OilKingGamePlay.Instance.animDrill.RunAnim();
		m_CheckFirst = true;
		int numBlocks = lstBlocks.Count;
		for (int i = m_PosPause; i < numBlocks; i++) {
			if (Header.Instance.isPause) {
				m_CheckFirst = false;
				yield break;
			}
			//we have 6 block
			utilImage.DOLocalMoveY (m_PosBeginY - (i + 1) * m_Distance, m_TimeUtilSkillShow / (numBlocks  - 1));
			yield return new WaitForSeconds (m_TimeUtilSkillShow / (numBlocks - 1));
			lstBlocks [i].gameObject.SetActive (false);
			lstBlocks [i].gameObject.SetActive (true);

			m_PosPause = i;

			if (m_PosPause == numBlocks - 1) {
				utilImage.DOScale (0f, .1f);
				yield return new WaitForSeconds (.1f);
				utilImage.transform.position = new Vector3(utilImage.transform.position.x, m_PosBeginY, utilImage.transform.position.z);

				checkStartUtilSkill = false;
				m_PosPause = 0;
				m_CheckFirst = false;
				OilKingGamePlay.Instance.animDrill.StopAnim();
				if (!OilKing_PlayUI.Instance.checkFreezeChar)
					OilKing_PlayUI.Instance.RaycastTargetButtons(true);


				OilKingGamePlay.Instance.TurnOnButton ();
			}
		}

	}

	public void ExecuteUtilSkillAnim ()
	{
		StartCoroutine (UtilSkillAnim (OilKingGamePlay.Instance.utilSkillTransform));
	}

	IEnumerator FeverAnim (bool isAppear)
	{
		Header.Instance.btnPause.interactable = false;
		if (isAppear) {
			//cutin image appear
			OilKing_PlayUI.Instance.feverTransform [6].DOScale (1f, .5f);
			yield return new WaitForSeconds (1.4f);
			//cutin image exit
			OilKing_PlayUI.Instance.feverTransform [6].DOScale (0f, .3f);
			yield return new WaitForSeconds (.3f);
			OilKingGamePlay.Instance.InvisibleCharacters(false);
			//image character show
			//image at 2,3 appear
			OilKing_PlayUI.Instance.feverTransform [2].DOScale (0.85f, .1f);
			OilKing_PlayUI.Instance.feverTransform [3].DOScale (0.85f, .1f);
			yield return new WaitForSeconds (.3f);
			//image at 1,4 appear
			OilKing_PlayUI.Instance.feverTransform [1].DOScale (0.85f, .1f);
			OilKing_PlayUI.Instance.feverTransform [4].DOScale (0.85f, .1f);
			yield return new WaitForSeconds (.3f);
			//image at 0,5 appear
			OilKing_PlayUI.Instance.feverTransform [0].DOScale (0.85f, .1f);
			OilKing_PlayUI.Instance.feverTransform [5].DOScale (0.85f, .1f);
			yield return new WaitForSeconds (.3f);
		} else {
			for (int i = 0; i < 6; i++) {
				OilKing_PlayUI.Instance.feverTransform [i].DOScale (0f, .3f);
			}
			OilKing_BlockManager.Instance.ReplaceBlockByFever (.1f);
			yield return new WaitForSeconds (0.5f);
		}
		Header.Instance.btnPause.interactable = true;
	}

	public void ExecuteFeverAnim (bool isAppear)
	{
		StartCoroutine (FeverAnim (isAppear));
	}

	IEnumerator ObjectThrowAnim (Transform objectThrow)
	{
		objectThrow.DOJump (m_AimObjectThrow, 3f, 1, 1, false);
		yield return new WaitForSeconds (1f);
	}

	public void ExecuteObjectThrow (Transform objectThrow)
	{
		StartCoroutine (ObjectThrowAnim (objectThrow));
	}
}
