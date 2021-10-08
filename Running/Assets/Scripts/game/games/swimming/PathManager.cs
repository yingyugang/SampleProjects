using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathManager : MonoBehaviour 
{
	public Transform[] topNodes;
	public Transform[] botNodes;
	public Transform[] zigzagNodes;

	private static PathManager m_Instance;
	public static PathManager Instance
	{
		get
		{
			return m_Instance;
		}
	}

	void Awake()
	{
		m_Instance = this;
	}

	public List<Vector3> RandomCurvePathDown(Vector3 pos)
	{
		List<Vector3> path = new List<Vector3>();
		path.Add(pos);
		path.Add(new Vector3(0, 0, 0));
		path.Add(botNodes[Random.Range(0, 2)].position);
		path.Add(new Vector3(0, -20, 0));

		return path;
	}

	public List<Vector3> RandomCurvePathUp(Vector3 pos)
	{
		List<Vector3> path = new List<Vector3>();
		path.Add(pos);
		path.Add(new Vector3(0, 0, 0));
		path.Add(topNodes[Random.Range(0, 2)].position);
		path.Add(new Vector3(0, 20, 0));

		return path;
	}

	public List<Vector3> ZigZagPathDown(Vector3 pos)
	{
		List<Vector3> path = new List<Vector3>();
		path.Add(pos);
		for (int i=0; i<zigzagNodes.Length; i++)
		{
			Vector3 v = zigzagNodes[i].position;
			path.Add(v);
		}
		path.Add(new Vector3(0, -20, 0));

		return path;
	}

	public List<Vector3> ZigZagPathUp(Vector3 pos)
	{
		List<Vector3> path = new List<Vector3>();
		path.Add(pos);
		for (int i=zigzagNodes.Length-1; i>0; i--)
		{
			Vector3 v = zigzagNodes[i].position;
			path.Add(v);
		}
		path.Add(new Vector3(0, +20, 0));

		return path;
	}
}
