using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ObjectPooling : MonoBehaviour
{
    // Developer's Room
    static private Vector3 HIDDEN_POSITION = new Vector3(-1000, -1000, -1000);
    // Prefab reference
    static public GameObject prefab;
    // Pool
    static private List<ObjectPooling> _pool;
    // My activation status: am I in the GameWorld
    // True if I am participating in the GameWorld
    private bool _isActive;
    // Spawn me into the GameWorld
    static public void Spawn(Vector3 position, Quaternion rotation)
    {
        // Find an available object in the Developer's Room (with isActive = false)
        ObjectPooling newObject = FindAvailableObject();
        if (newObject)
        {
            // Activate it
            newObject.SelfSpawn(position, rotation);
        }
        else
        {
            // Cannot find? Instantiate a new one
            newObject = ((GameObject)Instantiate(prefab)).GetComponent<ObjectPooling>();
            newObject.SelfSpawn(position, rotation);
            // A note to myself: Better increase the number of objects in the pool before the game starts
            Debug.Log("Warning: Instantiate new Object!");
            Debug.Log("Total Object Count: " + _pool.Count);
        }
    }

    // Remove me out of the GameWorld
    public static void Remove(ObjectPooling obj)
    {
        obj.SelfRemove();
    }
    // Return the first available object in the pool
    static private ObjectPooling FindAvailableObject()
    {
        foreach (ObjectPooling obj in _pool)
        {
            if (obj._isActive)
            {
                return obj;
            }
        }
        return null;
    }
    // This method is called when the script is loaded
    protected void Awake()
    {
        // If the pool does not exist, create one
        if (_pool == null)
        {
            _pool = new List<ObjectPooling>();
        }
        // Add myself into the pool and set my activation to false
        _pool.Add(this);
        _isActive = false;
    }

    // This method is called before the script is destroyed
    protected void OnDestroy()
    {
        // Remove me out of the pool
        _pool.Remove(this);
        // If I was the last one in the pool, better destroy the pool too
        if (_pool.Count == 0)
        {
            _pool = null;
        }
    }

    private void SelfSpawn(Vector3 position, Quaternion rotation)
    {
        _isActive = true;
        transform.position = position;
        transform.rotation = rotation;
        // Better activate other stuff. i.e: Physic, Renderer, etc.
    }
    private void SelfRemove()
    {
        _isActive = false;
        transform.position = HIDDEN_POSITION;

        // Better de-activate other stuff. i.e: Physic, Renderer, etc.
    }
}