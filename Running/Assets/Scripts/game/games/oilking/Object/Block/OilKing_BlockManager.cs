using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Diagnostics;


public class OilKing_BlockManager : MonoBehaviour
{
	public List<OilKing_Block> lstBlocks;

	public int speedMoveBlock = 40;

	public float waitTime = 2f;

	//distance when body shaked
	public float disBody = 0.2f;

	public Vector3[] posBlocks;

	private float m_TimeShake;

	//block at element 0 of lstBlocks
	private OilKing_Block blockAhead;

	private int numBlocks;

	private Vector3 angleDefault = new Vector3(0, 0, 0);
	private Vector3 angleFever = new Vector3(0, 0, 330f);
	private Vector3 scaleDefault = new Vector3(1, 1, 1);
	private Vector3 scaleFever = new Vector3(1.5f, 1.5f, 1.5f);

	private static OilKing_BlockManager m_Instance;

	public static OilKing_BlockManager Instance {
		get {
			if (m_Instance == null) {
				m_Instance = GameObject.FindObjectOfType<OilKing_BlockManager> ();
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
		Init ();
	}

	public void Init ()
	{
		numBlocks = lstBlocks.Count;
		posBlocks = new Vector3[numBlocks];

		float tmpDis = (lstBlocks[0].transform.position.y - lstBlocks[numBlocks - 1].transform.position.y) / (float)(numBlocks - 1);
		for (int i = 0; i < numBlocks ; i++)
		{
			posBlocks[i].y = lstBlocks[0].transform.position.y - tmpDis * i;
		}

		blockAhead = lstBlocks [0];
		ChangeSortingOrderBlocks ();
	}

	void MovementBlocks ()
	{
		if (OilKingGamePlay.Instance.isHit || OilKingGamePlay.Instance.isThrow) {
			OilKingGamePlay.Instance.isHit = false;
			OilKingGamePlay.Instance.isThrow = false;

			StartCoroutine (WaitForMoveBlock (waitTime));
		}
	}

	void Update ()
	{
		MovementBlocks ();
		BlockShake ();
	}

	IEnumerator WaitForMoveBlock (float waitTime)
	{
		if (GetBlockAhead ().canMove) {
			MoveBlock ();
		}

		yield return new WaitForSeconds (waitTime);

		lstBlocks [0].gameObject.SetActive (false);
		lstBlocks[0].transform.rotation = Quaternion.Euler(angleDefault);
		lstBlocks[0].transform.localScale = scaleDefault;

		lstBlocks.RemoveAt (0);

		GameObject nextBlock = PoolManagerOilKing.s_Instance.GetFreeObject (OilKingConfig.POOL_NAME_BLOCK_PREFAPS, posBlocks [numBlocks - 1]);


		lstBlocks.Add (nextBlock.GetComponent<OilKing_Block> ());

		blockAhead = lstBlocks [0];

		ChangeSortingOrderBlocks ();

	}

	void MoveBlock ()
	{
		for (int i = 1; i < numBlocks; i++) {
			lstBlocks [i].transform.position =
				Vector3.Lerp (lstBlocks [i].transform.position, posBlocks [i - 1], Time.deltaTime * speedMoveBlock);
		}
	}

	//get block at element 0 of lstBlocks
	public OilKing_Block GetBlockAhead ()
	{
		return blockAhead;
	}

	//generate a random Item when block Treasure broken
	/*
	IEnumerator GenerateItem (float waitTime)
	{
		lstBlocks [0].gameObject.SetActive (false);

		lstBlocks.RemoveAt (0);

		GameObject nextItem = PoolManagerOilKing.s_Instance.GetFreeObject (OilKingConfig.POOL_NAME_ITEM_PREFAPS, posBlocks [0]);

		lstBlocks.Insert (0, nextItem.GetComponent<OilKing_Item> ());

		blockAhead = lstBlocks [0];

		yield return null;// new WaitForSeconds (waitTime);
	}
	*/
	public void ExecuteGenerateItem ()
	{
		lstBlocks[0].gameObject.SetActive(false);
		lstBlocks.RemoveAt(0);
		GameObject nextItem = PoolManagerOilKing.s_Instance.GetFreeObject(OilKingConfig.POOL_NAME_ITEM_PREFAPS, posBlocks[0]);
		lstBlocks.Insert(0, nextItem.GetComponent<OilKing_Item>());
		blockAhead = lstBlocks[0];
	}

	public void ReplaceBlockByFever (float waitTime)
	{
		OilKing_PlayUI.Instance.RaycastTargetButtons (false);
		//yield return new WaitForSeconds (waitTime);

		for (int i = 0; i < numBlocks; i++) {
			lstBlocks [i].gameObject.SetActive (false);
		}

		//yield return new WaitForSeconds (waitTime);

		for (int i = 0; i < numBlocks; i++) {
			lstBlocks [i].gameObject.SetActive (true);
		}

		blockAhead = lstBlocks [0];
	}


	//public void ExecuteReplaceBlockByFever (float waitTime)
	//{
	//	StartCoroutine (ReplaceBlockByFever (waitTime));
	//}

	public Vector3 GetPosBlock ()
	{
		return posBlocks [0];
	}

	void BlockShake ()
	{
		m_TimeShake -= Time.deltaTime;
		if (m_TimeShake > 0 && lstBlocks [0].canMove) {
			float posX = Random.Range (-disBody, disBody);
			float posY = Random.Range (-disBody, disBody);
			float posZ = Random.Range (-disBody, disBody);
			lstBlocks [0].transform.position = new Vector3 (posBlocks [0].x + posX, posBlocks [0].y + posY, posBlocks [0].z + posZ);
		} else {
			lstBlocks [0].transform.position = posBlocks [0];
		}
	}

	public void ResetTimeShake ()
	{
		m_TimeShake = .1f;
	}

	void ChangeSortingOrderBlocks ()
	{
		for (int i = 0; i < numBlocks; i++) {
			lstBlocks [i].GetComponent<SpriteRenderer> ().sortingOrder = i+1;
			lstBlocks [i].transform.position = posBlocks [i];
			if(OilKingGamePlay.Instance.isFever)
			{
				lstBlocks[i].transform.rotation = Quaternion.Euler(angleFever);
				lstBlocks[i].transform.localScale = scaleFever;
			}
		}
	}

	public int GetNumberBlocks()
	{
		return lstBlocks.Count;
	}

	public void ResetRotationBlocks()
	{
		for (int i = 0; i < numBlocks; i++)
		{
			lstBlocks[i].transform.rotation = Quaternion.Euler(angleDefault);
			lstBlocks[i].transform.localScale = scaleDefault;
		}
	}
}
