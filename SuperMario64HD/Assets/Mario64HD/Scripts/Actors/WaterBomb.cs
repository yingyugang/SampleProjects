﻿using UnityEngine;
using System.Collections;

public class WaterBomb : SimpleStateMachine {

    enum WaterBombStates { Bounce, Spring, Exploding }

    [HideInInspector]
    public Transform Target;

    public LayerMask Ground;
    public Transform Art;
    public DynamicShadow Shadow;

    public AudioClip BounceSound;

    public GameObject DeathEffect;

    public int MaxBounces = 2;
    public float Gravity = 20.0f;
    public float BounceHeight = 4.0f;
    public float BounceSpeed = 1.0f;
    public float Radius = 0.5f;

    private Vector3 moveDirection;

    private int currentBounce = 0;
    private RaycastHit currentGround;

	void Start () {
        if (!Target)
            Target = GameObject.FindWithTag("Player").transform;

        currentState = WaterBombStates.Bounce;
	}

    public void PlayerTookDamage()
    {
        if ((WaterBombStates)currentState != WaterBombStates.Exploding)
        {
            Instantiate(DeathEffect, transform.position + Vector3.up * Radius, Quaternion.identity);

            Death();
        }
    }

    void Bounce_EnterState()
    {
        Art.GetComponent<Animation>().Play("bounce");
    }

	void Bounce_Update () {

        moveDirection -= Vector3.up * Gravity * Time.deltaTime;

        ProbeGround();

        if (WillBeGrounded() && moveDirection.y < 0)
        {
            ClampToGround();

            if (currentBounce < MaxBounces)
            {
                currentBounce++;
                currentState = WaterBombStates.Spring;
                return;
            }
            else
            {
                currentState = WaterBombStates.Exploding;
                return;
            }
        }
        else
        {
            transform.position += moveDirection * Time.deltaTime;
        }
	}

    void Spring_EnterState()
    {
        Art.GetComponent<Animation>().Play("spring");

        GetComponent<AudioSource>().PlayOneShot(BounceSound);

        moveDirection = Vector3.zero;
    }

    void Spring_Update()
    {
        Shadow.Scale = Art.localScale.x;

        if (SuperMath.Timer(timeEnteredState, Art.GetComponent<Animation>()["spring"].length))
        {
            moveDirection = (Target.position - transform.position).normalized * BounceSpeed;

            moveDirection += SuperMath.CalculateJumpSpeed(BounceHeight, Gravity) * Vector3.up;

            currentState = WaterBombStates.Bounce;

            return;
        }
    }

    void Spring_ExitState()
    {
        Shadow.Scale = 1.0f;
    }

    void Exploding_EnterState()
    {
        Instantiate(DeathEffect, transform.position + Vector3.up * Radius, Quaternion.identity);

        Art.GetComponent<Animation>().Play("explode");
    }

    void Exploding_Update()
    {
        Shadow.Scale = Art.localScale.x;

        if (SuperMath.Timer(timeEnteredState, Art.GetComponent<Animation>()["explode"].length))
        {
            Death();
        }
    }

    void ProbeGround()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -Vector3.up, out hit, Mathf.Infinity, Ground))
        {
            currentGround = hit;
        }
    }

    bool WillBeGrounded()
    {
        if (currentGround.distance < Mathf.Abs(moveDirection.y) * Time.deltaTime)
        {
            return true;
        }

        return false;
    }

    void ClampToGround()
    {
        transform.position -= Vector3.up * currentGround.distance;
    }

    void Death()
    {
        Destroy(gameObject);
    }
}
