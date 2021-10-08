using UnityEngine;
using System.Collections;

public class ParticleSorting : MonoBehaviour {

    public bool IgnoreTimeScale = false;
    private double lastTime;
    private ParticleSystem particle;
	// Use this for initialization
	void Start () {
        GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = "14";
        lastTime = Time.realtimeSinceStartup;
	}
    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if(IgnoreTimeScale)
        {
            float deltaTime = Time.realtimeSinceStartup - (float)lastTime;
            particle.Simulate(deltaTime, true, false); //last must be false!!
            lastTime = Time.realtimeSinceStartup;
        }
    }
}
