using UnityEngine;
using System.Collections.Generic;

// Use this to custom list object.
[System.Serializable]
public class PrefabParticles
{
	[SerializeField]
	public string Name;         // Object's name.
	public int Total;           // Total object.
	[SerializeField]
	public GameObject Prefab;   // Prefab object
}

// Use this to classify type of object.
[System.Serializable]
public class TypePool
{
	[SerializeField]
	public string Name;     // Type name
	[SerializeField]
	public PrefabParticles[] ListObject;
}

public class PoolManagerOilKing : MonoBehaviour
{
	public TypePool[] typesOfPool;
	private Dictionary<string, List<GameObject>> m_ListPools;
	private bool m_Initialized = false;
	public static PoolManagerOilKing s_Instance;

	void Awake()
	{
		s_Instance = this;
		Init();
	}

	public void ResetAll()
	{
		if (m_ListPools != null)
		{
			foreach (var dic in m_ListPools)
			{
				foreach (var item in dic.Value)
				{
					if (item != null)
					{
						item.SetActive(false);
					}
				}
			}
		}
	}

	public void DesSpawn(string name)
	{
		if (m_ListPools != null)
		{
			List<GameObject> list = m_ListPools[name];
			foreach (var item in list)
			{
				if (item != null && item.activeSelf)
				{
					item.SetActive(false);
				}
			}
		}
	}
	// Use this for initialization
	public void Init()
	{
		if (m_Initialized) return;
		m_ListPools = new Dictionary<string, List<GameObject>>();

		// Type of pool
		foreach (TypePool itempool in typesOfPool)
		{
			if (itempool != null && itempool.ListObject != null)
			{
				int length = itempool.ListObject.Length;
				for (int i = 0; i < length; i++)
				{
					// Checking object is exist in pool?
					bool isExist = m_ListPools.ContainsKey(itempool.ListObject[i].Prefab.name);
					if (!isExist) // If not exist
					{
						List<GameObject> listobject = new List<GameObject>();
						int total = itempool.ListObject[i].Total;
						for (int j = 0; j < total; j++)
						{
							GameObject itemobject = Instantiate(itempool.ListObject[i].Prefab);
							itemobject.transform.SetParent(transform);

							itemobject.SetActive(false);
							listobject.Add(itemobject);
						}
						// Note: Each gameobject has only one name.
						m_ListPools.Add(itempool.ListObject[i].Prefab.name, listobject);
					}
				}
			}
		}
		m_Initialized = true;
	}

	// Getting free object + set position + set active
	public GameObject GetFreeObject(string name, Vector3 pos, bool isActive = true)
	{
		GameObject itemobject = GetFreeObject(name);
		if (itemobject == null)
		{
			//Debug.Log("GetFreeObject null: " + name);
		}

		itemobject.transform.position = pos;
		itemobject.SetActive(isActive);
		ParticleSystem particlesystem = itemobject.GetComponent<ParticleSystem>();
		if (particlesystem != null)
		{
			particlesystem.Stop();
			particlesystem.Play();
		}
		return itemobject;
	}

	public GameObject GetFreeObject(string name)
	{
		if (m_ListPools != null)
		{
			// Get list objects base on tagKey
			List<GameObject> listobject = m_ListPools[name];
			if (listobject != null)
			{
				foreach (var item in listobject)
				{
					if (item != null && item.activeSelf == false)
					{

						return item;
					}
				}
				// Spawn new gameobject if any free gameobject is not found at the moment
				foreach (TypePool itempool in typesOfPool)
				{
					if (itempool != null && itempool.ListObject != null)
					{
						int length = itempool.ListObject.Length;
						for (int i = 0; i < length; i++)
						{
							if (itempool.ListObject[i].Prefab.name == name)
							{
								GameObject itemobject = Instantiate(itempool.ListObject[i].Prefab);
								itemobject.transform.parent = transform;
								itemobject.SetActive(false);
								listobject.Add(itemobject);
								return itemobject;
							}
						}
					}
				}
			}
			else
			{
				return null;
			}
		}
		return null;
	}
}