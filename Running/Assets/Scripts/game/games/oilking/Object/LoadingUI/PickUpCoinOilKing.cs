using UnityEngine;
using System.Collections;

public class PickUpCoinOilKing : MonoBehaviour
{

	private Rigidbody2D m_Rigidbody2D;
	public SpriteRenderer spriteRenderer;

	private float m_Speed = 10.0f;

	void OnEnable ()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D> ();
		m_Rigidbody2D.velocity = Vector2.down * m_Speed;
		float dir = Random.Range (-1f, 1f);
		float Mul = 1.5f;
		m_Rigidbody2D.velocity = new Vector2 (dir * Random.Range (1f, 5f) * Mul, Mul * Random.Range (1f, 5f));

		if (spriteRenderer.sprite == null) {
			spriteRenderer.sprite = OilKingAssetLoader.s_Instance.getSprite (TypeSprite.CoinCollection);
		}
	}
}
