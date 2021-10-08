﻿using UnityEngine;
using System.Collections;

public class CoinDrop : MonoBehaviour {

    public LayerMask Ground;

    public int Value = 1;
    public float LifeTime = 10.0f;
    public float FlickerTime = 3.0f;
    public float InvinciblePeriod = 0.2f;
    public float Radius = 0.5f;
    public float TossHeight = 3.0f;
    public float Gravity = 20.0f;
    public float Elasticity = 0.5f;
    public float SpinSpeed = 360.0f;
    public GameObject DeathEffect;
    public Transform Art;
    public GameObject BounceSound;
    public GameObject AppearSound;

    private float moveSpeed = 0;

    private bool sitting;

    private float spawnTime;

	void Start () {
        moveSpeed = SuperMath.CalculateJumpSpeed(TossHeight, Gravity);

        spawnTime = Time.time;

        StartCoroutine(FlickerAndDie());

        Instantiate(AppearSound, transform.position, Quaternion.identity);
	}

    void Update()
    {
        if (!sitting)
        {
            moveSpeed -= Gravity * Time.deltaTime;

            RaycastHit hit;

            if (moveSpeed < 0 && Physics.SphereCast(transform.position, Radius, -Vector3.up, out hit, Mathf.Abs(moveSpeed) * Time.deltaTime, Ground))
            {
                transform.position -= Vector3.up * hit.distance;

                if (Mathf.Abs(moveSpeed) < 1.0f)
                {
                    sitting = true;
                }
                else
                {
                    moveSpeed = -moveSpeed * Elasticity;

                    Instantiate(BounceSound, transform.position, Quaternion.identity);
                }
            }

            transform.position += moveSpeed * Vector3.up * Time.deltaTime;
        }

        Art.rotation *= Quaternion.AngleAxis(SpinSpeed * Time.deltaTime, transform.up);
    }

    void OnTriggerEnter(Collider col)
    {
        if (SuperMath.Timer(spawnTime, InvinciblePeriod) && col.gameObject.tag == "Player")
        {
            Death();
        }
    }

    IEnumerator FlickerAndDie()
    {
        yield return new WaitForSeconds(LifeTime - FlickerTime);

        StartCoroutine(Flicker(20));

        yield return new WaitForSeconds(FlickerTime * 0.5f);

        StartCoroutine(Flicker(40));

        yield return new WaitForSeconds(FlickerTime * 0.5f);

        Destroy(gameObject);
    }

    IEnumerator Flicker(float flickersPerSecond)
    {
        float i = 0;
        float lastFlicker = 0;
        float flickerFrequency = FlickerTime / flickersPerSecond;

        while (i < FlickerTime * 0.5f)
        {
            if (i > lastFlicker + flickerFrequency)
            {
                Art.GetComponent<Renderer>().enabled = !Art.GetComponent<Renderer>().enabled;

                lastFlicker = i;
            }

            i += Time.deltaTime;

            yield return 0;
        }
    }

    void Death()
    {
        GameObject.FindObjectOfType<GameMaster>().AddCoin(Value);
        GameObject.FindObjectOfType<MarioStatus>().AddHealth(1);

        if (DeathEffect)
            Instantiate(DeathEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
