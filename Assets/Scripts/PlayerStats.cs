﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float playerHealth;
    public bool isDead; //This is for stuff later on the UI
    //TODO
    //Come up with some more types of stats

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = 250f;
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        deadCheck();
    }

    public void selfDamage(float damage)
    {
        playerHealth -= damage;
    }

    public void deadCheck()
    {
        if (playerHealth <= 0)
        {
            isDead = true;
            Destroy(gameObject);
        }
    }
}
