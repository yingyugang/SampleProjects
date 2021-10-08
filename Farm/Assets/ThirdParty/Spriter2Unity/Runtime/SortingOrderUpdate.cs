using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SortingOrderUpdate : MonoBehaviour
{
    public float SortingOrder = 0;

    private Renderer cachedRenderer;

    private void Start()
    {
        cachedRenderer = GetComponent<Renderer>();
        if (!cachedRenderer)
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        //if (CommonObjectScript.nameScenes.Equals("Farm"))
        //{
        //    cachedRenderer.sortingOrder = (int)SortingOrder;
        //}
        //else
        cachedRenderer.sortingOrder = 0;// (int)SortingOrder;
    }
}
