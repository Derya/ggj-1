﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    [SerializeField]
    GameObject my_own_damn_self;

    private float timer;

    void Start()
    {
        timer = 0;
    }


    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 6)
        {
            Destroy(my_own_damn_self);
        }
    }
}
