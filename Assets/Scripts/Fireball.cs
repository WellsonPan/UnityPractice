﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float flightSpeed;
    public float directDamage;
    public float timeAlive;

    private float currentTime;

    public GameObject blast;

    public const float gravity = -.1962f;
    private bool isGrounded;
    public bool isGrenade;
    public float friction;
    public float wallFrictionCoefficient;
    private float wallFriction;

    public Transform groundCheck;
    public float groundDistance;
    public Rigidbody myRigidbody;

    public LayerMask groundMask;

    private GameObject arm;
    private bool isQuitting;

    Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        arm = GameObject.Find("PlayerArm");
        direction = arm.transform.forward;
        currentTime = Time.time;
        wallFriction = friction * wallFrictionCoefficient;
        myRigidbody = GetComponent<Rigidbody>();
        if(isGrenade)
        {
            myRigidbody.useGravity = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrenade)
        {
            isOnGround();
        }

        if(isGrounded)
        {
            direction *= friction;
        }

        //transform.Translate(direction * flightSpeed * Time.deltaTime, Space.Self);

        if(Time.time > currentTime + timeAlive || (isGrounded && !isGrenade))
        {
            Instantiate(blast, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(direction * flightSpeed * Time.fixedDeltaTime, Space.Self);
    }

    void isOnGround()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "SummonedWall" && !isGrenade)
        {
            Destroy(gameObject);
            collisionInfo.gameObject.GetComponent<Wall>().OnHit(directDamage);
        }
        else if (collisionInfo.collider.tag == "Enemy" && !isGrenade)
        {
            Destroy(gameObject);
        }
        else if(!isGrenade)
        {
            Destroy(gameObject);
        }
        else
        {
            float zDirAbs = Mathf.Abs(collisionInfo.GetContact(0).normal.z);
            float xDirAbs = Mathf.Abs(collisionInfo.GetContact(0).normal.x);
            
            if (zDirAbs > xDirAbs && !isGrounded)
            {
                direction = new Vector3(direction.x * wallFriction, direction.y, direction.z * -wallFriction);
            }
            else if (xDirAbs > zDirAbs && !isGrounded)
            {
                direction = new Vector3(direction.x * -wallFriction, direction.y, direction.z * wallFriction);
            }
            else
            {
                
            }
            //Debug.Log(collisionInfo.GetContact(0).normal);
        }
        //else if(collisionInfo.collider.tag == "Enemy")
        //{
        //    Destroy(gameObject);

        //}
    }

    void OnDestroy()
    {
        if (!isQuitting)
        {
            Instantiate(blast, transform.position, Quaternion.identity);
        }
    }

    private void OnApplicationQuit()
    {
        isQuitting = true;
    }
}
