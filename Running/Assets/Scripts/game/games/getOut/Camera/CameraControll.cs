using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class CameraControll : MonoBehaviour
{
	
	private float m_LimitXRight;
	private float m_LimitXLeft;

	private Vector3 velocity = Vector3.zero;
	public Transform target;
	public float smoothTime = 0.15f;
	public bool horizontalMaxEnabled = false;
	public float horizontalMax = 0f;
	public bool horizontalMinEnabled = false;
	public float horizontalMin = 0f;
	public float sizeHeader = 2.35f;
	private float PositionYCamera;


	void Start()
	{
		//set position of camera is on the limit left and follow the player
		m_LimitXLeft = -LoadMap.ScreenSize.x - (LoadMap.ScreenSize.y-(Map.WIDTH/2+2f) * Map.SizeBlock - sizeHeader);
		m_LimitXRight = -LoadMap.ScreenSize.x+ (Map.HEIGHT-1)*Map.SizeBlock + (LoadMap.ScreenSize.y-(Map.WIDTH/2+2f) * Map.SizeBlock - sizeHeader); 
		PositionYCamera = LoadMap.ScreenSize.y - (Map.WIDTH/2) * Map.SizeBlock;
		transform.position = new Vector3(LoadMap.ScreenSize.x+m_LimitXLeft,PositionYCamera,transform.position.z);
	}

	// Update is called once per frame
	void Update ()
	{
		if (target) {
			Vector3 targetPosition = target.position;
			targetPosition.y = PositionYCamera;
			if ((transform.position.x - LoadMap.ScreenSize.x) > m_LimitXLeft 
				&&(transform.position.x + LoadMap.ScreenSize.x) > m_LimitXRight 
				&& (target.transform.position.x -  LoadMap.ScreenSize.x) > m_LimitXLeft 
				&&(target.transform.position.x + LoadMap.ScreenSize.x) > m_LimitXRight)
			{
				if (horizontalMinEnabled && horizontalMaxEnabled) {
					targetPosition.x = Mathf.Clamp (target.position.x, horizontalMin, horizontalMax);
				} else if (horizontalMinEnabled) {
					targetPosition.x = Mathf.Clamp (target.position.x, horizontalMin, target.position.x);
				} else if (horizontalMaxEnabled) {
					targetPosition.x = Mathf.Clamp (target.position.x, target.position.x, horizontalMax);
				}
			} else {
				if (target.transform.position.x - LoadMap.ScreenSize.x < m_LimitXLeft)
					targetPosition.x = LoadMap.ScreenSize.x + m_LimitXLeft;
				else
					if (target.transform.position.x + LoadMap.ScreenSize.x > m_LimitXRight)
						targetPosition.x = m_LimitXRight - LoadMap.ScreenSize.x;
			}
			
			targetPosition.z = transform.position.z;
			transform.position = Vector3.SmoothDamp (transform.position, targetPosition, ref velocity, smoothTime);
		}

		if(transform.position.x + LoadMap.ScreenSize.x > m_LimitXRight)
			transform.position = new Vector3(m_LimitXRight - LoadMap.ScreenSize.x,transform.position.y,transform.position.z);
	}
}
